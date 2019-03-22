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

namespace Project.Service.OA.Leave
{
    public class OALeaveAppService : ApplicationService, IOALeaveAppService
    {
        public readonly IRepository<OALeave, Guid> _oaLeaveRepository;
        public readonly IRepository<OASignin, Guid> _oaSigninRepository;
        public readonly IRepository<OAWorkon, Guid> _oaWorkonRepository;
        public readonly IRepository<OAWorkout, Guid> _oaWorkoutRepository;
        public readonly IRepository<OABuessOut, Guid> _oaBuessOutRepository;
        public readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        //private readonly IOrganizeRepository _organizeRepository;
        //private readonly IRoadFlowUserRepository _roadFlowUserRepository;
        public OALeaveAppService(IRepository<OALeave, Guid> oaLeaveRepository, IRepository<OASignin, Guid> oaSigninRepository, IRepository<OAWorkon, Guid> oaWorkonRepository,
            IRepository<OAWorkout, Guid> oaWorkoutRepository, IRepository<OABuessOut, Guid> oaBuessOutRepository, IRepository<User, long> userRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            WorkFlowTaskManager workFlowTaskManager
            )
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaLeaveRepository = oaLeaveRepository;
            _oaSigninRepository = oaSigninRepository;
            _oaWorkonRepository = oaWorkonRepository;
            _oaWorkoutRepository = oaWorkoutRepository;
            _oaBuessOutRepository = oaBuessOutRepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OALeaveInputDto input)
        {
            var model = input.MapTo<OALeave>();
            model.Status = 0;
            var ret = _oaLeaveRepository.Insert(model);  
            return new InitWorkFlowOutput() {  InStanceId=ret.Id.ToString()} ;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public OALeaveDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaLeaveRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OALeaveDto>();
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OALeaveListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaLeaveRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }

            var count = ret.Count();
            var list = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OALeaveListDto>>();
            foreach (var item in list)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OALeaveListDto>(count, list);
        }

        public void Update(OALeaveInputDto input)
        {
            var ret = _oaLeaveRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaLeaveRepository.Update(ret);
        }
    }
}
