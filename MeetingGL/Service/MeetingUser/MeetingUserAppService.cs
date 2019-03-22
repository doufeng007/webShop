using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp;
using Abp.Authorization;
using Abp.Application.Services;

namespace MeetingGL
{
    public class MeetingUserAppService : FRMSCoreAppServiceBase, IMeetingUserAppService
    {
        private readonly IRepository<MeetingUser, Guid> _repository;
        private readonly IRepository<XZGLMeeting, Guid> _xZGLMeetingRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IRepository<WorkFlowTask, Guid> _workFlowTaskRepository;
        private readonly ProjectNoticeManager _noticeManager;
        public MeetingUserAppService(IRepository<MeetingUser, Guid> repository
        , WorkFlowBusinessTaskManager workFlowBusinessTaskManager, IAbpFileRelationAppService abpFileRelationAppService,
            ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IRepository<WorkFlowTask, Guid> workFlowTaskRepository
            , IRepository<XZGLMeeting, Guid> xZGLMeetingRepository,
            ProjectNoticeManager noticeManager
        )
        {
            this._repository = repository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _xZGLMeetingRepository = xZGLMeetingRepository;
            _noticeManager = noticeManager;
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [AbpAuthorize]
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<MeetingUserOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<MeetingUserOutputDto>();
            ret.MeetingUserRoleName = model.MeetingUserRole.ToString();
            ret.ReturnReceiptStatusName = ret.ReturnReceiptStatus.ToString();
            ret.UserName = (await UserManager.GetUserByIdAsync(ret.UserId)).Name;
            var service = AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IXZGLMeetingAppService>();
            ret.MeetingInfo = await service.GetForViewAsync(new EntityDto<Guid>() { Id = model.MeetingId });
            ret.FileList = _abpFileRelationAppService.GetList(new GetAbpFilesInput() { BusinessId = id.ToString(), BusinessType = (int)AbpFileBusinessType.会议请假申请附件 });
            return ret;

        }
        /// <summary>
        /// 添加一个MeetingUser
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateMeetingUserInput input)
        {
            var newmodel = new MeetingUser()
            {
                MeetingId = input.MeetingId,
                MeetingUserRole = input.MeetingUserRole,
                UserId = input.UserId,
                ReturnReceiptStatus = ReturnReceiptStatus.无回执,
            };
            newmodel.Status = 0;
            await _repository.InsertAsync(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }


        /// <summary>
        /// 添加一个MeetingUser
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public InitWorkFlowOutput CreateSelf(CreateMeetingUserInput input)
        {
            var newmodel = new MeetingUser()
            {
                MeetingId = input.MeetingId,
                MeetingUserRole = input.MeetingUserRole,
                UserId = input.UserId,
                ReturnReceiptStatus = ReturnReceiptStatus.无回执,
            };
            newmodel.Status = 0;
            _repository.Insert(newmodel);
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个MeetingUser
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateMeetingUserInput input)
        {
            if (input.InStanceId != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.InStanceId);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                dbmodel.ReturnReceiptStatus = input.ReturnReceiptStatus;
                if (input.ReturnReceiptStatus == ReturnReceiptStatus.确定参会)
                    dbmodel.ConfirmData = DateTime.Now;
                else if (input.ReturnReceiptStatus == ReturnReceiptStatus.申请请假)
                {
                    dbmodel.AskForLeaveRemark = input.AskForLeaveRemark;
                    var fileList = new List<AbpFileListInput>();
                    foreach (var entity in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = entity.Id, Sort = entity.Sort });
                    }
                    await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                    {
                        BusinessId = dbmodel.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.会议请假申请附件,
                        Files = fileList
                    });
                }
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }


        //private MeetingUserLogDto GetChangeModel(MeetingUser model)
        //{
        //    var ret = model.MapTo<MeetingUserLogDto>();
        //    return ret;
        //}
        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        public string GetMeetingCreateUserForReturnReceipt(Guid instanceId)
        {
            var query = from a in _xZGLMeetingRepository.GetAll()
                        join b in _repository.GetAll() on a.Id equals b.MeetingId
                        where b.Id == instanceId
                        select a.MeetingCreateUser;
            var ret = query.FirstOrDefault();
            if (ret != 0)
                return $"u_{ret}";
            else
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "查询会议发起者失败！");


        }


        public string GetMeetingCreateUser(Guid meetingId)
        {
            var model = _xZGLMeetingRepository.Get(meetingId);
            return $"u_{model.MeetingCreateUser}";
        }

        /// <summary>
        /// 会议确认 管理员作废会议回执的处理中的待办
        /// </summary>
        [RemoteService(IsEnabled = false)]
        public void AutoInvalidMeetingUserReturnReceiptTodos(Guid meetingId, bool isSendMessage = false)
        {
            var tasks = from a in _workFlowTaskRepository.GetAll()
                        join b in _repository.GetAll() on a.InstanceID equals b.Id.ToString()
                        where b.MeetingId == meetingId && (a.Status == 0 || a.Status == 1)
                        select a;
            foreach (var item in tasks)
            {
                item.Status = 10;
                _workFlowTaskRepository.Update(item);
            }
            if (isSendMessage)
            {
                var taskUsers = (from a in tasks group a by a.ReceiveID into g select g).ToList();
                if (taskUsers.Count() > 0)
                {
                    foreach (var item in taskUsers)
                    {
                        var task = item.FirstOrDefault();
                        _noticeManager.CreateOrUpdateNotice(new NoticePublishInput()
                        {
                            Title =task.Title+ "终止任务",
                            Content = "终止任务",
                            NoticeUserIds = task.ReceiveID.ToString(),
                            NoticeType = 1
                        });
                    }
                }
            }
        }
    }
}