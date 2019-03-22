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

namespace Project.Service.OA.BuessOut
{
    public class OABuessOutAppService : ApplicationService, IOABuessOutAppService
    {
        public readonly IRepository<OABuessOut, Guid> _oaBuessOutRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OABuessOutAppService(IRepository<OABuessOut, Guid> oaBuessOutRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaBuessOutRepository = oaBuessOutRepository;
            _workFlowTaskManager=workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OABuessOutInputDto input)
        {
            var model = input.MapTo<OABuessOut>();
            var hours = (model.EndTime.Value - model.StartTime.Value).Hours;
            if (hours < 0)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不能晚于结束时间。");
            }
            model.Status = 0;
            var ret = _oaBuessOutRepository.Insert(model);

            return new InitWorkFlowOutput() {  InStanceId=ret.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OABuessOutDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = _oaBuessOutRepository.Get(id);
            return model.MapTo<OABuessOutDto>();
        }
        [AbpAuthorize]
        public PagedResultDto<OABuessOutListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaBuessOutRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }

            var count = ret.Count();
            var list = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OABuessOutListDto>>();
            foreach (var item in list)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OABuessOutListDto>(count, list);
        }

        public void Update(OABuessOutInputDto input)
        {
            var hours = (input.EndTime.Value - input.StartTime.Value).Hours;
            if (hours < 0)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "开始时间不能晚于结束时间。");
            }
            var ret = _oaBuessOutRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaBuessOutRepository.Update(ret);
        }
    }
}
