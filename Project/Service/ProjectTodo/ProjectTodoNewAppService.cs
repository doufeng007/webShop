using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Authorization.Users;
using System.Linq;
using ZCYX.FRMSCore.Application;
using Abp.Authorization;
using Abp.Linq.Extensions;

namespace Project
{
    public class ProjectIdComparer : IEqualityComparer<ProjectListStatus>
    {
        public bool Equals(ProjectListStatus x, ProjectListStatus y)
        {
            if (x == null)
                return y == null;
            return x.Id == y.Id;
        }

        public int GetHashCode(ProjectListStatus obj)
        {
            if (obj == null)
                return 0;
            return obj.Id.GetHashCode();
        }
    }
    public class ProjectBaseIdComparer : IEqualityComparer<ProjectBase>
    {
        public bool Equals(ProjectBase x, ProjectBase y)
        {
            if (x == null)
                return y == null;
            return x.Id == y.Id;
        }

        public int GetHashCode(ProjectBase obj)
        {
            if (obj == null)
                return 0;
            return obj.Id.GetHashCode();
        }
    }
    public class ProjectTodoNewAppService : ApplicationService, IProjectTodoNewAppService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<WorkFlow, Guid> _workFlowRepository;
        private readonly IRepository<ProjectAuditGroupUser, Guid> _projectAuditGroupUserRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<ConstructionOrganizations> _constructionOrganizationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<OATask, Guid> _oataskRepository;
        private readonly IRepository<ProjectRealationUser, Guid> _projectRealationUserRepository;
        //private readonly IRoadFlowUserRepository _roadFlowUserRepository;
        private readonly WorkFlowOrganizationUnitsManager _organizeRepository;
        private readonly IRepository<UserFollowProject, Guid> _userFollowProjectRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        public ProjectTodoNewAppService(IProjectBaseRepository projectBaseRepository,
            IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository,
            IWorkFlowTaskRepository workFlowTaskRepository, IRepository<WorkFlow, Guid> workFlowRepository,
            IRepository<ConstructionOrganizations> constructionOrganizationRepository,
            IRepository<User, long> userRepository,
            IRepository<OATask, Guid> oataskRepository,
            //IRoadFlowUserRepository roadFlowUserRepository,
            WorkFlowOrganizationUnitsManager organizeRepository,
            IRepository<UserFollowProject, Guid> userFollowProjectRepository,
            IRepository<ProjectRealationUser, Guid> projectRealationUserRepository,
            IRepository<ProjectAuditGroupUser, Guid> projectAuditGroupUserRepository, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository)
        {
            _projectRealationUserRepository = projectRealationUserRepository;
            _projectBaseRepository = projectBaseRepository;
            _workFlowRepository = workFlowRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _projectAuditGroupUserRepository = projectAuditGroupUserRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _constructionOrganizationRepository = constructionOrganizationRepository;
            _userRepository = userRepository;
            _oataskRepository = oataskRepository;
            //_roadFlowUserRepository = roadFlowUserRepository;
            _organizeRepository = organizeRepository;
            _userFollowProjectRepository = userFollowProjectRepository;
            _singleProjectInfoRepository = singleProjectInfoRepository;
        }
        [AbpAuthorize]
        public PagedResultDto<OATodoList> GetOATodoList(SearchProjectListStatus input)
        {
            //获取当前用户orgid
            var org = _organizeRepository.GetDeptByUserID(AbpSession.UserId.Value);
            var underuserids = new List<long>();
            if (org != null)
            {
                int cout = 0;
                underuserids = _organizeRepository.GetAllUsersById(org.Id).Select(ite => ite.Id).ToList();
            }
            //var userguids = _roadFlowUserRepository.GetAll().Where(ite => underuserids.Contains(ite.AbpUserId)).Select(ite => ite.Id).ToList();
            underuserids.Add(AbpSession.UserId.Value);
            var oatask = _oataskRepository.GetAll().Where(ite => underuserids.Contains(ite.CreatorUserId.Value));

            switch (input.Status)
            {
                case 0:
                    oatask = oatask.Where(ite => ite.Status == 0);
                    break;
                case 1:
                    oatask = oatask.Where(ite => ite.Status == 2);
                    break;
                case 2:
                    oatask = oatask.Where(ite => ite.Status == 1);
                    break;
            }
            var count = oatask.Count();
            var ret = oatask.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList();
            var model = new List<OATodoList>();
            foreach (var i in ret)
            {
                var x = new OATodoList();
                x.Doing = true;
                var memberIdArry = i.ExecutorUser.Split(",");
                //foreach (var member in memberIdArry)
                //{
                //    var memberName = "";
                //    if (member.IsGuid())
                //    {
                //        var memberId = member.ToGuid();
                //        var memberModel = _organizeRepository.FirstOrDefault(r => r.Id == memberId);
                //        if (memberModel != null)
                //        {
                //            memberName = "[部门]" + memberModel.Name;
                //        }
                //    }
                //    else if (member.StartsWith(MemberPerfix.UserPREFIX))
                //    {
                //        var memberId = MemberPerfix.RemovePrefix(member).ToGuid();
                //        var usermodel =
                //             _roadFlowUserRepository.FirstOrDefault(
                //                r => r.Id == memberId);
                //        if (usermodel != null)
                //        {
                //            memberName = usermodel.Name;
                //        }
                //    }
                //    else
                //    {
                //    }
                //    if (!memberName.IsNullOrWhiteSpace())
                //    {
                //        if (!x.DoUser.IsNullOrWhiteSpace())
                //            x.DoUser = x.DoUser + ",";
                //        x.DoUser = x.DoUser + memberName;
                //    }


                //}
                x.Level = "C";
                switch (i.PriorityCode)
                {
                    case "1":
                        x.Level = "A";
                        break;
                    case "2":
                        x.Level = "B";
                        break;
                    case "3":
                        x.Level = "C";
                        break;
                }
                x.ReciveTime = i.CreationTime;
                x.Title = i.Title;
                var valuser = _userRepository.GetAll().FirstOrDefault(ite => ite.Id == i.ValUser);
                if (valuser != null)
                {
                    x.WatchUser = valuser.Surname;
                }
                x.Des = i.Summary;
                model.Add(x);
            }
            return new PagedResultDto<OATodoList>(count, model);
        }
        [AbpAuthorize]
        public PagedResultDto<ProjectListStatus> GetProjectListStatus(SearchProjectListStatus input)
        {
            //获取当前用户orgid
            //获取当前用户orgid
            var org = _organizeRepository.GetDeptByUserID(AbpSession.UserId.Value);
            var underuserids = new List<long>();
            //判断是否部门主管，如果是这显示下级人员所有项目
            var isleader = org.ChargeLeader.Contains("u_" + AbpSession.UserId.Value);
            if (isleader)
            {

                if (org != null)
                {
                    int cout = 0;
                    underuserids = _organizeRepository.GetAllUsersById(org.Id).Select(ite => ite.Id).ToList();
                }
            }
            underuserids.Add(AbpSession.UserId.Value);

            var projectids = new List<Guid>();
            //查询分派人表中的项目id
            var p1 = _projectAuditMemberRepository.GetAll().Where(ite => underuserids.Contains(ite.UserId)).Select(ite => ite.ProjectBaseId).Distinct().ToList();
            projectids.AddRange(p1);
            //查询项目干系人表中的项目id
            var p2 = _projectRealationUserRepository.GetAll().Where(ite => underuserids.Contains(ite.UserID)).Select(ite => ite.InstanceID).ToList();
            projectids.AddRange(p2);

            var project = //from a in _projectBaseRepository.GetAll()
                           from single in _singleProjectInfoRepository.GetAll()
                           join a in _projectBaseRepository.GetAll() on single.ProjectId equals a.Id
                          join b in _constructionOrganizationRepository.GetAll() on a.SendUnit equals b.Id
                          //join c in _projectAuditMemberRepository.GetAll() on a.Id equals c.ProjectBaseId
                          join d in _userFollowProjectRepository.GetAll() on new { userid = AbpSession.UserId.Value, projectid = a.Id } equals new { userid = d.Userid, projectid = d.Projectid } into follow
                          from tt in follow.DefaultIfEmpty()
                          where underuserids.Contains(single.CreatorUserId.Value) || projectids.Contains(a.Id)
                          select new ProjectListStatus()
                          {
                              AppraisalTypeId = a.AppraisalTypeId,
                              AuditAmount = single.AuditAmount,
                              FinishDays = a.Days,
                              IsImportant = a.Is_Important ?? false,
                              IsFollow = tt != null ? true : false,
                              Id = a.Id,
                              // ProjectManagerUser
                              ProjectName = a.ProjectName,
                              ProjectStatus = single.ProjectStatus,
                              //ProjectStatusText = a.ProjectStatus.ToString(),
                              SafaBudget = single.SingleProjectSafaBudget,
                              SendTotalBudget = single.SingleProjectbudget,
                              SendUnit = a.SendUnit,
                              SendUnitText = b.Name,
                              Status = single.Status,
                              CreationTime = a.CreationTime,
                              // StartTime = a.CreationTime,
                              //AppraisalType
                              //FinishTime=a.CreationTime.AddDays(a.Days)
                              ReadyEndTime = a.ReadyEndTime,
                              ReadyStartTime = a.ReadyStartTime,
                          };
            switch (input.Status)
            {
                case 1:
                    project = project.Where(ite => ite.ProjectStatus.HasValue && (int)ite.ProjectStatus.Value > 0 && ite.Status != -1);
                    break;
                case 2:
                    project = project.Where(ite => ite.Status == -1);
                    break;
                default:
                    project = project.Where(ite => ite.ProjectStatus.HasValue == false || ite.ProjectStatus.Value == ProjectStatus.待审);
                    break;
            }
            if (input.ProjectStatus.HasValue)
            {
                project = project.Where(ite => ite.ProjectStatus == input.ProjectStatus.Value);
            }
            project = project.Distinct();

            var count = project.Count();
            var ret = project.OrderByDescending(ite => ite.CreationTime).OrderByDescending(ite => ite.IsFollow).OrderByDescending(ite => ite.IsImportant).PageBy(input).ToList();
            var projectid2 = ret.Select(ite => ite.Id).ToList();

            foreach (var p in ret)
            {
                p.ProjectManagerUser = "";
                p.ProjectStatusText = "待办";
                if (p.ProjectStatus.HasValue)
                {
                    p.ProjectStatusText = p.ProjectStatus.ToString();
                }
                var a = _projectAuditMemberRepository.GetAll().FirstOrDefault(ite => ite.ProjectBaseId == p.Id && ite.GroupId.HasValue);
                if (a != null && a.GroupId.HasValue)
                {
                    var group = _projectAuditGroupUserRepository.GetAll().FirstOrDefault(ite => ite.GroupId == a.GroupId.Value && ite.UserRole == 1);
                    if (group != null)
                    {
                        var user = _userRepository.FirstOrDefault(group.UserId);
                        if (user != null)
                        {
                            p.ProjectManagerUser = user.Surname;
                        }
                    }
                }
                p.AppraisalType = "";
                p.FinishTime = p.CreationTime.AddDays(p.FinishDays ?? 0);
                p.StartTime = p.CreationTime;
                switch (p.AppraisalTypeId)
                {
                    case 10:
                        p.AppraisalType = "项目概算";
                        break;
                    case 8:
                        p.AppraisalType = "项目预算";
                        break;
                    case 9:
                        p.AppraisalType = "政府采购预算";
                        break;

                    case 18:
                        p.AppraisalType = "财政监管";
                        break;
                    case 19:
                        p.AppraisalType = "日常咨询";
                        break;

                    case 12:
                        p.AppraisalType = "项目调整预算";
                        break;
                    case 1:
                        p.AppraisalType = "项目概算";
                        break;
                    case 13:
                        p.AppraisalType = "支付审核";
                        break;
                    case 11:
                        p.AppraisalType = "专项核查";
                        break;
                    case 15:
                        p.AppraisalType = "项目结算";
                        break;
                    case 16:
                        p.AppraisalType = "项目决算";
                        break;
                    case 17:
                        p.AppraisalType = "绩效评价财务类";
                        break;
                    case 20:
                        p.AppraisalType = "绩效评价工程类";
                        break;
                }
            }

            return new PagedResultDto<ProjectListStatus>(count, ret);
        }
        [AbpAuthorize]
        public ProjectTodoNewDto GetProjectStatic()
        {
            //获取当前用户orgid

            var org = _organizeRepository.GetDeptByUserID(AbpSession.UserId.Value);
            var underuserids = new List<long>();
            //判断是否部门主管，如果是这显示下级人员所有项目
            var isleader = org != null && string.IsNullOrWhiteSpace(org.ChargeLeader) == false && org.ChargeLeader.Contains("u_" + AbpSession.UserId.Value);
            if (isleader)
            {

                if (org != null)
                {
                    int cout = 0;
                    underuserids = _organizeRepository.GetAllUsersById(org.Id).Select(ite => ite.Id).ToList();
                }
            }
            //var userguids = _roadFlowUserRepository.GetAll().Where(ite => underuserids.Contains(ite.AbpUserId)).Select(ite => ite.Id).ToList();
            underuserids.Add(AbpSession.UserId.Value);

            var model = new ProjectTodoNewDto();
            var projectids = new List<Guid>();
            //查询分派人表中的项目id
            var p1 = _projectAuditMemberRepository.GetAll().Where(ite1 => underuserids.Contains(ite1.UserId)).Select(ite => ite.ProjectBaseId).Distinct().ToList();
            projectids.AddRange(p1);
            //查询项目干系人表中的项目id
            var p2 = _projectRealationUserRepository.GetAll().Where(ite2 => underuserids.Contains(ite2.UserID)).Select(ite => ite.InstanceID).ToList();
            projectids.AddRange(p2);
            var project = from a in _singleProjectInfoRepository.GetAll()
                          join b in _projectBaseRepository.GetAll() on a.ProjectId equals b.Id
                              //join b in _constructionOrganizationRepository.GetAll() on a.SendUnit equals b.Id
                          where underuserids.Contains(a.CreatorUserId.Value)
                          || projectids.Contains(b.Id)
                          select a;
            project = project.Distinct();

            model.DoingCount = project.Where(ite3 => ite3.ProjectStatus.HasValue && (int)ite3.ProjectStatus.Value > 0 && ite3.Status != -1).Count();
            model.DoingSum = project.Where(ite4 => ite4.ProjectStatus.HasValue && (int)ite4.ProjectStatus.Value > 0 && ite4.Status != -1).Sum(ite => ite.SingleProjectbudget);
            model.DoingSum = Math.Round(model.DoingSum / 10000, 0);
            model.DoneCount = project.Where(ite5 => ite5.Status == -1).Count();
            model.DoneSum = project.Where(ite6 => ite6.Status == -1).Sum(ite => ite.AuditAmount) ?? 0;
            model.DoneSum = Math.Round(model.DoneSum / 10000, 0);
            model.TodoCount = project.Where(ite7 => ite7.ProjectStatus.HasValue == false || ite7.ProjectStatus.Value == ProjectStatus.待审).Count();
            var oatask = _oataskRepository.GetAll().Where(ite8 => underuserids.Contains(ite8.CreatorUserId.Value));
            model.DoingCount += oatask.Where(ite9 => ite9.Status == 2).Count();
            model.DoneCount += oatask.Where(ite10 => ite10.Status == -1).Count();
            model.TodoCount += oatask.Where(ite11 => ite11.Status == 0).Count();
            var count = Convert.ToDecimal(model.TodoCount + model.DoingCount + model.DoneCount);
            model.HandleRate =count!=0?
                Math.Round(
                    Convert.ToDecimal(model.DoneCount) /
                    count, 2):0;
            return model;
        }
        [AbpAuthorize]
        public ProjectTodoNewDto GetProjectStaticForMy()
        {

            //var orgid = _organizeRepository.GetAll().FirstOrDefault(ite => ite.Leader.Contains(userguid)).Id;
            //int cout = 0;
            //var underuserids = _projectBaseRepository.GetUsersWithCurrentAndUnderOrg(orgid, 100, 1, out cout).Select(ite => ite.Id).ToList();
            //underuserids.Add(AbpSession.UserId.Value);
            //var userguids = _roadFlowUserRepository.GetAll().Where(ite => underuserids.Contains(ite.AbpUserId)).Select(ite => ite.Id).ToList();

            var model = new ProjectTodoNewDto();

            var tasks = _workFlowTaskRepository.GetAll().Where(ite => ite.ReceiveID == AbpSession.UserId.Value && ite.TodoType.Value > 0 && ite.Type != 6);

            model.DoingCount = tasks.Where(ite => ite.Status == 1).Count();
            model.DoneCount = tasks.Where(ite => ite.Status >= 2).Count();
            model.TodoCount = tasks.Where(ite => ite.Status == 0).Count();
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<ProjectTodoNewListDto> GetWorkTaskList(SearchTodoInput input)
        {
            try
            {
                var tasks = from a in _workFlowTaskRepository.GetAll()
                            join b in _workFlowRepository.GetAll() on a.FlowID equals b.Id
                            where a.ReceiveID == AbpSession.UserId.Value && a.Status >= 0
                            select new ProjectTodoNewListDto()
                            {
                                FlowID = a.FlowID,
                                FlowName = b.Name,
                                GroupID = a.GroupID,
                                Id = a.Id,
                                InstanceID = a.InstanceID,
                                ReceiveID = a.ReceiveID,
                                ReceiveName = a.ReceiveName,
                                ReceiveTime = a.ReceiveTime,
                                SenderID = a.SenderID,
                                SenderName = a.SenderName,
                                SenderTime = a.SenderTime,
                                Status = a.Status,
                                StepID = a.StepID,
                                StepName = a.StepName,
                                Title = a.Title,
                                TodoType = a.TodoType,
                                Type = a.Type,
                                AppraisalTypeId = 0,
                            };
                tasks = tasks.Where(ite => ite.Type != 6);
                if (input.TodoType == 0)
                {
                    //流程待办
                    tasks = tasks.Where(ite => (ite.TodoType.HasValue == false || ite.TodoType.Value == 0) && ite.Status < 2);
                }
                else
                {
                    //我的待办
                    tasks = tasks.Where(ite => ite.TodoType.HasValue && ite.TodoType.Value > 0);
                    if (input.Status == 0)
                    {
                        tasks = tasks.Where(ite => ite.Status == 0);
                    }
                    else if (input.Status == 1)
                    {
                        tasks = tasks.Where(ite => ite.Status == 1);
                    }
                    else
                    {
                        tasks = tasks.Where(ite => ite.Status >= 2);
                    }
                }
                var projectFlowIds = _workFlowRepository.GetAll().Where(ite => ite.Type == Guid.Parse("CD897AD9-BC85-4D1E-85D7-08D5570CD07C")).Select(ite => ite.Id).ToList();
                var otherFlowIds = _workFlowRepository.GetAll().Where(ite => ite.Type != Guid.Parse("CD897AD9-BC85-4D1E-85D7-08D5570CD07C")).Select(ite => ite.Id).ToList();

                if (input.FlowType.HasValue)
                {
                    if (input.FlowType == 1)
                    {
                        //项目类的工作流

                        tasks = tasks.Where(ite => projectFlowIds.Contains(ite.FlowID));
                    }
                    else
                    {
                        //非项目类工作流
                        tasks = tasks.Where(ite => otherFlowIds.Contains(ite.FlowID));
                    }
                }


                var count = tasks.Count();
                var ret = tasks.OrderByDescending(ite => ite.SenderTime).PageBy(input).ToList();
                var projectids = Array.ConvertAll<string, Guid>(ret.Where(ite => projectFlowIds.Contains(ite.FlowID)).Select(ite => ite.InstanceID).ToArray(), new Converter<string, Guid>(ite => Guid.Parse(ite)));
                var projects = _projectBaseRepository.GetAll().Where(ite => projectids.Contains(ite.Id)).ToList();
                foreach (var t in ret)
                {
                    if (string.IsNullOrWhiteSpace(t.InstanceID))
                    {
                        continue;
                    }
                    t.StatusTitle = GetStatusTitle(t.Status);
                    if (projectFlowIds.Contains(t.FlowID))
                    {
                        var project = projects.FirstOrDefault(ite => ite.Id.ToString() == t.InstanceID);
                        if (project != null)
                        {
                            t.AppraisalTypeId = project.AppraisalTypeId;
                            t.IsImportent = project.Is_Important ?? false;
                            var follow = _userFollowProjectRepository.GetAll().FirstOrDefault(ite => ite.Userid == AbpSession.UserId.Value && ite.Projectid == project.Id);
                            if (follow != null)
                            {
                                t.IsFollow = true;
                            }
                        }
                    }
                }
                return new PagedResultDto<ProjectTodoNewListDto>(count, ret.ToList());
            }
            catch (Exception ex)
            {
                return null;
                //这里会报一个奇怪的异常 variable 'context' of type 'Abp.EntityFrameworkCore.AbpDbContext' referenced from scope '', but it is not defined    暂时未处理
                //throw new Exception("haha");
            }
        }
        /// <summary>
        /// 得到状态显示标题
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
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
    }
}
