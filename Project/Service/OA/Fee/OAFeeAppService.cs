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

namespace Project.Service.OA.Fee
{
    public class OAFeeAppService : ApplicationService, IOAFeeAppService
    {
        private readonly IRepository<OAFee, Guid> _oaFeeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAFeeAppService(IRepository<OAFee, Guid> oaFeeRepository, IRepository<User, long> userRepository, WorkFlowTaskManager workFlowTaskManager
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oaFeeRepository = oaFeeRepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAFeeInputDto input)
        {
            var model = input.MapTo<OAFee>();
            model.Status = 0;
            _oaFeeRepository.InsertOrUpdate(model);
            return new InitWorkFlowOutput() {  InStanceId=model.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAFeeDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaFeeRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAFeeDto>();
            model.UserName = _userRepository.Get(ret.CreatorUserId.Value).Surname;

            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OAFeeListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaFeeRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);


            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAFeeListDto>>();

            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }



            return new PagedResultDto<OAFeeListDto>(total, model);
        }

        public void Update(OAFeeInputDto input)
        {
            var ret = _oaFeeRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaFeeRepository.Update(ret);
        }
    }
}
