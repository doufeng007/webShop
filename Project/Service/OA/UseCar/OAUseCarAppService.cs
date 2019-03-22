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
    public class OAUseCarAppService : ApplicationService, IOAUseCarAppService
    {
        public readonly IRepository<OAUseCar, Guid> _oaUseCarRepository;
        public readonly IRepository<User, long> _userRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;

        public OAUseCarAppService(
            IRepository<OAUseCar, Guid> oaUseCarRepository, IRepository<User, long> userRepository, WorkFlowTaskManager workFlowTaskManager
            )
        {
            _oaUseCarRepository = oaUseCarRepository;
            _userRepository = userRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OAUseCarInputDto input)
        {
            var model = input.MapTo<OAUseCar>();
            model.Status = 0;
            var ret = _oaUseCarRepository.Insert(model);

            return new InitWorkFlowOutput() { InStanceId=ret.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public OAUseCarDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaUseCarRepository.Get(id);
            if (ret == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "数据不存在");
            }
            var model = ret.MapTo<OAUseCarDto>();
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OAUseCarListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = _oaUseCarRepository.GetAll().Where(ite => ite.CreatorUserId == AbpSession.UserId.Value);
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.Title.Contains(input.SearchKey));
            }

            var count = ret.Count();
            var list = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OAUseCarListDto>>();
            foreach (var item in list)
            {
                item.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.Status.Value);
            }
            return new PagedResultDto<OAUseCarListDto>(count, list);
        }

        public void Update(OAUseCarInputDto input)
        {
            var ret = _oaUseCarRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            _oaUseCarRepository.Update(ret);
        }
    }
}
