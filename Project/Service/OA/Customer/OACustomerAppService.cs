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

namespace Project
{
    public class OACustomerAppService : ApplicationService, IOACustomerAppService
    {
        private readonly IRepository<OACustomer, Guid> _oaCustomerRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OACustomerAppService(IRepository<OACustomer, Guid> oaCustomerRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaCustomerRepository = oaCustomerRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OACustomerInputDto input)
        {
            var model = input.MapTo<OACustomer>();
            model.Status = 0;
            _oaCustomerRepository.InsertOrUpdate(model);
            return new InitWorkFlowOutput() { InStanceId=model.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OACustomerDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaCustomerRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OACustomerDto>();
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OACustomerListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaCustomerRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Name.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OACustomerListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OACustomerListDto>(total, model);
        }

        public void Update(OACustomerInputDto input)
        {
            var ret = _oaCustomerRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaCustomerRepository.Update(ret);
        }
    }
}
