using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.WorkFlow;
using Abp.File;
using Abp.Authorization;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;
using Microsoft.Extensions.Configuration;
using ZCYX.FRMSCore.Configuration;
using Abp.Reflection.Extensions;

namespace Project
{
    public class WorkTaskAppService : ApplicationService, IWorkTaskAppService
    {
        private readonly WorkTaskManager _workTaskManager;
        private readonly IRepository<ProjectWorkTask, Guid> _projectWorkTaskRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        //private readonly IProjectAuditRepository _projectAuditRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<WorkFlow, Guid> _workFlowRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<ProjectRegistration, Guid> _projectRegistrationRepository;
        private readonly IRepository<NoticeDocument, Guid> _noticeDocumentRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IWorkFlowWorkTaskAppService _workFlowWorkTaskAppService;
        public WorkTaskAppService(WorkTaskManager workTaskManager, IRepository<ProjectWorkTask, Guid> projectWorkTaskRepository,
            IRepository<User, long> userRepository, IProjectBaseRepository projectBaseRepository,
            // IProjectAuditRepository projectAuditRepository,
            IWorkFlowTaskRepository workFlowTaskRepository, IRepository<WorkFlow, Guid> workFlowRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<ProjectRegistration, Guid> projectRegistrationRepository, IRepository<NoticeDocument, Guid> noticeDocumentRepository, 
            IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository,IWorkFlowWorkTaskAppService workFlowWorkTaskAppService
            , IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository)
        {
            _workTaskManager = workTaskManager;
            _projectWorkTaskRepository = projectWorkTaskRepository;
            _userRepository = userRepository;
            _projectBaseRepository = projectBaseRepository;
            //_projectAuditRepository = projectAuditRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _workFlowRepository = workFlowRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectRegistrationRepository = projectRegistrationRepository;
            _noticeDocumentRepository = noticeDocumentRepository;
            var coreAssemblyDirectoryPath = typeof(WorkTaskAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _workFlowWorkTaskAppService = workFlowWorkTaskAppService;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }

        [AbpAuthorize]
        public async Task WriteWorkLog(WorkLogInput input)
        {
            long userId = AbpSession.UserId.Value;


            if (input.StepId.HasValue)
            {
                var taskModel = await _workFlowTaskRepository.FirstOrDefaultAsync(r => r.InstanceID == input.ProjectId.Value.ToString() && r.StepID == input.StepId.Value);
                if (taskModel != null)
                {
                    input.StepName = taskModel.StepName;
                }
            }

            var logId = await _workTaskManager.InsertWorkLog(new ProjectWorkTask
            {
                UserId = userId,
                ProjectId = input.ProjectId,
                Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd")),
                StepId = input.StepId,
                StepName = input.StepName
            }, new ProjectWorkLog
            {
                UserId = userId,
                ProjectId = input.ProjectId,
                Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd")),
                Content = input.Content,
                WorkTime = input.WorkTime,
                LogType = input.LogType,
                //Files = Newtonsoft.Json.JsonConvert.SerializeObject(input.Files),
                StepId = input.StepId,
                StepName = input.StepName
            });


            var fileList = new List<AbpFileListInput>();
            foreach (var filemodel in input.Files)
            {
                var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                fileList.Add(fileone);
            }

            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = logId.ToString(),
                BusinessType = (int)AbpFileBusinessType.工作底稿附件,
                Files = fileList
            });

            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = input.ProjectId.Value;
            noticeInput.Content = $"发布工作底稿,详情在项目查询工作室查看";
            noticeInput.UserType = 1;
            var projectName = from p in _singleProjectInfoRepository.GetAll()
                              where p.Id == input.ProjectId
                              select p.SingleProjectName;
            if (projectName.Any())
            {
                noticeInput.Title = $"项目：{projectName.FirstOrDefault()} 发布工作底稿";
                await noticeService.CreateProjectWorkSpaceNotice(noticeInput);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取项目数据异常");
            }


        }

        

        [AbpAuthorize]
        public async Task<PmInitWorkFlowOutput> WriteRegistrationIsPm(WorkRegistrationInput input)
        {
            var list = _appConfiguration["App:WorkContact"].Split(",").Select(x => x.ToGuid()).ToList();
            if (input.StepId.HasValue)
            {
                var isPm = list.Contains(input.StepId.Value);
                long personOnCharge = 0;
                if (isPm)
                {
                    var leaderMemberType = (int)AuditRoleEnum.项目负责人;
                    var pmUserId = (from pm in _projectAuditMemberRepository.GetAll()
                                    join u in _userRepository.GetAll() on pm.UserId equals u.Id
                                    where pm.ProjectBaseId == input.ProjectId.Value && pm.UserAuditRole == leaderMemberType
                                    select u.Id).FirstOrDefault();
                    if (pmUserId != null)
                        personOnCharge = pmUserId;
                }
                if (personOnCharge > 0)
                {
                    long userId = AbpSession.UserId.Value;
                    if (input.StepId.HasValue)
                    {
                        var taskModel = await _workFlowTaskRepository.FirstOrDefaultAsync(r => r.InstanceID == input.ProjectId.Value.ToString() && r.StepID == input.StepId.Value);
                        if (taskModel != null)
                        {
                            input.StepName = taskModel.StepName;
                        }
                    }
                    var regId = await _workTaskManager.InsertRegistration(new ProjectWorkTask
                    {
                        UserId = userId,
                        TaskType = input.Type == 1 ? 3 : 4,
                        ProjectId = input.ProjectId,
                        Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd")),
                        StepId = input.StepId,
                        StepName = input.StepName
                    },
                    new ProjectRegistration
                    {
                        SendUserId = userId,
                        ProjectId = input.ProjectId,
                        Code = input.Code,
                        Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd")),
                        WorkTime = input.WorkTime,
                        Type = input.Type,
                        Content = input.Content,
                        RecieveUserId = 0,
                        StepId = input.StepId,
                        StepName = input.StepName,
                        PersonOnChargeType=PersonOnChargeTypeEnum.待汇总,
                        PersonOnCharge = personOnCharge
                    },input.Files);
                    return new PmInitWorkFlowOutput() { IsPm = true, initWorkFlow = null };
                }
                else
                {
                    var initWorkFlow = await WriteRegistration(input);
                    return new PmInitWorkFlowOutput() { IsPm = false, initWorkFlow = initWorkFlow };
                }
            }
            else
            {
                input.StepName = "负责人汇总";
                var initWorkFlow = await WriteRegistration(input);
                var registrations = _projectRegistrationRepository.GetAll().Where(x => input.RegistrationIds.Contains(x.Id)).ToList();
                registrations.ForEach(x =>
                {
                    x.RegistrationId = initWorkFlow.InStanceId.ToGuid();
                    x.IsSummary = true;
                    x.PersonOnChargeType = PersonOnChargeTypeEnum.已汇总;
                    _projectRegistrationRepository.Update(x);
                });
                return new PmInitWorkFlowOutput() { IsPm = false, initWorkFlow = initWorkFlow };
            }

        }


        [AbpAuthorize]
        public async Task<InitWorkFlowOutput> WriteRegistration(WorkRegistrationInput input)
        {
            long userId = AbpSession.UserId.Value;
            if (input.StepId.HasValue)
            {
                var taskModel = await _workFlowTaskRepository.FirstOrDefaultAsync(r => r.InstanceID == input.ProjectId.Value.ToString() && r.StepID == input.StepId.Value);
                if (taskModel != null)
                {
                    input.StepName = taskModel.StepName;
                }
            }
            var regId = await _workTaskManager.InsertRegistration(new ProjectWorkTask
            {
                UserId = userId,
                TaskType = input.Type == 1 ? 3 : 4,
                ProjectId = input.ProjectId,
                Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd")),
                StepId = input.StepId,
                StepName = input.StepName
            },
            new ProjectRegistration
            {
                SendUserId = userId,
                ProjectId = input.ProjectId,
                Code = input.Code,
                Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd")),
                WorkTime = input.WorkTime,
                Type = input.Type,
                Content = input.Content,
                RecieveUserId = 0,
                StepId = input.StepId,
                PersonOnChargeType = input.IsPersonOnCharge? PersonOnChargeTypeEnum.负责人:PersonOnChargeTypeEnum.普通,
                StepName = input.StepName
            }, input.Files);


            var initRetdata = _workFlowWorkTaskAppService.InitWorkFlowInstance(new InitWorkFlowInput() { FlowId = input.FlowId, FlowTitle = input.FlowTitle, InStanceId = regId.ToString() });
            return new InitWorkFlowOutput() { InStanceId = regId.ToString(), FlowId = input.FlowId, GroupId = initRetdata.GroupId, TaskId = initRetdata.TaskId, StepId = initRetdata.StepId, StepName = initRetdata.StepName };

        }


        [AbpAuthorize]
        public async Task UpdateRegistration(UpdateWorkRegistrationInput input)
        {
            long userId = AbpSession.UserId.Value;
            var model = await _projectRegistrationRepository.GetAsync(input.Id);
            if (input.StepId.HasValue)
            {
                var taskModel = await _workFlowTaskRepository.FirstOrDefaultAsync(r => r.InstanceID == input.ProjectId.Value.ToString() && r.StepID == input.StepId.Value);
                if (taskModel != null)
                {
                    input.StepName = taskModel.StepName;
                }
            }
            model.SendUserId = userId;
            model.ProjectId = input.ProjectId;
            model.Code = input.Code;
            model.Title = string.Format("{0}-{1}", input.Title, input.WorkTime.ToString("yyyyMMdd"));
            model.WorkTime = input.WorkTime;
            model.Type = input.Type;
            model.Content = input.Content;
            model.RecieveUserId = 0;
            model.StepId = input.StepId;
            model.PersonOnChargeType = input.IsPersonOnCharge ? PersonOnChargeTypeEnum.负责人 : PersonOnChargeTypeEnum.普通;
            model.StepName = input.StepName;

        }

        /// <summary>
        /// 工作记录生成后，激活子流程事件
        /// </summary>
        /// <param name="instanceId"></param>
        [RemoteService(false)]
        public string ProjectWriteRegisFlowActive(Guid instanceId)
        {
            var subInstaceId = "";
            var projectRegisModel = _projectRegistrationRepository.Get(instanceId);
            var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = projectRegisModel.ProjectId.Value;
            var inputTypeStr = projectRegisModel.Type == 1 ? "工作联系" : "意见征询";
            noticeInput.Content = $"发布{inputTypeStr},详情在项目查询工作室查看";

            var projectName = from p in _singleProjectInfoRepository.GetAll()
                              where p.Id == projectRegisModel.ProjectId
                              select p.SingleProjectName;
            if (projectName.Any())
            {
                //noticeInput.Title = $"项目：{projectName.FirstOrDefault()} 发布{inputTypeStr}";
                //noticeService.CreateProjectWorkSpaceNotice(noticeInput);



                var model = new NoticeDocument();
                model.Id = Guid.NewGuid();
                model.DispatchTime = DateTime.Now;
                model.Content = projectRegisModel.Content;
                model.IsNeedRes = true;
                model.ProjectId = projectRegisModel.ProjectId;
                model.Title = $"{projectName.FirstOrDefault()}-工作联系";
                model.ProjectRegistrationId = instanceId;
                //model.NoticeType = input.NoticeType;
                //model.DispatchUnit = input.DispatchUnit;
                //model.Status = 0;
                //model.PrintNum = input.PrintNum;
                //model.DispatchCode = input.DispatchCode;
                //model.Urgency = input.Urgency;
                //model.SecretLevel = input.SecretLevel;
                //model.ReceiveId = input.ReceiveId;
                //model.ReceiveName = input.ReceiveName;
                //model.Reason = input.Reason;
                //model.PubilishUserName = input.PubilishUserName;
                //model.MainReceiveName = input.MainReceiveName;
                //model.DocumentTyep = input.DocumentTyep;
                //model.DispatchUnitName = input.DispatchUnitName;
                _noticeDocumentRepository.Insert(model);
                subInstaceId = model.Id.ToString();
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取项目数据异常");
            }
            return subInstaceId;
        }





        public PagedResultDto<ProjectWorkTaskList> GetWorkTaskPage(GetWorkTaskListInput input)
        {
            var all = new List<ProjectWorkTaskList>();
            _workFlowTaskRepository.GetAll().Where(p => p.InstanceID == input.ProjectId.ToString()).ToList().ForEach(p =>
            {
                all.Add(new ProjectWorkTaskList
                {
                    Id = p.Id,
                    TaskType = 0,
                    Title = p.StepName,
                    CreationTime = p.SenderTime,
                    UserName = p.SenderName,
                    ReceiveName = p.ReceiveName,
                    CompletedTime = p.CompletedTime1?.ToString() ?? "",
                    StatusTitle = GetStatusTitle(p.Status),
                    Note = p.Note,

                });
            });
            var projectTask = (from task in _projectWorkTaskRepository.GetAll()
                               join user in _userRepository.GetAll() on task.UserId equals user.Id into worktask
                               from user in worktask.DefaultIfEmpty()
                               where task.ProjectId == input.ProjectId
                               orderby task.CreationTime descending
                               select new ProjectWorkTaskList
                               {
                                   Id = task.Id,
                                   CreationTime = task.CreationTime,
                                   ProjectId = task.ProjectId,
                                   StepId = task.StepId,
                                   StepName = task.StepName,
                                   InstanceId = task.InstanceId,
                                   TaskType = task.TaskType,
                                   Title = task.Title,
                                   UserId = task.UserId,
                                   UserName = user.UserName,
                                   ReceiveName = "-",
                                   CompletedTime = "-",
                                   StatusTitle = "-",
                                   Note = "",
                               }).ToList();

            var query = all.Union(projectTask).ToList();

            if (query == null)
                return new PagedResultDto<ProjectWorkTaskList>(0, new List<ProjectWorkTaskList>());

            var total = query.Count();
            var list = query.OrderByDescending(p => p.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<ProjectWorkTaskList>(total, list);
        }


        private string GetStatusTitle(int status)
        {
            string title = string.Empty;
            switch (status)
            {
                case -1:
                    title = "等待中";
                    break;
                case 0:
                    title = "待处理";
                    break;
                case 1:
                    title = "处理中";
                    break;
                case 2:
                    title = "已完成";
                    break;
                case 3:
                    title = "已退回";
                    break;
                case 4:
                    title = "他人已处理";
                    break;
                case 5:
                    title = "他人已退回";
                    break;
                case 6:
                    title = "终止";
                    break;
                case 7:
                    title = "他人已终止";
                    break;
                case 8:
                    title = "退回审核";
                    break;
                case 9:
                    title = "申请停滞";
                    break;
                default:
                    title = "其它";
                    break;
            }

            return title;
        }

        //public async Task<PagedResultDto<WorkAuditList>> GetWorkAuditPage(GetWorkAuditListInput input)
        //{
        //    var query = (from audit in _projectAuditRepository.GetAll()
        //                 join user in _userRepository.GetAll() on audit.UserId equals user.Id into workaudit
        //                 from user in workaudit.DefaultIfEmpty()
        //                 where audit.TaskId == input.TaskId
        //                 orderby audit.CreationTime descending
        //                 select new WorkAuditList
        //                 {
        //                     Id = audit.Id,
        //                     FieldName = audit.FieldName,
        //                     OldValue = audit.OldValue,
        //                     NewValue = audit.NewValue,
        //                     UserName = user.UserName
        //                 });
        //    if (query == null)
        //        return new PagedResultDto<WorkAuditList>(0, new List<WorkAuditList>());

        //    var total = await query.CountAsync();
        //    var list = await query.Skip(input.SkipCount).Take(input.MaxResultCount).ToListAsync();

        //    return new PagedResultDto<WorkAuditList>(total, list);
        //}


        //public async Task WriteWorkInformationEnter(WorkInformationEnterInput input)
        //{
        //    long userId = AbpSession.UserId.Value;
        //    await _workTaskManager.InsertInformationEnter(new ProjectWorkTask
        //    {
        //        UserId = userId,
        //        ProjectId = input.ProjectId,
        //        Title = input.Title,
        //        StepId = input.StepId,
        //        StepName = input.StepName
        //    }, new ProjectInformationEnter
        //    {
        //        UserId = userId,
        //        ProjectId = input.ProjectId.Value,
        //        Title = input.Title,
        //        Content = input.Content,
        //        InformationEnterType = input.InformationEnterType
        //    });
        //    await CurrentUnitOfWork.SaveChangesAsync();
        //}

        //public Tuple<string, bool> GetStepComment(WorkInformationEnterInput input)
        //{

        //    if (input.ProjectId.HasValue == false || input.StepId.HasValue == false)
        //    {
        //        return Tuple.Create<string, bool>(null, false);
        //    }
        //    WorkFlowTask curstep = _workFlowTaskRepository.GetAll().FirstOrDefault(ite => ite.InstanceID == input.ProjectId.Value.ToString() && ite.StepID == input.StepId.Value);
        //    if (curstep == null || curstep.PrevStepID == Guid.Empty)
        //    {
        //        return Tuple.Create<string, bool>(null, false); ;
        //    }
        //    WorkFlowTask prestep = _workFlowTaskRepository.GetAll().OrderByDescending(ite => ite.CompletedTime1).FirstOrDefault(ite => ite.InstanceID == input.ProjectId.Value.ToString() && ite.StepID == curstep.PrevStepID);
        //    if (prestep == null)
        //    {
        //        return Tuple.Create<string, bool>(null, false); ;
        //    }
        //    if (string.IsNullOrWhiteSpace(input.FlowId) == false)
        //    {
        //        var workflower = _workFlowRepository.Get(Guid.Parse(input.FlowId));
        //        if (workflower != null && string.IsNullOrWhiteSpace(workflower.RunJSON) == false)
        //        {
        //            var helper = new RoadFlow.Platform.WorkFlowHelper();
        //            var msg = "";
        //            var model = helper.GetWorkFlowRunModel(workflower.RunJSON, out msg);
        //            var step = model.Steps.FirstOrDefault(ite => ite.ID == input.StepId);
        //            if (step != null)
        //            {
        //                if (step.OpinionDisplay == 1)
        //                {
        //                    return Tuple.Create<string, bool>(prestep.Comment, true);
        //                }
        //            }
        //        }


        //    }
        //    return Tuple.Create<string, bool>(prestep.Comment, false);
        //}


    }
}