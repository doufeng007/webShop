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
    public class OAPettyCashAppService : ApplicationService, IOAPettyCashAppService
    {
        private readonly IRepository<OAPettyCash, Guid> _oaPettyCashRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAPettyCashAppService(IRepository<OAPettyCash, Guid> oaPettyCashRepository, IRepository<User, long> userRepository, WorkFlowTaskManager workFlowTaskManage
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oaPettyCashRepository = oaPettyCashRepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManage;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAPettyCashInputDto input)
        {
            var model = input.MapTo<OAPettyCash>();
            model.Status = 0;
            _oaPettyCashRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OAPettyCashDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaPettyCashRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAPettyCashDto>();
            model.UserName = _userRepository.Get(ret.CreatorUserId.Value).Surname;

            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OAPettyCashListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaPettyCashRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);


            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Reason.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAPettyCashListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAPettyCashListDto>(total, model);
        }

        public void Update(OAPettyCashInputDto input)
        {
            var ret = _oaPettyCashRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaPettyCashRepository.Update(ret);
        }
    }
}
