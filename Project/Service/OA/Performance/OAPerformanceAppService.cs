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
using ZCYX.FRMSCore.Model;

namespace Project.Service.OA.Performance
{
    public class OAPerformanceAppService : ApplicationService, IOAPerformanceAppService
    {
        public readonly IRepository<OAPerformance, Guid> _oaPerformanceRepository;
        public readonly IRepository<OAPerformanceMain, Guid> _oaPerformanceMainRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAPerformanceAppService(IRepository<OAPerformance, Guid> oaPerformanceRepository,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            IRepository<OAPerformanceMain, Guid> oaPerformanceMainRepository, WorkFlowTaskManager workFlowTaskManager)
        // :base(oaPerformanceRepository)
        {
            _oaPerformanceRepository = oaPerformanceRepository;
            _oaPerformanceMainRepository = oaPerformanceMainRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAPerformanceInputDto input)
        {
           
           
               var model = input.MapTo<OAPerformance>();
            model.Status = 0;
            var ret = _oaPerformanceRepository.Insert(model);

            return new InitWorkFlowOutput() { InStanceId = ret.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAPerformanceDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _oaPerformanceRepository.GetAll().FirstOrDefault(ite => ite.Main_id == id && ite.CreatorUserId == AbpSession.UserId);

            var ret = model.MapTo<OAPerformanceDto>();
            var main = _oaPerformanceMainRepository.Get(model.Main_id.Value);
            ret.StartTime = main.StartTime;
            ret.EndTime = main.EndTime;
            return ret;
        }
        [AbpAuthorize]
        public PagedResultDto<OAPerformanceListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = from a in _oaPerformanceRepository.GetAll()
                      join b in _oaPerformanceMainRepository.GetAll() on a.Main_id equals b.Id
                      where a.CreatorUserId == AbpSession.UserId
                      orderby a.CreationTime descending
                      select new OAPerformanceListDto()
                      {
                          Title = a.Title,
                          AuditUser = a.AuditUser,
                          EndTime = b.EndTime,
                          FinishPersent = a.FinishPersent,
                          FinishTask = a.FinishTask,
                          Id = a.Id,
                          PlanTask = a.PlanTask,
                          Score = a.Score,
                          StartTime = b.StartTime,
                          Status = a.Status.Value,
                          Main_id = a.Main_id
                      };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey) || ite.FinishTask.Contains(input.SearchKey)|| ite.PlanTask.Contains(input.SearchKey));
            }

            var count = ret.Count();
            var list = ret.PageBy(input).ToList();
            foreach (var item in list)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAPerformanceListDto>(count, list);
        }

        public void Update(OAPerformanceInputDto input)
        {
            var model = _oaPerformanceRepository.Get(input.Id.Value);
            if (model != null)
            {
                input.Main_id = model.Main_id;
                input.Title = model.Title;
                input.MapTo(model);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "未找到该条数据。");
            }
            var ret = _oaPerformanceRepository.Update(model);
        }
    }
}
