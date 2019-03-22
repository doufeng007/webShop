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
using Project;
using Abp.Domain.Uow;
using Abp.Authorization;
using Abp.Application.Services;

namespace MeetingGL
{
    public class MeetingIssueAppService : FRMSCoreAppServiceBase, IMeetingIssueAppService
    {
        private readonly IRepository<MeetingIssue, Guid> _repository;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workflowOrganizationUnitsRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<MeetingIssueRelation, Guid> _meetingIssueRelationRepository;

        public MeetingIssueAppService(IRepository<MeetingIssue, Guid> repository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager
            , IRepository<WorkFlowOrganizationUnits, long> workflowOrganizationUnitsRepository, IRepository<SingleProjectInfo, Guid> singleProjectRepository
            , IUnitOfWorkManager unitOfWorkManager, IRepository<MeetingIssueRelation, Guid> meetingIssueRelationRepository

        )
        {
            this._repository = repository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _workflowOrganizationUnitsRepository = workflowOrganizationUnitsRepository;
            _singleProjectRepository = singleProjectRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _meetingIssueRelationRepository = meetingIssueRelationRepository;


        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<MeetingIssueListOutputDto>> GetList(GetMeetingIssueListInput input)
        {
            var userRoles = await UserManager.GetRolesAsync(await UserManager.GetUserByIdAsync(AbpSession.UserId.Value));
            var allData = false;
            var orgData = false;
            var projectData = false;
            var currentUsers = $"u_{AbpSession.UserId.Value}";
            if (userRoles.Any(r => r == "ZJL" || r == "FGLD" || r == "XZRY"))
                allData = true;
            else
            {
                if (userRoles.Any(r => string.Compare(r, "DLEADER", true) == 0))
                    orgData = true;
                if (userRoles.Any(r => r == "XMFZR"))
                    projectData = true;
            }


            var statusArry = new List<string>();
            if (!input.Status.IsNullOrEmpty())
                statusArry = input.Status.Split(",").ToList();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                            join b in _workflowOrganizationUnitsRepository.GetAll().Where(x => !x.IsDeleted) on a.OrgId equals b.Id into g
                            from b in g.DefaultIfEmpty()
                            join c in _singleProjectRepository.GetAll() on a.SingleProjectId.Value equals c.Id into f
                            from c in f.DefaultIfEmpty()
                            where !a.IsDeleted && a.Status > 0 &&
                            (allData ||
                            (!allData && ((orgData && a.IssueType == IssueType.部门议题 && b.Leader.GetStrContainsArray(currentUsers))
                            || (projectData && a.ProjectLeaderId.HasValue && a.ProjectLeaderId.Value == AbpSession.UserId.Value))))
                            select new MeetingIssueListOutputDto()
                            {
                                Id = a.Id,
                                Name = a.Name,
                                OrgId = a.OrgId,
                                OrgName = b == null ? "" : b.DisplayName,
                                UserId = a.UserId,
                                Content = a.Content,
                                CreationTime = a.CreationTime,
                                Status = a.Status,
                                IssueType = a.IssueType,
                                SingleProjectId = a.SingleProjectId,
                                SingleProjecetName = c == null ? "" : c.SingleProjectName,


                            };
                query = query.WhereIf(!input.Status.IsNullOrEmpty(), r => statusArry.Contains(((int)r.Status).ToString()));
                if (!string.IsNullOrEmpty(input.SearchKey))
                    query = query.Where(x => x.Name.Contains(input.SearchKey));
                var toalCount = await query.CountAsync();
                var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
                foreach (var item in ret)
                {
                    if (!item.UserId.IsNullOrEmpty())
                        item.UserName = _workFlowOrganizationUnitsManager.GetNames(item.UserId);
                    item.StatusTitle = item.Status.ToString();
                }
                return new PagedResultDto<MeetingIssueListOutputDto>(toalCount, ret);
            }


        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<MeetingIssueOutputDto> Get(NullableIdDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            var ret = new MeetingIssueOutputDto();
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                ret = model.MapTo<MeetingIssueOutputDto>();
                if (model.OrgId.HasValue)
                    ret.OrgName = (await _workflowOrganizationUnitsRepository.GetAsync(model.OrgId.Value)).DisplayName;
                if (!model.UserId.IsNullOrEmpty())
                    ret.UserName = _workFlowOrganizationUnitsManager.GetNames(model.UserId);
                if (model.SingleProjectId.HasValue)
                {
                    var singleProject = await _singleProjectRepository.FirstOrDefaultAsync(r => r.Id == model.SingleProjectId.Value);
                    if (singleProject != null)
                        ret.SingleProjecetName = singleProject.SingleProjectName;
                }

                ret.StautsTitle = ret.Stauts.ToString();
            }

            return ret;
        }
        /// <summary>
        /// 添加一个MeetingIssue
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task<MeetingIssueOutputDto> Create(CreateMeetingIssueInput input)
        {
            if (input.IssueType == IssueType.部门议题 && !input.OrgId.HasValue)
                throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "部门议题请选择部门");

            if (input.IssueType == IssueType.项目议题 && !input.SingleProjectId.HasValue)
                throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "项目议题请选择项目");

            var newmodel = new MeetingIssue()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                OrgId = input.OrgId,
                UserId = input.UserId,
                Content = input.Content,
                IssueType = input.IssueType,
                SingleProjectId = input.SingleProjectId,
                RelationProposalId = input.RelationProposalId,
                Status = MeetingIssueStatus.待议,
            };
            if (input.RelationMeetingId.HasValue)
            {
                newmodel.RelationMeetingId = input.RelationMeetingId;
                newmodel.Status = MeetingIssueStatus.草稿;
            }

            if (input.IssueType == IssueType.项目议题)
            {
                var _projectAuditMemberRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<ProjectAuditMember, Guid>>();
                var projectleaderModel = _projectAuditMemberRepository.GetAll().FirstOrDefault(r => r.ProjectBaseId == input.SingleProjectId.Value && r.UserAuditRole == (int)AuditRoleEnum.项目负责人);
                if (projectleaderModel != null)
                    newmodel.ProjectLeaderId = projectleaderModel.UserId;
            }
            await _repository.InsertAsync(newmodel);
            var ret = new MeetingIssueOutputDto()
            {
                Id = newmodel.Id,
                Name = input.Name,
                OrgId = input.OrgId,
                UserId = input.UserId,
                Content = input.Content
            };
            if (ret.OrgId.HasValue)
                ret.OrgName = (await _workflowOrganizationUnitsRepository.GetAsync(ret.OrgId.Value)).DisplayName;
            if (!ret.UserId.IsNullOrEmpty())
                ret.UserName = _workFlowOrganizationUnitsManager.GetNames(ret.UserId);
            ret.StautsTitle = ret.Stauts.ToString();
            return ret;
        }


        public MeetingIssueOutputDto CreateSelf(CreateMeetingIssueInput input)
        {
            if (input.IssueType == IssueType.部门议题 && !input.OrgId.HasValue)
                throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "部门议题清选择部门");

            if (input.IssueType == IssueType.项目议题 && !input.SingleProjectId.HasValue)
                throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "项目议题清选择项目");

            var newmodel = new MeetingIssue()
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                OrgId = input.OrgId,
                UserId = input.UserId,
                Content = input.Content,
                IssueType = input.IssueType,
                SingleProjectId = input.SingleProjectId,
            };
            if (input.RelationMeetingId.HasValue)
            {
                newmodel.RelationMeetingId = input.RelationMeetingId;
                newmodel.Status = MeetingIssueStatus.草稿;
            }
            else
                newmodel.Status = MeetingIssueStatus.待议;


            if (input.IssueType == IssueType.项目议题)
            {
                var _projectAuditMemberRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<ProjectAuditMember, Guid>>();
                var projectleaderModel = _projectAuditMemberRepository.GetAll().FirstOrDefault(r => r.ProjectBaseId == input.SingleProjectId.Value && r.UserAuditRole == (int)AuditRoleEnum.项目负责人);
                if (projectleaderModel != null)
                    newmodel.ProjectLeaderId = projectleaderModel.UserId;
            }

            _repository.Insert(newmodel);
            var ret = new MeetingIssueOutputDto()
            {
                Id = newmodel.Id,
                Name = input.Name,
                OrgId = input.OrgId,
                UserId = input.UserId,
                Content = input.Content,
                IssueType = input.IssueType,
                SingleProjectId = input.SingleProjectId,
            };
            if (ret.OrgId.HasValue)
                ret.OrgName = (_workflowOrganizationUnitsRepository.Get(ret.OrgId.Value)).DisplayName;
            if (!ret.UserId.IsNullOrEmpty())
                ret.UserName = _workFlowOrganizationUnitsManager.GetNames(ret.UserId);
            ret.StautsTitle = ret.Stauts.ToString();
            return ret;
        }

        /// <summary>
        /// 修改一个MeetingIssue
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateMeetingIssueInput input)
        {
            if (input.Id != Guid.Empty)
            {
                if (input.IssueType == IssueType.部门议题 && !input.OrgId.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "部门议题清选择部门");

                if (input.IssueType == IssueType.项目议题 && !input.SingleProjectId.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "项目议题清选择项目");
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                dbmodel.OrgId = input.OrgId;
                dbmodel.UserId = input.UserId;
                dbmodel.Content = input.Content;
                dbmodel.IssueType = input.IssueType;
                if (dbmodel.IssueType == IssueType.部门议题)
                    dbmodel.SingleProjectId = null;
                else
                    dbmodel.OrgId = null;
                if (input.IssueType == IssueType.项目议题)
                {
                    var _projectAuditMemberRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<ProjectAuditMember, Guid>>();
                    var projectleaderModel = _projectAuditMemberRepository.GetAll().FirstOrDefault(r => r.ProjectBaseId == input.SingleProjectId.Value && r.UserAuditRole == (int)AuditRoleEnum.项目负责人);
                    if (projectleaderModel != null)
                        dbmodel.ProjectLeaderId = projectleaderModel.UserId;
                }

                await _repository.UpdateAsync(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }


        public void UpdateSelf(UpdateMeetingIssueInput input)
        {
            if (input.Id != Guid.Empty)
            {
                if (input.IssueType == IssueType.部门议题 && !input.OrgId.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "部门议题清选择部门");

                if (input.IssueType == IssueType.项目议题 && !input.SingleProjectId.HasValue)
                    throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "项目议题清选择项目");


                var dbmodel = _repository.FirstOrDefault(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                dbmodel.OrgId = input.OrgId;
                dbmodel.UserId = input.UserId;
                dbmodel.Content = input.Content;
                dbmodel.IssueType = input.IssueType;
                if (dbmodel.IssueType == IssueType.部门议题)
                    dbmodel.SingleProjectId = null;
                else
                    dbmodel.OrgId = null;

                if (input.IssueType == IssueType.项目议题)
                {
                    var _projectAuditMemberRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<ProjectAuditMember, Guid>>();
                    var projectleaderModel = _projectAuditMemberRepository.GetAll().FirstOrDefault(r => r.ProjectBaseId == input.SingleProjectId.Value && r.UserAuditRole == (int)AuditRoleEnum.项目负责人);
                    if (projectleaderModel != null)
                        dbmodel.ProjectLeaderId = projectleaderModel.UserId;
                }
                _repository.Update(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }

        [RemoteService(false)]
        public void ChangeIssueProjectLeader(Guid singleProjectId, long userId)
        {
            var issueModels = _repository.GetAll().Where(r => r.SingleProjectId == singleProjectId && !r.ProjectLeaderId.HasValue);
            foreach (var item in issueModels)
            {
                item.ProjectLeaderId = userId;
                _repository.Update(item);
            }
        }


        /// <summary>
        /// 更新会议记录新增的议题状态
        /// </summary>
        /// <param name="meetingId"></param>
        [RemoteService(false)]
        public void UpdateNesIssueSatatus(Guid meetingId)
        {
            var query = from a in _repository.GetAll()
                        where a.RelationMeetingId == meetingId
                        select a;
            foreach (var item in query)
            {
                if (item.Status == MeetingIssueStatus.草稿)
                {
                    item.Status = MeetingIssueStatus.待议;
                    _repository.Update(item);
                }
            }

            var relationIssues = from a in _repository.GetAll()
                                 join b in _meetingIssueRelationRepository.GetAll() on a.Id equals b.IssueId
                                 where b.MeetingId == meetingId
                                 select new { a, b };
            foreach (var item in relationIssues)
            {
                if (item.a.Status == MeetingIssueStatus.准备中)
                {
                    item.a.Status = item.b.Status == MeetingIssueResultStatus.HasPass ? MeetingIssueStatus.已议 : MeetingIssueStatus.延迟;
                    _repository.Update(item.a);
                }
            }

        }
    }
}