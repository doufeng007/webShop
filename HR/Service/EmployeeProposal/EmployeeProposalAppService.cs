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
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.File;
using Abp.WorkFlow;
using Newtonsoft.Json;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp.Events.Bus;
using Abp.Application.Services;

namespace HR
{
    [AbpAuthorize]
    public class EmployeeProposalAppService : FRMSCoreAppServiceBase, IEmployeeProposalAppService
    {
        private readonly IRepository<EmployeeProposal, Guid> _repository;
        private readonly IRepository<UserPosts, Guid> _userPostRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizeRepository;
        private readonly IRepository<PostInfo, Guid> _postRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IWorkFlowOrganizationUnitsAppService _unitsAppService;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        public IEventBus _eventBus { get; set; }
        public EmployeeProposalAppService(IRepository<EmployeeProposal, Guid> repository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager,
            IRepository<UserPosts, Guid> userPostRepository, IRepository<PostInfo, Guid> postRepository, IRepository<WorkFlowOrganizationUnits, long> organizeRepository,
            IRepository<User, long> userRepository, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager,
            IAbpFileRelationAppService abpFileRelationAppService, IWorkFlowOrganizationUnitsAppService unitsAppService, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizeRepository
            , ProjectAuditManager projectAuditManager, WorkFlowCacheManager workFlowCacheManager, IWorkFlowTaskRepository workFlowTaskRepository)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _userPostRepository = userPostRepository;
            _postRepository = postRepository;
            _organizeRepository = organizeRepository;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _unitsAppService = unitsAppService;
            _userOrganizeRepository = userOrganizeRepository;
            _projectAuditManager = projectAuditManager;
            _workFlowCacheManager = workFlowCacheManager;
            _workFlowTaskRepository = workFlowTaskRepository;
            _eventBus = NullEventBus.Instance;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>

        public async Task<PagedResultDto<EmployeeProposalListOutputDto>> GetList(GetEmployeeProposalListInput input)
        {

            var user = await base.GetCurrentUserAsync();
            var userId = user.Id.ToString();
            var queryBase = from a in _repository.GetAll().Where(x => !x.IsDeleted) select a;
            if (input.Type != null)
                queryBase = queryBase.Where(x => x.Type == input.Type);
            var query = from a in queryBase.Where(x => x.CreatorUserId == user.Id
                 )
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                  x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                  x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new EmployeeProposalListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            CreationTime = a.CreationTime,
                            ParticipateUser = a.ParticipateUser,
                            Status = a.Status ?? 0,
                            OpenModel =  openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            if (input.showMyCase == 1)
            {
                query = from a in queryBase.Where(x => (x.DealWithUsers.GetStrContainsArray(userId) ||
                                                                      x.CopyForUsers.GetStrContainsArray(userId))
                    )
                        let openModel = (from c in _workFlowTaskRepository.GetAll().Where(x =>
                                   x.FlowID == input.FlowId && x.InstanceID == a.Id.ToString() &&
                                   x.ReceiveID == AbpSession.UserId.Value)
                                         select c)
                        select new EmployeeProposalListOutputDto()
                        {
                            Id = a.Id,
                            // DepartmentInfos = post,
                            Title = a.Title,
                            CreationTime = a.CreationTime,
                            Status = a.Status ?? 0,
                            OpenModel =a.Status>-1&& openModel.Count(y => y.Type != 6 && (y.Status == 1 || y.Status == 0)) > 0
                                            ? 1
                                            : 2
                        };
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r=>r.OpenModel).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var result = ret.MapTo<List<EmployeeProposalListOutputDto>>();
            foreach (var item in result)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item);

                if (input.showMyCase == 0)
                {
                    if (!string.IsNullOrEmpty(item.ParticipateUser))
                    {
                        //默认取第一个收件人
                        var Postuser = Convert.ToInt32(MemberPerfix.RemovePrefix(item.ParticipateUser)?.Split(',')[0]);
                        item.UserName = _workFlowOrganizationUnitsManager.GetNames(item.ParticipateUser);
                        var userOrgModel = await _unitsAppService.GetUserPostInfo(
                            new NullableIdDto<long>() { Id = Postuser, }, new NullableIdDto<long>() { Id = null });
                        if (userOrgModel != null)
                        {
                            item.DepartmentName = userOrgModel.OrgId_Name;
                            item.PostName = userOrgModel.UserPosts.FirstOrDefault()?.PostName;
                        }
                    }
                }
                else
                {
                    var a = _repository.FirstOrDefault(x => x.Id == item.Id);
                    var departinfo = (
                    from b in UserManager.Users.Where(x => x.Id == a.CreatorUserId)
                    join c1 in _userOrganizeRepository.GetAll().DefaultIfEmpty() on b.Id
                        equals c1.UserId into tmp1
                    from c in tmp1.DefaultIfEmpty()
                    join e1 in _organizeRepository.GetAll().DefaultIfEmpty() on c.OrganizationUnitId equals e1.Id into tmp3
                    from e in tmp3.DefaultIfEmpty()
                    join d1 in _userPostRepository.GetAll().DefaultIfEmpty() on b.Id equals d1.UserId into tmp2
                    from d in tmp2.DefaultIfEmpty()
                    join f1 in _postRepository.GetAll().DefaultIfEmpty() on d.PostId equals f1.Id into tmp4
                    from f in tmp4.DefaultIfEmpty()
                    select new DepartmentInfo
                    {
                        WorkNumber = b.WorkNumber,
                        Name = b.Name,
                        PhoneNumber = b.PhoneNumber,
                        DepartmentName = e == null ? "" : e.DisplayName,
                        PostName = f == null ? "" : f.Name
                    }
                ).ToList();
                    item.DepartmentName = departinfo.FirstOrDefault()?.DepartmentName;
                    item.PostName = departinfo.FirstOrDefault()?.PostName;
                    item.WorkNumber = departinfo.FirstOrDefault()?.WorkNumber;
                    item.UserName = departinfo.FirstOrDefault()?.Name;
                    item.PhoneNumber = departinfo.FirstOrDefault()?.PhoneNumber;
                }
            }
            
            return new PagedResultDto<EmployeeProposalListOutputDto>(toalCount, result);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<EmployeeProposalOutputDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var output = model.MapTo<EmployeeProposalOutputDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.提案附件 });
            var createuserinfo = (
                from b in UserManager.Users.Where(x => model.CreatorUserId == x.Id)
                join c1 in _userOrganizeRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on b.Id
                    equals c1.UserId into tmp1
                from c in tmp1.DefaultIfEmpty()
                join e1 in _organizeRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on c.OrganizationUnitId equals e1.Id into tmp3
                from e in tmp3.DefaultIfEmpty()
                join d1 in _userPostRepository.GetAll()
                    .Where(x => !x.IsDeleted).DefaultIfEmpty() on b.Id equals d1.UserId into tmp2
                from d in tmp2.DefaultIfEmpty()
                join f1 in _postRepository.GetAll().Where(x => !x.IsDeleted).DefaultIfEmpty() on d.PostId equals f1.Id into tmp4
                from f in tmp4.DefaultIfEmpty()
                select new EmployeeProposalListOutputDto()
                {
                    WorkNumber = b.WorkNumber,
                    UserName = b.Name,
                    DepartmentName = e == null ? "" : e.DisplayName,
                    PostName = f == null ? "" : f.Name,
                    PhoneNumber = b.PhoneNumber
                }).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(model.ParticipateUser) == false)
            {
                output.ParticipateUser_Name = _workFlowOrganizationUnitsManager.GetNames(model.ParticipateUser);
            }
            if (createuserinfo != null)
            {
                output.WorkNumber = createuserinfo.WorkNumber;
                output.UserName = createuserinfo.UserName;
                output.DepartmentName = createuserinfo.DepartmentName;
                output.PostName = createuserinfo.PostName;
                output.PhoneNumber = createuserinfo.PhoneNumber;
            }

            if (model.OrgId.HasValue)
                output.OrgName = _organizeRepository.Get(model.OrgId.Value).DisplayName;
            if (model.IssueUserId.HasValue)
                output.IssueUserName = _workFlowOrganizationUnitsManager.GetNames("u_"+model.IssueUserId.ToString());
            return output;
        }

        /// <summary>
        /// 添加一个EmployeeProposal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(CreateEmployeeProposalInput input)
        {
            var newmodel = new EmployeeProposal()
            {
                Title = input.Title,
                Type = input.Type,
                Content = input.Content,
                IsIssue = input.IsIssue,
                ParticipateUser = input.ParticipateUser
            };
            newmodel.IssueUserId = AbpSession.UserId.Value;
            await _repository.InsertAsync(newmodel);
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = newmodel.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.提案附件,
                    Files = fileList
                });
            }
            return new InitWorkFlowOutput() { InStanceId = newmodel.Id.ToString() };
        }

        /// <summary>
        /// 修改一个EmployeeProposal
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateEmployeeProposalInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                var logModel = new EmployeeProposal();
                if (input.IsUpdateForChange)
                {
                    logModel = dbmodel.DeepClone();

                }
                dbmodel.Title = input.Title;
                dbmodel.Type = input.Type;
                dbmodel.Content = input.Content;
                dbmodel.Comment = input.Comment;
                dbmodel.IsIssue = input.IsIssue;
                dbmodel.OrgId = input.OrgId;
                dbmodel.SingleProjectId = input.SingleProjectId;
                dbmodel.IssueUserId = input.IssueUserId;
                dbmodel.IssueType = input.IssueType;
                if (!string.IsNullOrEmpty(input.ParticipateUser))
                {
                    dbmodel.ParticipateUser = input.ParticipateUser;
                }
                await _repository.UpdateAsync(dbmodel);
                if (input.IsUpdateForChange)
                {
                    var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
                    if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
                    var logs = GetChangeModel(logModel).GetColumnAllLogs(GetChangeModel(dbmodel));
                    await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);
                }
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
                    BusinessType = (int)AbpFileBusinessType.提案附件,
                    Files = fileList
                });

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
        private EmployeeProposalLogDto GetChangeModel(EmployeeProposal model)
        {
            var ret = model.MapTo<EmployeeProposalLogDto>();
            ret.TypeName = model.Type.GetLocalizedDescription();
            return ret;
        }
        [RemoteService(false)]
        public void InsertIssue(Guid id)
        {
            var model = _repository.GetAll().FirstOrDefault(x => x.Id == id);
            if (model == null)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            if (!model.IsIssue)
                return;
            _eventBus.Trigger(new MeetingIssueByEvent() {
                Name = model.Title,
                OrgId = model.OrgId,
                UserId = "u_"+model.IssueUserId.ToString(),
                Content = model.Content,
                SingleProjectId = model.SingleProjectId,
                RelationProposalId = model.Id,
                IssueType = Convert.ToInt32(model.IssueType)
            });
        }
    }
}