using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class OAPerformanceMainAppService : ApplicationService, IOAPerformanceMainAppService
    {
        public readonly IRepository<OAPerformance, Guid> _oaPerformanceRepository;
        public readonly IRepository<OAPerformanceMain, Guid> _oaPerformanceMainRepository;
        public readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAPerformanceMainAppService(
            IRepository<OAPerformance, Guid> oaPerformanceRepository,
            IRepository<OAPerformanceMain, Guid> oaPerformanceMainRepository,
            IRepository<User, long> userRepository,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            )
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaPerformanceMainRepository = oaPerformanceMainRepository;
            _oaPerformanceRepository = oaPerformanceRepository;
            _userRepository = userRepository;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAPerformanceMainInputDto input)
        {
            var model = input.MapTo<OAPerformanceMain>();
            if (string.IsNullOrWhiteSpace(input.AuditUser))
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参与员工不能为空。");
            }
            _oaPerformanceRepository.Delete(ite => ite.Main_id == input.Id);
            model.Status = 0;
            model.CreatorUserId = AbpSession.UserId;
            var ret = _oaPerformanceMainRepository.Insert(model);
            var usersids = ret.AuditUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (var i = 0; i < usersids.Count; i++)
            {
                usersids[i] = usersids[i].Replace("u_", "");
            }
            var users = _userRepository.GetAll().Where(ite => usersids.Contains(ite.Id.ToString())).ToList();
            foreach (var u in users)
            {
                var p = new OAPerformance()
                {
                    Title = input.Title,
                    Main_id = ret.Id,
                    CreatorUserId = u.Id,
                    Status = 0
                };
                _oaPerformanceRepository.Insert(p);
            }
            return new InitWorkFlowOutput() { InStanceId = ret.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAPerformanceMainDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _oaPerformanceMainRepository.Get(id);
            var ret = model.MapTo<OAPerformanceMainDto>();
            var list = from a in _oaPerformanceRepository.GetAll()
                       join b in _userRepository.GetAll() on a.CreatorUserId.Value equals b.Id
                       where a.Main_id == model.Id
                       select new OAPerformanceDto()
                       {
                           Comment = a.Comment,
                           FinishPersent = a.FinishPersent,
                           FinishTask = a.FinishTask,
                           Id = a.Id,
                           LeaderComment = a.LeaderComment,
                           Main_id = a.Main_id,
                           PlanTask = a.PlanTask,
                           Score = a.Score,
                           Status = a.Status,
                           Surname = b.Surname,
                           Title = a.Title
                       };


            ret.OAPerformance = list.ToList();

            return ret;
        }
        [AbpAuthorize]
        public PagedResultDto<OAPerformanceMainListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaPerformanceMainRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }

            var count = ret.Count();
            var list = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAPerformanceMainListDto>>();
            foreach (var item in list)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAPerformanceMainListDto>(count, list);
        }

        public void Update(OAPerformanceMainInputDto input)
        {
            var model = _oaPerformanceMainRepository.Get(input.Id.Value);
            if (model == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到该条数据。");
            }
             model = input.MapTo(model);
            if (string.IsNullOrWhiteSpace(input.AuditUser))
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "参与员工不能为空。");
            }
            _oaPerformanceRepository.Delete(ite => ite.Main_id == input.Id);
            model.Status = 0;
            model.CreatorUserId = AbpSession.UserId;
            var ret = _oaPerformanceMainRepository.Update(model);
            var usersids = ret.AuditUser.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (var i = 0; i < usersids.Count; i++)
            {
                usersids[i] = usersids[i].Replace("u_", "");
            }
            var users = _userRepository.GetAll().Where(ite => usersids.Contains(ite.Id.ToString())).ToList();
            foreach (var u in users)
            {
                var p = new OAPerformance()
                {
                    Title = input.Title,
                    Main_id = ret.Id,
                    CreatorUserId = u.Id,
                    Status = 0
                };
                _oaPerformanceRepository.Insert(p);
            }
        }
    }
}
