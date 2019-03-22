using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.RealTime;
using Abp.Threading;
using Abp.AutoMapper;
using Abp.Application.Services;
using ZCYX.FRMSCore.Authorization.Users;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class DispatchMessageAppService : ApplicationService, IDispatchMessageAppService
    {
        private readonly WorkTaskManager _workTaskManager;
        private readonly IRepository<DispatchMessage, Guid> _dispatchMessageRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<ConstructionOrganizations> _constructionOrganizationRepository;
        private readonly IRepository<ProjectAuditMemberResult, Guid> _projectAuditMemberResultRepository;
        private readonly IRepository<NoticeDocument, Guid> _noticeDocumentRepository;

        public DispatchMessageAppService(WorkTaskManager workTaskManager, IRepository<DispatchMessage, Guid> dispatchMessageRepository, IProjectBaseRepository projectBaseRepository,
            IRepository<SingleProjectInfo, Guid> singleProjectRepository,
             IRepository<User, long> userRepository, IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ConstructionOrganizations> constructionOrganizationRepository
            , IRepository<ProjectAuditMemberResult, Guid> projectAuditMemberResultRepository, IRepository<NoticeDocument, Guid> noticeDocumentRepository)
        {
            _workTaskManager = workTaskManager;
            _dispatchMessageRepository = dispatchMessageRepository;
            _projectBaseRepository = projectBaseRepository;
            _userRepository = userRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _constructionOrganizationRepository = constructionOrganizationRepository;
            _projectAuditMemberResultRepository = projectAuditMemberResultRepository;
            _noticeDocumentRepository = noticeDocumentRepository;
            _singleProjectRepository = singleProjectRepository;
        }


        public async Task CreateOrUpdate(DispatchPublishInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateDispatch(input);
            }
            else
            {
                await CreateDispatch(input);
            }
        }


        [RemoteService(IsEnabled = false)]
        public Guid CreateActive(string instanceId,long userId)
        {
            var ret = new DispatchPublishInput();
            var projectId = instanceId.ToGuid();
            var singleprojectmodel = _singleProjectRepository.Get(projectId);
            var projectmodel = _projectBaseRepository.FirstOrDefault(ite=>ite.Id==singleprojectmodel.ProjectId);
            if (projectmodel == null) {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr,"项目信息不存在。");
            }
            if (projectmodel.SendUnit == 0)
            {
                ret.SendUnitName = string.Empty;
            }
            else
            {
                var sendunitemodel = _constructionOrganizationRepository.Get(projectmodel.SendUnit);
                ret.SendUnitName = sendunitemodel == null ? "" : sendunitemodel.Name;
            }

            ret.ProjectName = projectmodel.ProjectName;
            ret.ProjectCode = projectmodel.ProjectCode;
            ret.AppraisalTypeId = projectmodel.AppraisalTypeId;
            ret.ProjectId = projectmodel.Id;
            var leaderModel = from a in _projectAuditMemberRepository.GetAll()
                              join u in _userRepository.GetAll() on a.UserId equals u.Id
                              where a.ProjectBaseId == projectId && a.UserAuditRole == 1
                              select u.Name;
            ret.ProjectLeader = leaderModel.FirstOrDefault() ?? "";
            var query = from memeber in _projectAuditMemberRepository.GetAll()
                        join user in _userRepository.GetAll() on memeber.UserId equals user.Id
                        where memeber.UserAuditRole == 2 && memeber.ProjectBaseId == projectmodel.Id
                        select user.Name;
            ret.ProjectReviewer = "";
            var membsers = query.ToList();
            foreach (var name in membsers)
            {
                if (!string.IsNullOrWhiteSpace(ret.ProjectReviewer))
                    ret.ProjectReviewer = ret.ProjectReviewer + ",";
                ret.ProjectReviewer = ret.ProjectReviewer + name; ;
            }
            ret.StartDate = projectmodel.CreationTime;

            var auditRole = (int)AuditRoleEnum.复核人三;
            var threadAuditModel = from a in _projectAuditMemberRepository.GetAll()
                                   join b in _projectAuditMemberResultRepository.GetAll() on a.Id equals b.Pid
                                   where a.ProjectBaseId == projectmodel.Id && a.UserAuditRole == auditRole
                                   select b;
            ret.AuditAmount = threadAuditModel.FirstOrDefault()?.AuditAmount ?? null;


            var newId = Guid.NewGuid();
            var entity = new NoticeDocument()
            {
                Id = newId,
                Title = $"{projectmodel.ProjectName}-{singleprojectmodel.SingleProjectName}-发文",
                Additional = "",
                DispatchCode = "",
                ProjectId = projectId,
                ProjectLeader = ret.ProjectLeader,
                ProjectReviewer = ret.ProjectReviewer,
                ProjectUndertakeCode = ret.ProjectUndertakeCode,
                Reason = "",
                CreatorUserId = userId,
                SendUnitName = ret.SendUnitName,
                DispatchTime = DateTime.Now,
                NoticeDocumentBusinessType = (int)NoticeDocumentBusinessType.项目评审发文,

            };
            if (ret.EndDate.HasValue)
                entity.EndDate = ret.EndDate.Value;
            if (ret.StartDate.HasValue)
                entity.StartDate = ret.StartDate.Value;
            _noticeDocumentRepository.Insert(entity);
            return newId;
        }

        protected virtual async Task UpdateDispatch(DispatchPublishInput input)
        {
            var user_id = AbpSession.UserId;
            var ret = new DispatchPublishOutput();
            var projectmodel = await _projectBaseRepository.GetAsync(input.ProjectId);
            var model = await _dispatchMessageRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectId == input.ProjectId && r.UserId == user_id);
            if (model != null)
            {
                model.Additional = input.Additional;
                model.DispatchCode = input.DispatchCode;
                model.EndDate = input.EndDate;
                model.ProjectUndertakeCode = input.ProjectUndertakeCode;
                model.Reason = input.Reason;
                model.Remark = input.Remark;
                await _dispatchMessageRepository.UpdateAsync(model);
            }
        }

        protected virtual async Task CreateDispatch(DispatchPublishInput input)
        {
            var userId = AbpSession.GetUserId();

            var projectModel = await _projectBaseRepository.GetAll().FirstOrDefaultAsync(r => r.Id == input.ProjectId);
            if (projectModel != null)
            {
                projectModel.AuditAmount = input.AuditAmount;
                await _projectBaseRepository.UpdateAsync(projectModel);
            }


            await _workTaskManager.InsertDispatchAsync(new ProjectWorkTask
            {
                UserId = userId,
                ProjectId = input.ProjectId,
                StepId = input.StepId,
                StepName = input.StepName,
                Title = "批文签发"
            }, new DispatchMessage
            {
                UserId = userId,
                ProjectId = input.ProjectId,
                Additional = input.Additional,
                DispatchCode = input.DispatchCode,
                EndDate = input.EndDate,
                ProjectLeader = input.ProjectLeader,
                ProjectName = input.ProjectName,
                ProjectReviewer = input.ProjectReviewer,
                ProjectUndertakeCode = input.ProjectUndertakeCode,
                Reason = input.Reason,
                Remark = input.Remark,
                StartDate = input.StartDate,
                SendUnitName = input.SendUnitName
            });

        }

        public async Task<DispatchPublishOutput> GetDispatchForEidt(Guid projectId)
        {
            var user_id = AbpSession.UserId;
            var ret = new DispatchPublishOutput();
            var projectmodel = await _projectBaseRepository.GetAsync(projectId);

            var model = await _dispatchMessageRepository.GetAll().FirstOrDefaultAsync(r => r.ProjectId == projectId);
            if (model != null)
            {
                ret = model.MapTo<DispatchPublishOutput>();
                ret.BaseOutput = projectmodel.MapTo<CreateOrUpdateProjectBaseInput>();
                ret.ProjectCode = projectmodel.ProjectCode;
                ret.AppraisalTypeId = projectmodel.AppraisalTypeId;
                ret.AuditAmount = projectmodel.AuditAmount;
                ret.ProjectName = projectmodel.ProjectName;
                ret.SingleProjectName = projectmodel.SingleProjectName;
                ret.SingleProjectCode = projectmodel.SingleProjectCode;
                ret.ProjectId = projectmodel.Id;
                ret.StartDate = projectmodel.CreationTime;
                ret.ProjectLeader = model.ProjectLeader;
                ret.ProjectReviewer = model.ProjectReviewer;
                ret.SendUnitName = model.SendUnitName;

            }
            else
            {
                ret.BaseOutput = projectmodel.MapTo<CreateOrUpdateProjectBaseInput>();
                if (projectmodel.SendUnit == 0)
                {
                    ret.SendUnitName = string.Empty;
                }
                else
                {
                    var sendunitemodel = await _constructionOrganizationRepository.GetAsync(projectmodel.SendUnit);
                    ret.SendUnitName = sendunitemodel == null ? "" : sendunitemodel.Name;
                }

                ret.ProjectName = projectmodel.ProjectName;
                ret.ProjectCode = projectmodel.ProjectCode;
                ret.AppraisalTypeId = projectmodel.AppraisalTypeId;
                ret.ProjectId = projectmodel.Id;
                //if (string.IsNullOrWhiteSpace(projectmodel.PersonLiable))
                //    ret.ProjectLeader = "";
                //else
                //{
                //    var usermodel = await _userRepository.GetAsync(Int64.Parse(projectmodel.PersonLiable));
                //    ret.ProjectLeader = usermodel.Name;

                //}
                var leaderModel = from a in _projectAuditMemberRepository.GetAll()
                                  join u in _userRepository.GetAll() on a.UserId equals u.Id
                                  where a.ProjectBaseId == projectId && a.UserAuditRole == 1
                                  select u.Name;
                ret.ProjectLeader = leaderModel.FirstOrDefault() ?? "";
                var query = from memeber in _projectAuditMemberRepository.GetAll()
                            join user in _userRepository.GetAll() on memeber.UserId equals user.Id
                            where memeber.UserAuditRole == 2 && memeber.ProjectBaseId == projectmodel.Id
                            select user.Name;
                ret.ProjectReviewer = "";
                var membsers = query.ToList();
                foreach (var name in membsers)
                {
                    if (!string.IsNullOrWhiteSpace(ret.ProjectReviewer))
                        ret.ProjectReviewer = ret.ProjectReviewer + ",";
                    ret.ProjectReviewer = ret.ProjectReviewer + name; ;
                }
                ret.StartDate = projectmodel.CreationTime;

                var auditRole = (int)AuditRoleEnum.复核人三;
                var threadAuditModel = from a in _projectAuditMemberRepository.GetAll()
                                       join b in _projectAuditMemberResultRepository.GetAll() on a.Id equals b.Pid
                                       where a.ProjectBaseId == projectmodel.Id && a.UserAuditRole == auditRole
                                       select b;
                ret.AuditAmount = threadAuditModel.FirstOrDefault()?.AuditAmount ?? null;
            }


            return ret;

        }

        public async Task<DispatchPublishOutput> GetDispatchForView(Guid id)
        {
            var model = await _dispatchMessageRepository.GetAll().FirstOrDefaultAsync(p => p.Id == id);
            if (model == null)
                return new DispatchPublishOutput();

            return new DispatchPublishOutput
            {
                Id = model.Id,
                Additional = model.Additional,
                DispatchCode = model.DispatchCode,
                EndDate = model.EndDate,
                ProjectName = model.ProjectName,
                ProjectLeader = model.ProjectLeader,
                ProjectReviewer = model.ProjectReviewer,
                ProjectUndertakeCode = model.ProjectUndertakeCode,
                Reason = model.Reason,
                SendUnitName = model.SendUnitName,
                Remark = model.Remark,
                StartDate = model.StartDate,
                ProjectId = model.ProjectId
            };
        }


    }
}