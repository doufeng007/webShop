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

namespace Project.Service.OA.Pay
{
    public class OAPayAppService : ApplicationService, IOAPayAppService
    {
        private readonly IRepository<OAPay, Guid> _oaPayRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAPayAppService(IRepository<OAPay, Guid> oaPayRepository, IRepository<User, long> userRepository, WorkFlowTaskManager workFlowTaskManager,
            WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oaPayRepository = oaPayRepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAPayInputDto input)
        {
            var model = input.MapTo<OAPay>();
            model.Status = 0;
            _oaPayRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAPayDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaPayRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAPayDto>();
            model.UserName = _userRepository.Get(ret.CreatorUserId.Value).Surname;
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OAPayListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaPayRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAPayListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAPayListDto>(total, model);
        }

        public void Update(OAPayInputDto input)
        {
            var ret = _oaPayRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaPayRepository.Update(ret);
        }
    }
}
