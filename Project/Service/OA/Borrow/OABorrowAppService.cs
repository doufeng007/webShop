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

namespace Project.Service.OA.Borrow
{
    public class OABorrowAppService : ApplicationService, IOABorrowAppService
    {
        private readonly IRepository<OABorrow, Guid> _oaBorrowRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OABorrowAppService(IRepository<OABorrow, Guid> oaBorrowRepository, IRepository<User, long> userRepository, WorkFlowTaskManager workFlowTaskManager
            ,WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oaBorrowRepository = oaBorrowRepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OABorrowInputDto input)
        {
            var model = input.MapTo<OABorrow>();
            model.Status = 0;
            _oaBorrowRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId = model.Id.ToString() };
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OABorrowDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaBorrowRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OABorrowDto>();
            model.UserName = _userRepository.Get(ret.CreatorUserId.Value).Surname;

            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OABorrowListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaBorrowRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);


            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OABorrowListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OABorrowListDto>(total, model);
        }

        public void Update(OABorrowInputDto input)
        {
            var ret = _oaBorrowRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaBorrowRepository.Update(ret);
        }
    }
}
