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
using Abp.UI;
using Abp.Runtime.Session;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Newtonsoft.Json.Linq;
using ZCYX.FRMSCore.Authorization.Users;
using Abp.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.WorkFlow;
using Abp.Authorization;
using System.Data.SqlClient;
using Abp.Authorization.Users;
using Dapper;
using ZCYX.FRMSCore.Application;
using System.Data;
using ZCYX.FRMSCore.Model;

namespace Project.Service.ProjectTodo
{
    public class ProjectTodoAppService : ApplicationService, IProjectTodoAppService
    {
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workFlowOrganizationUnitsRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IRepository<ConstructionOrganizations> _constructionOrganizationRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<UserFollowProject, Guid> _userFollowProjectRepository;
        private readonly IRepository<User, long> _userrepository;
        private readonly IDynamicRepository _dynamicRepository;

        public ProjectTodoAppService(IRepository<WorkFlowOrganizationUnits, long> workFlowOrganizationUnitsRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository
            , IProjectBaseRepository projectBaseRepository, IRepository<ConstructionOrganizations> constructionOrganizationRepository, IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository
            , IRepository<UserFollowProject, Guid> userFollowProjectRepository, IRepository<User, long> userrepository, IDynamicRepository dynamicRepository)
        {
            _projectBaseRepository = projectBaseRepository;
            _workFlowOrganizationUnitsRepository = workFlowOrganizationUnitsRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _constructionOrganizationRepository = constructionOrganizationRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _userFollowProjectRepository = userFollowProjectRepository;
            _userrepository = userrepository;
            _dynamicRepository = dynamicRepository;
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<ProjectListStatus>> GetProjectListStatusAsync(SearchProjectListStatus input)
        {
            try
            {


                var org = from a in _workFlowOrganizationUnitsRepository.GetAll()
                          join b in _userOrganizationUnitRepository.GetAll() on a.Id equals b.OrganizationUnitId
                          where b.UserId == AbpSession.UserId.Value
                          select b;
                var orgModel = await org.FirstOrDefaultAsync();
                if (orgModel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "用户所属部门获取失败");
                }
                var service = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IWorkFlowOrganizationUnitsAppService>();
                var users = await service.GetUserWithCurrentAndUnderOrg(new UserUnderOrgProssceStaticInput() { OrgId = orgModel.Id });
                var underuserids = new List<long>();
                underuserids = users.Select(r => r.Id).ToList();
                underuserids.Add(AbpSession.UserId.Value);
                var leaderMemberType = (int)AuditRoleEnum.项目负责人;
                var project = from a in _projectBaseRepository.GetAll()
                              join b in _constructionOrganizationRepository.GetAll() on a.SendUnit equals b.Id
                              join cc in _projectAuditMemberRepository.GetAll() on a.Id equals cc.ProjectBaseId into clist
                              from c in clist.DefaultIfEmpty()
                              join d in _userFollowProjectRepository.GetAll() on new { userid = AbpSession.UserId.Value, projectid = a.Id } equals new { userid = d.Userid, projectid = d.Projectid } into follow
                              from tt in follow.DefaultIfEmpty()
                              where (underuserids.Contains(a.CreatorUserId.Value) || (c != null && underuserids.Contains(c.UserId)))
                              select new ProjectListStatus()
                              {
                                  AppraisalTypeId = a.AppraisalTypeId,
                                  AuditAmount = a.AuditAmount,
                                  FinishDays = a.Days,
                                  IsImportant = a.Is_Important ?? false,
                                  IsFollow = tt != null ? true : false,
                                  Id = a.Id,
                                  // ProjectManagerUser
                                  ProjectName = a.ProjectName,
                                  ProjectStatus = a.ProjectStatus,
                                  //ProjectStatusText = a.ProjectStatus.ToString(),
                                  SafaBudget = a.SafaBudget,
                                  SendTotalBudget = a.SendTotalBudget,
                                  SendUnit = a.SendUnit,
                                  SendUnitText = b.Name,
                                  Status = a.Status,
                                  StartTime = a.CreationTime,
                                  //AppraisalType
                                  //FinishTime=a.CreationTime.AddDays(a.Days)
                                  ProjectManagerUser = (from pm in _projectAuditMemberRepository.GetAll()
                                                        join u in _userrepository.GetAll() on pm.UserId equals u.Id
                                                        where pm.ProjectBaseId == a.Id && pm.UserAuditRole == leaderMemberType
                                                        select u.Name).FirstOrDefault() ?? "",


                              };
                switch (input.Status)
                {
                    case 1:
                        project = project.Where(ite => ite.ProjectStatus.HasValue && (int)ite.ProjectStatus.Value > 0);
                        break;
                    case 2:
                        project = project.Where(ite => ite.Status == 1);
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
                //var ret = project.OrderByDescending(ite => ite.CreationTime).OrderByDescending(ite => ite.IsFollow).OrderByDescending(ite => ite.IsImportant).PageBy(input).ToList();
                var data = project.OrderByDescending(ite => ite.StartTime).OrderByDescending(ite => ite.IsFollow).OrderByDescending(ite => ite.IsImportant).Skip(input.SkipCount).Take(input.MaxResultCount);
                var ret = await data.ToListAsync();
                var projectid2 = ret.Select(ite => ite.Id).ToList();

                foreach (var p in ret)
                {
                    p.ProjectManagerUser = "";
                    p.ProjectStatusText = "待办";
                    if (p.ProjectStatus.HasValue)
                    {
                        p.ProjectStatusText = p.ProjectStatus.ToString();
                    }
                    //var a = _projectAuditMemberRepository.GetAll().FirstOrDefault(ite => ite.ProjectBaseId == p.Id && ite.UserAuditRole == (int)AuditRoleEnum.项目负责人);
                    //if (a != null)
                    //{
                    //    var user = _userrepository.Get(a.UserId);
                    //    p.ProjectManagerUser = user.Surname;

                    //}
                    p.AppraisalType = "";
                    p.FinishTime = p.CreationTime.AddDays(p.FinishDays ?? 0);
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
            catch (Exception ex)
            {

                throw;
            }
        }

        [AbpAuthorize]
        public async Task<dynamic> GetProjectTodoCount(GetProjectTodoListInput input)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@userid", AbpSession.UserId.Value);
            parameters.Add("@hasComplete", input.IsComplete);
            var data = await _dynamicRepository.QueryFirstAsync($"exec spGetTodoTasksCount @userid,@hasComplete", parameters);
            return data;
        }
    }
}
