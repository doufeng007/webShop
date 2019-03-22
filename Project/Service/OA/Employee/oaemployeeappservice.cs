using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application.Dto;
using ZCYX.FRMSCore.Authorization.Users;

namespace Project
{
    public class OAEmployeeAppService : ApplicationService, IOAEmployeeAppService
    { 
        public readonly IRepository<OAEmployee, Guid> _oaEmployeeRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IRepository<User, long> _userRepository;
        public OAEmployeeAppService(IRepository<OAEmployee, Guid> oaEmployeeRepository, WorkFlowTaskManager workFlowTaskManager, IRepository<User, long> userRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oaEmployeeRepository = oaEmployeeRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _userRepository = userRepository;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAEmployeeInputDto input)
        {
            var model = input.MapTo<OAEmployee>();
            model.Status = 0;
            var ret = _oaEmployeeRepository.InsertOrUpdate(model);
            return new InitWorkFlowOutput() { InStanceId = ret.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAEmployeeDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _oaEmployeeRepository.Get(id);
            var ret= model.MapTo<OAEmployeeDto>();
            ret.UserId_Name = _userRepository.Get(ret.UserId).Name;
            return ret;
        }
        [AbpAuthorize]
        public PagedResultDto<OAEmployeeListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaEmployeeRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false) {
                ret = ret.Where(ite => ite.Name.Contains(input.SearchKey));
            }

            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAEmployeeListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAEmployeeListDto>(total, model);
        }

        public void Update(OAEmployeeInputDto input)
        {
            var ret = _oaEmployeeRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaEmployeeRepository.Update(ret);
        }
    }
}
