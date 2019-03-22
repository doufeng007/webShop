using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Extensions;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using Abp.WorkFlow;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.File;

namespace Project
{
    public class OAMeetingAppService : FRMSCoreAppServiceBase, IOAMeetingAppService
    {
        private readonly IRepository<OAMeeting, Guid> _oAMeetingRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAMeetingUser, Guid> _oAMeetingUserRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        public OAMeetingAppService(IRepository<OAMeeting, Guid> oAMeetingRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAMeetingUser, Guid> oAMeetingUserRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager)
        {
            _oAMeetingRepository = oAMeetingRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAMeetingUserRepository = oAMeetingUserRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAMeetingDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAMeeting = await _oAMeetingRepository.GetAsync(id);
            var output = oAMeeting.MapTo<OAMeetingDto>();
            var oAusers =
               await _oAMeetingUserRepository.GetAll().Where(r => r.OAMeetingId == oAMeeting.Id).ToListAsync();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA会议附件 });

            var hostUser = await UserManager.GetUserByIdAsync(output.HostUser);
            var noteUser = await UserManager.GetUserByIdAsync(output.NoteUser);
            output.HostUser_Name = hostUser?.Name ?? "";
            output.NoteUser_Name = noteUser?.Name ?? "";
            if (output.DepartmentId.HasValue)
            {
                var depEntity = await _organizeRepository.FirstOrDefaultAsync(output.DepartmentId.Value);
                if (depEntity == null)
                {
                    output.DepartmentId_Name = "";
                }
                else
                {
                    output.DepartmentId_Name = depEntity.DisplayName;
                }
                
            }

            oAusers.ForEach(r =>
            {
                var entity = new CreateOrUpdateOAMeetingUserInput();
                entity.OAMeetingId = r.OAMeetingId;
                entity.UserId = r.UserId;
                output.Users.Add(entity);
            });
            if (string.IsNullOrWhiteSpace(output.ParticipateUser) == false) {
                output.ParticipateUser_Name = _workFlowOrganizationUnitsManager.GetNames(output.ParticipateUser);
            }
            
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAMeetingListDto>> GetAll(GetOAMeetingListInput input)
        {

            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAMeetingRepository.GetAll()
                        join musers in _oAMeetingUserRepository.GetAll() on m.Id equals musers.OAMeetingId into muser
                        where (input.HostByCurrentUser && m.HostUser == currentUserId)
                        || (input.HasParticipate && muser.Select(r => r.UserId).Contains(currentUserId)) || (input.CreateByCurrentUser && m.CreatorUserId == currentUserId)
                        select m;

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Title.Contains(input.SearchKey));

            }
            var count = await query.CountAsync();
            var oAMeetings = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAMeetingDtos = new List<OAMeetingListDto>();
            foreach (var oAMeeting in oAMeetings)
            {
                var entity = new OAMeetingListDto() { Id = oAMeeting.Id, Title = oAMeeting.Title };
                entity.InstanceId = oAMeeting.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, entity as BusinessWorkFlowListOutput);
                oAMeetingDtos.Add(entity);
            }
            return new PagedResultDto<OAMeetingListDto>(count, oAMeetingDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAMeetingCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAMeeting = new OAMeeting();
            input.MapTo(oAMeeting);
            oAMeeting.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAMeeting.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA会议附件,
                    Files = fileList
                });
            }
            var userHostInfo = await UserManager.GetUserByIdAsync(input.HostUser);
            var notuser = await UserManager.GetUserByIdAsync(input.NoteUser);
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = manager.GetAllUsers(input.ParticipateUser);
            foreach (var user in users)
            {
                var entity = new OAMeetingUser();
                entity.Id = Guid.NewGuid();
                entity.OAMeetingId = oAMeeting.Id;
                entity.UserId = user.Id;
                await _oAMeetingUserRepository.InsertAsync(entity);
            }
            oAMeeting.NotifyUsers = oAMeeting.ParticipateUser + ",u_" + userHostInfo + ",u_" + notuser;
            await _oAMeetingRepository.InsertAsync(oAMeeting);
            ret.InStanceId = oAMeeting.Id.ToString();
            return ret;
        }


        public async Task Update(OAMeetingUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAMeeting = await _oAMeetingRepository.GetAsync(input.Id);
            input.MapTo(oAMeeting);
            var fileList = new List<AbpFileListInput>();
            if (input.FileList != null)
            {
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
            }
            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.OA会议附件,
                Files = fileList
            });


            var userHostInfo = await UserManager.GetUserByIdAsync(input.HostUser);
            var notuser = await UserManager.GetUserByIdAsync(input.NoteUser);
            await _oAMeetingUserRepository.DeleteAsync(r => r.OAMeetingId == oAMeeting.Id);
            var manager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var users = manager.GetAllUsers(input.ParticipateUser);
            foreach (var user in users)
            {
                var entity = new OAMeetingUser();
                entity.Id = Guid.NewGuid();
                entity.OAMeetingId = oAMeeting.Id;
                entity.UserId = user.Id;
                await _oAMeetingUserRepository.InsertAsync(entity);
            }
            oAMeeting.NotifyUsers = oAMeeting.ParticipateUser + ",u_" + userHostInfo.Id + ",u_" + notuser.Id;

            await _oAMeetingRepository.UpdateAsync(oAMeeting);

        }
    }
}

