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
using Abp.File;
using ZCYX.FRMSCore.Application;
using Microsoft.Extensions.Configuration;
using Abp.Reflection.Extensions;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore;
using Abp.WorkFlowDictionary;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectAppService : FRMSCoreAppServiceBase, IProjectAppService
    {
        private readonly IRepository<ProjectBudget, Guid> _projectBudgetRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IProjectBaseRepository _projectBaseRepository;
        private readonly IRepository<ProjectBudgetControl, Guid> _projectBudgetControlRepository;
        private readonly IProjectFileRepository _projectFileRepository;
        private readonly IRepository<SingleProjectInfo, Guid> _singleProjectInfoRepository;
        private readonly IRepository<ProjectAuditMember, Guid> _projectAuditMemberRepository;
        private readonly IRepository<ProjectAuditRole> _projectAuditRoleRepository;
        private readonly WorkTaskManager _workTaskManager;
        //private readonly IBackgroudWorkJobWithHangFire _backgroudWorkJobWithHangFire;
        private readonly IRepository<AappraisalFileType, int> _aappraisalFileTypeRepository;
        private readonly IRepository<Code_AppraisalType, int> _code_AppraisalTypeRepository;
        private readonly IProjectListRepository _projectListRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _workFlowOrganizationUnitsRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<ProjectAreas, Guid> _projectAreaRepository;
        private readonly IRepository<ConstructionOrganizations> _constructionOrganizationsRepository;
        private readonly IWorkFlowTaskRepository _workFlowTaskRepository;
        private readonly IRepository<UserFollowProject, Guid> _userFollowProjectRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;
        private readonly IRepository<ChargeOrganizations> _chargeOrganizations;
        private readonly IRepository<ReplyUnit> _replyUnitProjectRepository;
        private readonly IRepository<BusinessDepartment> _bdRepository;
        private readonly WorkFlowCacheManager _workFlowCacheManager;
        private readonly ProjectAuditManager _projectAuditManager;
        private readonly IRepository<SingleProjectFee, Guid> _singleFeeRepository;
        private readonly IRepository<ProjectRealationUser, Guid> _projectRelationUser;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionary;
        private readonly IRepository<ProjectAuditGroup, Guid> _projectAuditGroupRepository;
        private readonly IRepository<ProjectAuditGroupUser, Guid> _projectAuditGroupUserRepository;
        private string AuditFlowId { get; set; }


        public ProjectAppService(IRepository<ProjectBudget, Guid> projectBudgetRepository,
            IRepository<User, long> userRepository,
            IRepository<ChargeOrganizations> chargeOrganizations,
            IProjectBaseRepository projectBaseRepository
            , IRepository<ProjectBudgetControl, Guid> projectBudgetControlRepository,
            IProjectFileRepository projectFileRepository, IRepository<SingleProjectInfo, Guid> singleProjectInfoRepository
            , IRepository<AappraisalFileType, int> aappraisalFileTypeRepository, IRepository<Code_AppraisalType, int> code_AppraisalTypeRepository, IProjectListRepository projectListRepository
            , IRepository<WorkFlowOrganizationUnits, long> workFlowOrganizationUnitsRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository,
            IRepository<ProjectAuditMember, Guid> projectAuditMemberRepository, IRepository<ProjectAuditRole> projectAuditRoleRepository
            , IAbpFileRelationAppService abpFileRelationAppService, IRepository<ProjectAreas, Guid> projectAreaRepository, IWorkFlowTaskRepository workFlowTaskRepository
            , IRepository<ConstructionOrganizations> constructionOrganizationsRepository, IRepository<UserFollowProject, Guid> userFollowProjectRepository
            , WorkTaskManager workTaskManager, WorkFlowTaskManager workFlowTaskManager, WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager, IRepository<AbpDictionary, Guid> abpDictionary
            , IRepository<ReplyUnit> replyUnitProjectRepository, IRepository<BusinessDepartment> bdRepository, WorkFlowCacheManager workFlowCacheManager
            , ProjectAuditManager projectAuditManager, IRepository<SingleProjectFee, Guid> singleRepository, IRepository<ProjectRealationUser, Guid> projectRelationUser
            , IRepository<ProjectAuditGroup, Guid> projectAuditGroupRepository, IRepository<ProjectAuditGroupUser, Guid> projectAuditGroupUserRepository)
        {
            _abpDictionary = abpDictionary;
            _projectBudgetRepository = projectBudgetRepository;
            _userRepository = userRepository;
            _projectBaseRepository = projectBaseRepository;
            _projectBudgetControlRepository = projectBudgetControlRepository;
            _projectFileRepository = projectFileRepository;
            _singleProjectInfoRepository = singleProjectInfoRepository;
            _aappraisalFileTypeRepository = aappraisalFileTypeRepository;
            _code_AppraisalTypeRepository = code_AppraisalTypeRepository;
            _projectListRepository = projectListRepository;
            _chargeOrganizations = chargeOrganizations;
            _workFlowOrganizationUnitsRepository = workFlowOrganizationUnitsRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _projectAuditMemberRepository = projectAuditMemberRepository;
            _projectAuditRoleRepository = projectAuditRoleRepository;
            //_backgroudWorkJobWithHangFire = backgroudWorkJobWithHangFire;
            _abpFileRelationAppService = abpFileRelationAppService;
            _projectAreaRepository = projectAreaRepository;
            _workFlowTaskRepository = workFlowTaskRepository;
            _constructionOrganizationsRepository = constructionOrganizationsRepository;
            _userFollowProjectRepository = userFollowProjectRepository;
            _workTaskManager = workTaskManager;
            var coreAssemblyDirectoryPath = typeof(ProjectAuditStopAppService).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            AuditFlowId = _appConfiguration["App:AuditFlow"];
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
            _replyUnitProjectRepository = replyUnitProjectRepository;
            _workFlowCacheManager = workFlowCacheManager;
            _projectAuditManager = projectAuditManager;
            _bdRepository = bdRepository;
            _singleFeeRepository = singleRepository;
            _projectRelationUser = projectRelationUser;
            _projectAuditGroupRepository = projectAuditGroupRepository;
            _projectAuditGroupUserRepository = projectAuditGroupUserRepository;
        }

        public async Task<PagedResultDto<ProjectByXmfzrListOutput>> GetProjectByXmfzrList(GetProjectByXmfzrInput input)
        {
            var query = from a in _singleProjectInfoRepository.GetAll()
                        select new ProjectByXmfzrListOutput
                        {
                            Id = a.Id,
                            ProjectName = a.SingleProjectName,
                            CreationTime = a.CreationTime
                        };
            if (input.IsXmfzr)
            {
                var groupUsers = _projectAuditGroupUserRepository.GetAll().Where(y => y.UserId == AbpSession.UserId.Value && y.UserRole == 1).Select(y => y.GroupId).ToList();
                var projectBases = _projectAuditMemberRepository.GetAll().Where(x => x.GroupId.HasValue && groupUsers.Contains(x.GroupId.Value)).Select(x => x.ProjectBaseId);
                query = query.Where(x => projectBases.Contains(x.Id));
            }
            if (!string.IsNullOrEmpty(input.SearchKey))
            {
                query = query.Where(x => x.ProjectName.Contains(input.SearchKey));
            }
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<ProjectByXmfzrListOutput>(toalCount, ret);
        }

        public async Task<List<CreateOrUpdateProjectFileInput>> GetPreProjectFiles(int appraisalTypeId)
        {
            if (appraisalTypeId == 0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "项目类型参数错误");
            var ret = new List<CreateOrUpdateProjectFileInput>();
            var filetypes = new List<AappraisalFileType>();
            GetProjectFileByAppraisalTypeId(appraisalTypeId, filetypes);
            filetypes = filetypes.OrderBy(r => r.Sort).OrderByDescending(ite => ite.CreationTime).ToList();
            foreach (var item in filetypes)
            {
                ret.Add(new CreateOrUpdateProjectFileInput
                {
                    AappraisalFileType = item.Id,
                    AappraisalFileTypeName = item.Name,
                    IsPaperFile = false,
                    HasUpload = false,
                    IsMust = item.IsMust,
                    PaperFileNumber = 0
                });
            }
            return ret;
        }





        /// <summary>
        /// 创建项目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> CreateAsync(CreateOrUpdateProJectBudgetManagerInput input)
        {
            var inputmodel = new GetProjectBudgetForEditOutput(input);
            var projectbase = inputmodel.BaseOutput.MapTo<ProjectBase>();
            var singleInfos = input.SingleProjectInfos.MapTo<List<SingleProjectInfo>>();
            if (string.IsNullOrWhiteSpace(input.ProjectName))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请填写项目名称");
            if (!string.IsNullOrWhiteSpace(input.SingleProjectCode))
            {
                var exitemodelBySingleProjectCode =
                    await _projectBaseRepository.GetAll()
                        .Where(r => r.SingleProjectCode == input.SingleProjectCode)
                        .CountAsync();
                if (exitemodelBySingleProjectCode > 0)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "单项编码不能重复");
                }
            }

            if (input.SendTime > DateTime.Now)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "送审时间不能超过当日");
            projectbase.Id = Guid.NewGuid();
            await _projectBaseRepository.InsertAsync(projectbase);


            foreach (var item in input.SingleProjectInfos)
            {
                var singleModel = item.MapTo<SingleProjectInfo>();
                singleModel.Id = Guid.NewGuid();
                singleModel.ProjectId = projectbase.Id;
                singleModel.ProjectCode = projectbase.ProjectCode;
                await _singleProjectInfoRepository.InsertAsync(singleModel);
                var singleFeeModel = item.SingleFee.MapTo<SingleProjectFee>();
                CalculateFee(ref singleFeeModel);
                singleFeeModel.Id = Guid.NewGuid();
                singleFeeModel.ProjectId = projectbase.Id;
                singleFeeModel.SingleProjectId = singleModel.Id;
                await _singleFeeRepository.InsertAsync(singleFeeModel);

                item.ProjectFiles.ForEach(r =>
                {
                    var entity = new ProjectFile();
                    r.MapTo(entity);
                    entity.Id = Guid.NewGuid();
                    entity.ProjectBaseId = singleModel.ProjectId;
                    entity.SingleProjectId = singleModel.Id;
                    if (r.IsPaperFile || r.Files.Count > 0)
                    {
                        r.HasUpload = true;
                    }
                    else
                        r.HasUpload = false;

                    var fileList = new List<AbpFileListInput>();
                    foreach (var filemodel in r.Files)
                    {
                        var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                        fileList.Add(fileone);
                    }

                    _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                    {
                        BusinessId = entity.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.送审资料,
                        Files = fileList
                    });
                    _projectFileRepository.InsertAsync(entity);
                });


                var controls = item.ProjectBudgetControls.MapTo<List<ProjectBudgetControl>>();

                controls.ForEach(r =>
                {
                    r.Id = Guid.NewGuid();
                    r.Pro_Id = projectbase.Id;
                    r.SingleProjectId = singleModel.Id;
                    _projectBudgetControlRepository.InsertAsync(r);
                });

            }

            await CurrentUnitOfWork.SaveChangesAsync();
            if (input.CertainSubmite == 1)
            {
                //_backgroudWorkJobWithHangFire.CreateJobForProjectForceSubmit(projectbase.Id);
            }
            return new InitWorkFlowOutput() { InStanceId = projectbase.Id.ToString() };





        }

        private  void CalculateFee(ref SingleProjectFee model)
        {
            model.PreliminaryFee = (model.FeasibilityStudyFee ?? 0) + (model.SurveyFee ?? 0) + (model.DesignFee ?? 0) + (model.ResearchTestFee ?? 0) + (model.EnvironmentalImpactFee ?? 0) + (model.OtherFee ?? 0);
            model.ExpropriationFee = (model.LandAcquisitionFee ?? 0) + (model.LandReclamationFee ?? 0);
            model.ProjectDeviceFee = (model.DeviceFee ?? 0) + (model.InstallFee ?? 0) + (model.ConstructionFee ?? 0);
            model.OtherTotalFee = (model.SupervisorFee ?? 0) + (model.LandUseTax ?? 0) + (model.FarmlandOccupationTax ?? 0) + (model.vehicleAndVesselTax ?? 0) + (model.StampDutyFee ?? 0) + (model.TemporaryFacilitiesFee ?? 0) + (model.CulturaRrelicsProtectionFee ?? 0) + (model.ForestRrestorationFee ?? 0) + (model.SafetyProductionFee ?? 0) + (model.SafetyAssessmentFee ?? 0) + (model.NetworkRentalFee ?? 0) + (model.BuldManagerFee ?? 0)
                + (model.ACMF ?? 0) + (model.EngineeringInsuranceFee ?? 0) + (model.BiddingFee ?? 0) + (model.ContractNotarialFee ?? 0) + (model.EngineeringInspectionFee ?? 0)
                + (model.EquipmentInspectionFee ?? 0) + (model.CombinedTestFee ?? 0) + (model.AttorneyFee ?? 0) + (model.ChannelMaintenanceFee ?? 0) + (model.NavigationAidsFee ?? 0)
                + (model.AerialSurveyFee ?? 0) + (model.UnforeseenFee ?? 0) + (model.SystemoperationFee ?? 0) + (model.AuditFee ?? 0) + (model.OtherFee2 ?? 0);
            model.TotalFee = model.PreliminaryFee + model.ExpropriationFee + model.ProjectDeviceFee + model.OtherTotalFee;          
        }

        public async Task<GetProjectBudgetForEditOutput> GetProjectBudgetForEdit(GetProjectForEditInput input)
        {
            var model = new GetProjectBudgetForEditOutput();
            var baseInfo = await _projectBaseRepository.GetAsync(input.Id);
            model.BaseOutput = baseInfo.MapTo<CreateOrUpdateProjectBaseInput>();
            if (baseInfo.CompetentUnit.HasValue)
            {
                var c = _chargeOrganizations.FirstOrDefault(ite => ite.Id == baseInfo.CompetentUnit.Value);
                if (c != null)
                    model.BaseOutput.CompetentUnit_Name = c.Name;
            }
            if (!model.BaseOutput.Industry.IsNullOrEmpty())
            {
                var industry = _abpDictionary.FirstOrDefault(x => x.Id == model.BaseOutput.Industry.ToGuid());
                model.BaseOutput.Industry_Name = industry?.Title;
            }
            if (model.BaseOutput.Area_Id.HasValue)
            {
                var areaModel = await _projectAreaRepository.GetAsync(model.BaseOutput.Area_Id.Value);
                model.BaseOutput.AreaName = areaModel.Name;
            }
            if (model.BaseOutput.UnitRoom.HasValue)
            {
                var bdModel = await _bdRepository.GetAsync(model.BaseOutput.UnitRoom.Value);
                model.BaseOutput.UnitRoomName = bdModel.Name;
            }

            var singleInfoquery = from s in _singleProjectInfoRepository.GetAll()
                                  where s.ProjectId == baseInfo.Id
                                  select s;
            var singleInfos = await singleInfoquery.OrderBy(x=>x.CreationTime).ToListAsync();
            model.SingleProjectInfos = singleInfos.MapTo<List<SingleProjectInfoOutput>>();
            foreach (var item in model.SingleProjectInfos)
            {
                if (!item.DeparmentId.IsNullOrEmpty())
                {
                    item.DeparmentId = item.DeparmentId.Replace("l_", "");
                    var code = "";
                    var orgid = "";
                    item.DeparmentId_Name = _workFlowOrganizationUnitsManager.GetName(item.DeparmentId, out code, out orgid);
                }
                if (!item.Industry.IsNullOrEmpty())
                {
                    var industry = _abpDictionary.FirstOrDefault(x=>x.Id==item.Industry.ToGuid());
                    item.Industry_Name = industry?.Title;
                }
            }
            var projectBudgetControls = await (from a in _projectBudgetControlRepository.GetAll()
                                               join b in _abpDictionary.GetAll() on a.Code equals b.Id.ToString()
                                               where a.Pro_Id == input.Id
                                               select new ProjectBudgetControlOutput
                                               {
                                                   ApprovalMoney = a.ApprovalMoney,
                                                   Code = a.Code,
                                                   CodeName = b.Title,
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Pro_Id = a.Pro_Id,
                                                   SendMoney = a.SendMoney,
                                                   ValidationMoney = a.ValidationMoney,
                                                   SingleProjectId = a.SingleProjectId,

                                               }).ToListAsync();

            var projectFees = await _singleFeeRepository.GetAll().Where(r => r.ProjectId == input.Id).ToListAsync();

            var projectFilemodel = from file in _projectFileRepository.GetAll()
                                   join appfile in _aappraisalFileTypeRepository.GetAll() on file.AappraisalFileType equals appfile.Id
                                   where file.ProjectBaseId == input.Id && (!input.AuditRoleId.HasValue || (input.AuditRoleId.HasValue && appfile.AuditRoleIds.Contains(input.AuditRoleId.ToString())))
                                   orderby appfile.Sort
                                   orderby file.CreationTime descending
                                   select new CreateOrUpdateProjectFileInput()
                                   {
                                       Id = file.Id,
                                       AappraisalFileType = appfile.Id,
                                       AappraisalFileTypeName = appfile.Name,
                                       FileName = file.FileName,
                                       HasUpload = file.HasUpload,
                                       IsPaperFile = file.IsPaperFile,
                                       ProjectBaseId = input.Id,
                                       FilePath = file.FilePath,
                                       IsMust = file.IsMust,
                                       PaperFileNumber = file.PaperFileNumber,
                                       Back = file.Back,
                                       SingleProjectId = file.SingleProjectId,
                                   };
            var projectFilemodelList = projectFilemodel.ToList();
            foreach (var file in projectFilemodelList)
                file.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = file.Id.ToString(), BusinessType = (int)AbpFileBusinessType.送审资料 });
            foreach (var item in model.SingleProjectInfos)
            {
                item.SingleFee = projectFees.SingleOrDefault(r => r.SingleProjectId == item.Id).MapTo<SingleProjectFeeOutput>();
                item.ProjectBudgetControls = projectBudgetControls.Where(r => r.SingleProjectId == item.Id).MapTo<List<ProjectBudgetControlOutput>>();
                item.ProjectFiles = projectFilemodelList.Where(r => r.SingleProjectId == item.Id).ToList();
            }

            if (model.BaseOutput.SendUnit != 0)
                model.BaseOutput.SendUnitName = (await _constructionOrganizationsRepository.GetAsync(model.BaseOutput.SendUnit)).Name;
            return model;
        }


        public async Task<GetProjectBudgetForEditOutput> GetSingleProject(GetSingleProjectInput input)
        {
            var model = new GetProjectBudgetForEditOutput();
            var singleInfo = await _singleProjectInfoRepository.GetAsync(input.Id);
            var baseInfo = await _projectBaseRepository.GetAsync(singleInfo.ProjectId);
            model.BaseOutput = baseInfo.MapTo<CreateOrUpdateProjectBaseInput>();
            model.BaseOutput.ProjectStatus = singleInfo.ProjectStatus;
            if (baseInfo.CompetentUnit.HasValue)
            {
                var c = _chargeOrganizations.FirstOrDefault(ite => ite.Id == baseInfo.CompetentUnit.Value);
                if (c != null)
                    model.BaseOutput.CompetentUnit_Name = c.Name;
            }
            if (model.BaseOutput.Area_Id.HasValue)
            {
                var areaModel = await _projectAreaRepository.GetAsync(model.BaseOutput.Area_Id.Value);
                model.BaseOutput.AreaName = areaModel.Name;
            }
           
            var singleModel = singleInfo.MapTo<SingleProjectInfoOutput>();
            if (singleModel.GroupId.HasValue)
            {
                singleModel.GroupName = (await _projectAuditGroupRepository.GetAsync(singleModel.GroupId.Value)).Name;
            }
            var projectBudgetControls = await (from a in _projectBudgetControlRepository.GetAll()
                                               join b in _abpDictionary.GetAll() on a.Code equals b.Id.ToString()
                                               where a.SingleProjectId == input.Id
                                               select new ProjectBudgetControlOutput
                                               {
                                                   ApprovalMoney = a.ApprovalMoney,
                                                   Code = a.Code,
                                                   CodeName = b.Title,
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Pro_Id = a.Pro_Id,
                                                   SendMoney = a.SendMoney,
                                                   ValidationMoney = a.ValidationMoney,
                                                   SingleProjectId = a.SingleProjectId,

                                               }).ToListAsync();

            var projectFees = await _singleFeeRepository.GetAll().Where(r => r.SingleProjectId == input.Id).ToListAsync();

            var projectFilemodel = from file in _projectFileRepository.GetAll()
                                   join appfile in _aappraisalFileTypeRepository.GetAll() on file.AappraisalFileType equals appfile.Id
                                   where file.SingleProjectId == input.Id && (!input.AuditRoleId.HasValue || (input.AuditRoleId.HasValue && appfile.AuditRoleIds.Contains(input.AuditRoleId.ToString())))
                                   orderby appfile.Sort
                                   orderby file.CreationTime descending
                                   select new CreateOrUpdateProjectFileInput()
                                   {
                                       Id = file.Id,
                                       AappraisalFileType = appfile.Id,
                                       AappraisalFileTypeName = appfile.Name,
                                       FileName = file.FileName,
                                       HasUpload = file.HasUpload,
                                       IsPaperFile = file.IsPaperFile,
                                       ProjectBaseId = input.Id,
                                       FilePath = file.FilePath,
                                       IsMust = file.IsMust,
                                       PaperFileNumber = file.PaperFileNumber,
                                       Back = file.Back,
                                       SingleProjectId = file.SingleProjectId,
                                   };
            var projectFilemodelList = projectFilemodel.ToList();
            foreach (var file in projectFilemodelList)
                file.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = file.Id.ToString(), BusinessType = (int)AbpFileBusinessType.送审资料 });

            singleModel.SingleFee = projectFees.FirstOrDefault().MapTo<SingleProjectFeeOutput>();
            singleModel.ProjectBudgetControls = projectBudgetControls.MapTo<List<ProjectBudgetControlOutput>>();
            singleModel.ProjectFiles = projectFilemodelList.ToList();

            if (!singleModel.DeparmentId.IsNullOrEmpty())
            {
                singleModel.DeparmentId = singleModel.DeparmentId.Replace("l_", "");
                var code = "";
                var orgid = "";
                singleModel.DeparmentId_Name = _workFlowOrganizationUnitsManager.GetName(singleModel.DeparmentId, out code, out orgid);
            }
            model.SingleProjectInfos = new List<SingleProjectInfoOutput>() { singleModel };
            foreach (var item in model.SingleProjectInfos)
            {
                if (!item.Industry.IsNullOrWhiteSpace())
                    item.Industry_Name = (_abpDictionary.Get(item.Industry.ToGuid())).Title;
            }
            if (model.BaseOutput.SendUnit != 0)
                model.BaseOutput.SendUnitName = (await _constructionOrganizationsRepository.GetAsync(model.BaseOutput.SendUnit)).Name;
            return model;
        }

        /// <summary>
        /// 获取项目评审所有数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetProjectBudgetForEditOutput> GetProjectSingleAllInfo(GetSingleProjectInput input)
        {
            var model = await GetSingleProject(input);
            var server = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ProjectResultAppService>();
            model.AuditResultOutput.LeaderResult = await server.GetAuditMemberResult(input.Id, (int)AuditRoleEnum.项目负责人);
            model.AuditResultOutput.ReviewResult = await server.GetAuditMemberResult(input.Id, (int)AuditRoleEnum.复核人);
            model.AuditResultOutput.ReviewResult2 = await server.GetAuditMemberResult(input.Id, (int)AuditRoleEnum.复核人二);
            model.AuditResultOutput.ReviewResult3 = await server.GetAuditMemberResult(input.Id, (int)AuditRoleEnum.复核人三);
            model.AuditResultOutput.FinanceResult1 = await server.GetAuditMemberResult(input.Id, (int)AuditRoleEnum.财务初审);
            model.AuditResultOutput.FinanceResult2 = await server.GetAuditMemberResult(input.Id, (int)AuditRoleEnum.财务评审);
            model.AuditResultOutput.Finishs = await server.GetFinishResult(input.Id, (int)AuditRoleEnum.汇总人员);


            var auditMembersquery = from am in _projectAuditMemberRepository.GetAll()
                                    join r in _projectAuditRoleRepository.GetAll() on am.UserAuditRole equals r.Id
                                    join user in _userRepository.GetAll() on am.UserId equals user.Id
                                    where am.ProjectBaseId == input.Id
                                    orderby am.UserAuditRole
                                    select new CreateOrUpdateProjectAuditMembersInput { Id = am.Id, ProjectBaseId = am.ProjectBaseId, UserAuditRole = am.UserAuditRole, UserAuditRoleName = r.Name, UserId = am.UserId, UserName = user.Name, FlowId = am.FlowId };

            var auditMembers = await auditMembersquery.ToListAsync();
            model.ProjectAuditMembersOutput = auditMembers;

            if (model.BaseOutput.SendUnit != 0)
            {
                model.BaseOutput.SendUnitName = (await _constructionOrganizationsRepository.GetAsync(model.BaseOutput.SendUnit)).Name;
            }
            if (model.BaseOutput.Area_Id.HasValue)
            {
                var projectAreaModel = await _projectAreaRepository.GetAsync(model.BaseOutput.Area_Id.Value);
                model.BaseOutput.AreaName = projectAreaModel.Name;
            }


            return model;
        }

        public async Task UpdateAsync(CreateOrUpdateProJectBudgetManagerInput input)
        {
            if (input.Id.HasValue)
            {
                await UpdateProjectAsync(input);
            }
            //else
            //{
            //    await CreateProjectAsync(input);
            //}
        }


        //public async Task DeleteAsync(EntityDto<Guid> input)
        //{
        //    var period = await _projectBudgetRepository.GetAsync(input.Id);
        //    await _projectBudgetRepository.DeleteAsync(period);
        //}



        [AbpAuthorize]
        public async Task UpdateForChangeAsync(ProJectBudgetUpdateChangeInput input)
        {
            await GetProjectChangeModel(input);

            if (string.IsNullOrWhiteSpace(input.ProjectName))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请填写项目名称");
            if (input.SendTime > DateTime.Now)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "送审时间不能超过当日");

            var inputmodel = new GetProjectBudgetForEditOutput(input);
            var db_projectBase = await _projectBaseRepository.GetAsync(input.Id.Value);
            inputmodel.BaseOutput.MapTo(db_projectBase);
            await _projectBaseRepository.UpdateAsync(db_projectBase);

            var db_ProjectModel = new GetProjectBudgetForEditOutput();
            if (input.IsChangeSingle)
                db_ProjectModel = await GetSingleProject(new GetSingleProjectInput() { AppraisalTypeId = input.AppraisalTypeId, Id = input.SingleProjectId.Value });
            else
                db_ProjectModel = await GetProjectBudgetForEdit(new GetProjectForEditInput() { AppraisalTypeId = input.AppraisalTypeId, Id = input.Id.Value, });
            foreach (var item in db_ProjectModel.SingleProjectInfos)
            {
                if (input.SingleProjectInfos.Any(r => r.Id == item.Id))  //编辑
                {
                    var inputSingelInfo = input.SingleProjectInfos.FirstOrDefault(r => r.Id == item.Id);
                    var db_singleModel = _singleProjectInfoRepository.Get(item.Id);
                    db_singleModel.SingleProjectName = inputSingelInfo.SingleProjectName;
                    db_singleModel.SingleProjectCode = inputSingelInfo.SingleProjectCode;
                    db_singleModel.ProjectNature = inputSingelInfo.ProjectNature;
                    db_singleModel.Industry = inputSingelInfo.Industry;
                    db_singleModel.Industry_Name = inputSingelInfo.Industry_Name;
                    db_singleModel.SingleProjectbudget = inputSingelInfo.SingleProjectbudget;
                    db_singleModel.SingleProjectSafaBudget = inputSingelInfo.SingleProjectSafaBudget;
                    db_singleModel.SingleProjectCode = input.ProjectCode;

                    //费用
                    var FeeEntity = await _singleFeeRepository.SingleAsync(r => r.Id == item.SingleFee.Id);
                    inputSingelInfo.SingleFee.MapTo(FeeEntity);

                    //控制信息
                    var add_projectBudgetControlModel = inputSingelInfo.ProjectBudgetControls.Where(r => r.Id == null).ToList();
                    add_projectBudgetControlModel.ForEach(r =>
                    {
                        var entity = r.MapTo<ProjectBudgetControl>();
                        entity.Pro_Id = input.Id.Value;
                        entity.SingleProjectId = item.Id;
                        _projectBudgetControlRepository.Insert(entity);
                    });
                    var less_projectBudgetControlIds =
                        item.ProjectBudgetControls.Select(r => r.Id)
                            .ToList()
                            .Except(
                                inputSingelInfo.ProjectBudgetControls.Where(r => r.Id.HasValue).Select(o => o.Id.Value).ToList())
                            .ToList();
                    less_projectBudgetControlIds.ForEach(r =>
                    {
                        _projectBudgetControlRepository.Delete(o => o.Id == r);
                    });
                    var update_projectBudgetControls =
                        inputSingelInfo.ProjectBudgetControls.Where(
                            r => r.Id != null && !less_projectBudgetControlIds.Contains(r.Id.Value)).ToList();
                    update_projectBudgetControls.ForEach(r =>
                    {
                        var db_projectBudgetControl = _projectBudgetControlRepository.FirstOrDefault(o => o.Id == r.Id.Value);
                        r.MapTo(db_projectBudgetControl);
                        _projectBudgetControlRepository.Update(db_projectBudgetControl);
                    });

                    //送审资料
                    foreach (var projectFile in item.ProjectFiles)
                    {
                        var db_ProjectFile = await _projectFileRepository.GetAsync(projectFile.Id.Value);
                        var newfile = inputSingelInfo.ProjectFiles.FirstOrDefault(r => r.Id == projectFile.Id);
                        if (newfile == null)
                        {
                            continue;
                        }
                        newfile.ProjectBaseId = input.Id.Value;
                        if (newfile.IsPaperFile || newfile.Files.Count > 0)
                        {
                            db_ProjectFile.HasUpload = true;
                        }
                        else
                            db_ProjectFile.HasUpload = false;

                        db_ProjectFile.IsPaperFile = newfile.IsPaperFile;
                        db_ProjectFile.PaperFileNumber = newfile.PaperFileNumber;
                        db_ProjectFile.Back = newfile.Back;
                        var fileList = new List<AbpFileListInput>();
                        foreach (var filemodel in newfile.Files)
                        {
                            var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                            fileList.Add(fileone);
                        }
                        _abpFileRelationAppService.Update(new CreateFileRelationsInput() { BusinessId = db_ProjectFile.Id.ToString(), BusinessType = (int)AbpFileBusinessType.送审资料, Files = fileList });
                        await _projectFileRepository.UpdateAsync(db_ProjectFile);
                    }

                }
                else   //删除
                {
                    await _singleFeeRepository.DeleteAsync(r => r.SingleProjectId == item.Id);
                    await _projectBudgetControlRepository.DeleteAsync(r => r.SingleProjectId == item.Id);
                    await _projectFileRepository.DeleteAsync(r => r.SingleProjectId == item.Id);
                    await _singleProjectInfoRepository.DeleteAsync(r => r.Id == item.Id);
                }

            }
            //新增
            foreach (var item in input.SingleProjectInfos.Where(r => !r.Id.HasValue))
            {
                var singleModel = item.MapTo<SingleProjectInfo>();
                singleModel.Id = Guid.NewGuid();
                singleModel.ProjectId = db_projectBase.Id;
                singleModel.SingleProjectCode = input.ProjectCode;

                await _singleProjectInfoRepository.InsertAsync(singleModel);
                var singleFeeModel = item.SingleFee.MapTo<SingleProjectFee>();
                singleFeeModel.Id = Guid.NewGuid();
                singleFeeModel.ProjectId = db_projectBase.Id;
                singleFeeModel.SingleProjectId = singleModel.Id;
                await _singleFeeRepository.InsertAsync(singleFeeModel);

                item.ProjectFiles.ForEach(r =>
                {
                    var entity = new ProjectFile();
                    r.MapTo(entity);
                    entity.Id = Guid.NewGuid();
                    entity.ProjectBaseId = singleModel.ProjectId;
                    entity.SingleProjectId = singleModel.Id;
                    if (r.IsPaperFile || r.Files.Count > 0)
                    {
                        r.HasUpload = true;
                    }
                    else
                        r.HasUpload = false;

                    var fileList = new List<AbpFileListInput>();
                    foreach (var filemodel in r.Files)
                    {
                        var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                        fileList.Add(fileone);
                    }

                    _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                    {
                        BusinessId = entity.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.送审资料,
                        Files = fileList
                    });
                    _projectFileRepository.InsertAsync(entity);
                });


                var controls = item.ProjectBudgetControls.MapTo<List<ProjectBudgetControl>>();

                controls.ForEach(r =>
                {
                    r.Id = Guid.NewGuid();
                    r.Pro_Id = db_projectBase.Id;
                    r.SingleProjectId = singleModel.Id;
                    _projectBudgetControlRepository.InsertAsync(r);
                });

            }

        }



        private async Task GetProjectChangeModel(ProJectBudgetUpdateChangeInput input)
        {
            var inputmodel = new GetProjectBudgetForEditOutput(input);
            var changeLogList = new List<LogColumnModel>();
            var db_projectBase = await _projectBaseRepository.GetAsync(input.Id.Value);
            db_projectBase.DeepClone<ProjectBase>();
            var old_projectBaseMOdel = db_projectBase.DeepClone();

            var old_ChangeModel = GetChangeModel(old_projectBaseMOdel);
            var newProject = inputmodel.BaseOutput.MapTo(db_projectBase);
            newProject.HasFinancialReview = old_projectBaseMOdel.HasFinancialReview;
            newProject.DeparmentId = old_projectBaseMOdel.DeparmentId;
            var new_ChangeModel = GetChangeModel(newProject);


            var singleInfoquery = from s in _singleProjectInfoRepository.GetAll()
                                  where s.ProjectId == input.Id
                                  && (!input.IsChangeSingle || s.Id == input.SingleProjectId.Value)
                                  select s;
            var singleInfos = await singleInfoquery.ToListAsync();
            var projectBudgetControls = await (from a in _projectBudgetControlRepository.GetAll()
                                               join b in _abpDictionary.GetAll() on a.Code equals b.Id.ToString()
                                               where a.Pro_Id == input.Id
                                               && (!input.IsChangeSingle || a.SingleProjectId == input.SingleProjectId.Value)
                                               select new ProjectBudgetControlOutput
                                               {
                                                   ApprovalMoney = a.ApprovalMoney,
                                                   Code = a.Code,
                                                   CodeName = b.Title,
                                                   Id = a.Id,
                                                   Name = a.Name,
                                                   Pro_Id = a.Pro_Id,
                                                   SendMoney = a.SendMoney,
                                                   ValidationMoney = a.ValidationMoney,
                                                   SingleProjectId = a.SingleProjectId,

                                               }).ToListAsync();

            var projectFees = await _singleFeeRepository.GetAll().Where(r => r.ProjectId == input.Id && (!input.IsChangeSingle || r.SingleProjectId == input.SingleProjectId.Value)).ToListAsync();

            var projectFilemodel = from file in _projectFileRepository.GetAll()
                                   join appfile in _aappraisalFileTypeRepository.GetAll() on file.AappraisalFileType equals appfile.Id
                                   where file.ProjectBaseId == input.Id
                                   && (!input.IsChangeSingle || file.SingleProjectId == input.SingleProjectId.Value)
                                   orderby appfile.Sort
                                   orderby file.CreationTime descending
                                   select new CreateOrUpdateProjectFileInput()
                                   {
                                       Id = file.Id,
                                       AappraisalFileType = appfile.Id,
                                       AappraisalFileTypeName = appfile.Name,
                                       FileName = file.FileName,
                                       HasUpload = file.HasUpload,
                                       IsPaperFile = file.IsPaperFile,
                                       ProjectBaseId = input.Id,
                                       FilePath = file.FilePath,
                                       IsMust = file.IsMust,
                                       PaperFileNumber = file.PaperFileNumber,
                                       Back = file.Back,
                                       SingleProjectId = file.SingleProjectId,
                                   };


            foreach (var item in singleInfos)
            {
                var old_SingleModel = new SingleProjectInfoChangeDto()
                {
                    Id = item.Id,
                    SingleProjectName = item.SingleProjectName,
                    SingleProjectCode = item.SingleProjectCode,
                    SingleProjectbudget = item.SingleProjectbudget,
                    SingleProjectSafaBudget = item.SingleProjectSafaBudget,
                    Industry = item.Industry_Name,
                    ProjectNature = ((ProjectNatureEnum)(int.Parse(item.ProjectNature))).ToString()
                };

                SingleFreeBind(ref old_SingleModel, projectFees.FirstOrDefault(r => r.SingleProjectId == item.Id));

                foreach (var control in projectBudgetControls.Where(r => r.SingleProjectId == item.Id))
                {
                    var entity = new ProjectControlChangeDto()
                    {
                        ApprovalMoney = control.ApprovalMoney,
                        CodeName = control.CodeName,
                        Id = control.Id,
                        Name = control.Name,
                        SendMoney = control.SendMoney,
                        ValidationMoney = control.ValidationMoney
                    };
                    old_SingleModel.Controls.Add(entity);
                }


                var oldfiles = await (from a in _projectFileRepository.GetAll()
                                      join bi in _aappraisalFileTypeRepository.GetAll() on a.AappraisalFileType equals bi.Id
                                      where a.ProjectBaseId == input.Id
                                      select new { ProjectFile = a, FileName = bi.Name }).ToListAsync();
                foreach (var projectfile in projectFilemodel.Where(r => r.SingleProjectId == item.Id))
                {
                    var entity = new ProjcetFileChangeDto()
                    {
                        AappraisalFileType_Name = projectfile.AappraisalFileTypeName,
                        Back = projectfile.Back,
                        Id = projectfile.Id.Value,
                        PaperFileNumber = projectfile.PaperFileNumber,
                    };
                    var fileData = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = projectfile.Id.Value.ToString(), BusinessType = (int)AbpFileBusinessType.送审资料 });
                    entity.Files = fileData.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList();
                    old_SingleModel.ProjectFiles.Add(entity);
                }

                old_ChangeModel.SingleProject.Add(old_SingleModel);
            }
            foreach (var item in input.SingleProjectInfos)
            {
                var new_SingelModel = new SingleProjectInfoChangeDto()
                {
                    Id = item.Id ?? Guid.Empty,
                    SingleProjectName = item.SingleProjectName,
                    SingleProjectCode = item.SingleProjectCode,
                    SingleProjectbudget = item.SingleProjectbudget,
                    SingleProjectSafaBudget = item.SingleProjectSafaBudget,
                    Industry = item.Industry_Name,
                    ProjectNature = ((ProjectNatureEnum)(int.Parse(item.ProjectNature))).ToString()
                };
               SingleFreeBind(ref new_SingelModel,item.SingleFee.MapTo<SingleProjectFee>());

                foreach (var control in item.ProjectBudgetControls)
                {
                    var entity = new ProjectControlChangeDto()
                    {
                        ApprovalMoney = control.ApprovalMoney,
                        CodeName = control.CodeName,
                        Id = control.Id ?? Guid.Empty,
                        Name = control.Name,
                        SendMoney = control.SendMoney,
                        ValidationMoney = control.ValidationMoney
                    };
                    new_SingelModel.Controls.Add(entity);
                }

                foreach (var projectFile in item.ProjectFiles)
                {
                    var entity_NewFile = new ProjcetFileChangeDto()
                    {
                        AappraisalFileType_Name = projectFile.AappraisalFileTypeName,
                        Back = projectFile.Back,
                        Files = projectFile.Files.Select(r => new AbpFileChangeDto { Id = r.Id, FileName = r.FileName }).ToList(),
                        Id = projectFile.Id ?? Guid.Empty,
                        PaperFileNumber = projectFile.PaperFileNumber,
                    };
                    new_SingelModel.ProjectFiles.Add(entity_NewFile);

                }

                new_ChangeModel.SingleProject.Add(new_SingelModel);
            }


            var flowModel = _workFlowCacheManager.GetWorkFlowModelFromCache(input.FlowId);
            if (flowModel == null) throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "流程不存在");
            var logs = old_ChangeModel.GetColumnAllLogs(new_ChangeModel);
            await _projectAuditManager.InsertAsync(logs, input.Id.ToString(), flowModel.TitleField.Table);

            if (input.ChangeType == 1)
            {
                var noticeService = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
                var noticeInput = new NoticePublishInputForWorkSpaceInput();
                noticeInput.ProjectId = db_projectBase.Id;
                noticeInput.Content = "补充资料,详情在项目查询工作室查看";
                noticeInput.UserType = 3;
                noticeInput.Title = $"项目：{db_projectBase.ProjectName} 补充资料";
                await noticeService.CreateProjectBaseWorkSpaceNotice(noticeInput);
            }
        }
        private void SingleFreeBind(ref SingleProjectInfoChangeDto input, SingleProjectFee model)
        {
            if (model != null) {
                CalculateFee(ref model);
                input.FeasibilityStudyFee = model.FeasibilityStudyFee;
                input.SurveyFee = model.SurveyFee;
                input.DesignFee = model.DesignFee;
                input.ResearchTestFee = model.ResearchTestFee;
                input.EnvironmentalImpactFee = model.EnvironmentalImpactFee;
                input.OtherFee = model.OtherFee;
                input.LandAcquisitionFee = model.LandAcquisitionFee;
                input.LandReclamationFee = model.LandReclamationFee;
                input.ConstructionFee = model.ConstructionFee;
                input.InstallFee = model.InstallFee;
                input.DeviceFee = model.DeviceFee;
                input.SupervisorFee = model.SupervisorFee;
                input.LandUseTax = model.LandUseTax;
                input.FarmlandOccupationTax = model.FarmlandOccupationTax;
                input.vehicleAndVesselTax = model.vehicleAndVesselTax;
                input.StampDutyFee = model.StampDutyFee;
                input.TemporaryFacilitiesFee = model.TemporaryFacilitiesFee;
                input.CulturaRrelicsProtectionFee = model.CulturaRrelicsProtectionFee;
                input.ForestRrestorationFee = model.ForestRrestorationFee;
                input.SafetyProductionFee = model.SafetyProductionFee;
                input.SafetyAssessmentFee = model.SafetyAssessmentFee;
                input.NetworkRentalFee = model.NetworkRentalFee;
                input.SystemoperationFee = model.SystemoperationFee;
                input.BuldManagerFee = model.BuldManagerFee;
                input.ACMF = model.ACMF;
                input.EngineeringInsuranceFee = model.EngineeringInsuranceFee;
                input.BiddingFee = model.BiddingFee;
                input.ContractNotarialFee = model.ContractNotarialFee;
                input.AuditFee = model.AuditFee;
                input.EngineeringInspectionFee = model.EngineeringInspectionFee;
                input.EquipmentInspectionFee = model.EquipmentInspectionFee;
                input.CombinedTestFee = model.CombinedTestFee;
                input.AttorneyFee = model.AttorneyFee;
                input.ChannelMaintenanceFee = model.ChannelMaintenanceFee;
                input.NavigationAidsFee = model.NavigationAidsFee;
                input.AerialSurveyFee = model.AerialSurveyFee;
                input.OtherFee2 = model.OtherFee2;
                input.UnforeseenFee = model.UnforeseenFee;
                input.PreliminaryFee = model.PreliminaryFee;
                input.ExpropriationFee = model.ExpropriationFee;
                input.ProjectDeviceFee = model.ProjectDeviceFee;
                input.OtherTotalFee = model.OtherTotalFee;
                input.TotalFee = model.TotalFee;
            }
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<GetSingleProjectInfoListOutput>> GetSingleProjectInfoList(GetSingleProjectInfoListInput input)
        {
            var query = (from a in _singleProjectInfoRepository.GetAll()
                         join p in _projectBaseRepository.GetAll() on a.ProjectId equals p.Id
                         join c in _code_AppraisalTypeRepository.GetAll() on p.AppraisalTypeId equals c.Id
                         join e in _constructionOrganizationsRepository.GetAll() on p.SendUnit equals e.Id into h
                         from e in h.DefaultIfEmpty()
                         select new GetSingleProjectInfoListOutput
                         {
                             Id = a.Id,
                             ProjectId = a.ProjectId,
                             SingleProjectName = a.SingleProjectName,
                             SingleProjectCode = a.SingleProjectCode,
                             SendUnitName = e == null ? "" : e.Name,
                             AppraisalTypeName = c.Name,
                             SendTime = p.SendTime,
                             CreationTime = a.CreationTime
                         });
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.SingleProjectName.Contains(input.SearchKey) ||x.SendUnitName.Contains(input.SearchKey) ||x.AppraisalTypeName.Contains(input.SearchKey));
            var count = await query.CountAsync();
            var data = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var ret = new PagedResultDto<GetSingleProjectInfoListOutput>(count, data);
            return ret;
        }
         [AbpAuthorize]
        public async Task<PagedResultDto<ProjectListGroupWithCodeDto>> GetProjectList(GetProjectListInput input)
        {
            var chargeleaderStr = MemberPerfix.UserPREFIX + AbpSession.UserId.Value;
            var orgmodel = await _workFlowOrganizationUnitsRepository.GetAll().Where(r => r.ChargeLeader.GetStrContainsArray(chargeleaderStr)).ToListAsync();
            var userList = new List<long>();
            if (orgmodel.Count() > 0)
            {
                var org_users = _workFlowOrganizationUnitsManager.GetAllByOrganizeIDArray(orgmodel.Select(r => r.Id).ToList().Distinct().ToArray());
                if (org_users.Count() > 0)
                {
                    userList.AddRange(org_users.Select(r => r.Id));
                }
            }

            userList.Add(AbpSession.UserId.Value);


            var query = (from a in _singleProjectInfoRepository.GetAll()
                         join p in _projectBaseRepository.GetAll() on a.ProjectId equals p.Id
                         join b in _projectRelationUser.GetAll() on a.ProjectId equals b.ProjectId into m
                         join c in _code_AppraisalTypeRepository.GetAll() on p.AppraisalTypeId equals c.Id
                         join d in _userFollowProjectRepository.GetAll() on new { ProjectId= a.ProjectId, UserId = AbpSession.UserId.Value }
                         equals new { ProjectId=d.Projectid, UserId = d.Userid } into ufp
                         join e in _constructionOrganizationsRepository.GetAll() on p.SendUnit equals e.Id into h
                         from e in h.DefaultIfEmpty()
                         where (m.Count() > 0 && m.Select(r => r.UserID).Intersect(userList).Count() > 0) || a.CreatorUserId == AbpSession.UserId.Value
                         group new ProjectListGroupWithCodeDto
                         {
                             ProjectCode = p.ProjectCode,
                             AppraisalTypeId = p.AppraisalTypeId,
                             AppraisalTypeName = c.Name,
                             CreationTime = p.CreationTime,
                             EntrustmentNumber = p.EntrustmentNumber,
                             Id = p.Id,
                             CreatorUserId = p.CreatorUserId.Value,
                             IsFollow = ufp.Count() > 0 ? true : false,
                             IsImportant = p.Is_Important ?? false,
                             ProjectName = p.ProjectName,
                             SendTime = p.SendTime,
                             SendTotalBudget = p.SendTotalBudget,
                             SendUnitName = e == null ? "" : e.Name,
                             HasComplete = p.Status == -1 ? true : false,
                         } by p.ProjectCode into g
                         select new { p = g.OrderByDescending(r => r.CreationTime).FirstOrDefault() }).Select(r => r.p);



            query = query.WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.ProjectName.Contains(input.SearchKey) || r.SendUnitName.Contains(input.SearchKey))
                .WhereIf(input.IsImportent, r => r.IsImportant)
                .WhereIf(input.IsFollow, r => r.IsFollow );

            var count = await query.CountAsync();
            var data = await query.OrderBy(r => r.HasComplete).ThenByDescending(r => r.IsFollow).ThenByDescending(r => r.IsImportant).ThenByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            var ret = new PagedResultDto<ProjectListGroupWithCodeDto>(count, data);
            return ret;


        }

        [AbpAuthorize]
        public async Task<List<ProjectSingleListDto>> GetSingleProjectByProjectCode(string projectCode)
        {
            var query = from a in _singleProjectInfoRepository.GetAll()
                        join b in _projectBaseRepository.GetAll() on a.ProjectId equals b.Id
                        join c in _projectAuditGroupRepository.GetAll() on a.GroupId equals c.Id into g
                        from c in g.DefaultIfEmpty()
                        where b.ProjectCode == projectCode
                        orderby a.CreationTime descending
                        select new ProjectSingleListDto()
                        {
                            GroupID = a.GroupId,
                            GroupName = c == null ? "" : c.Name,
                            Id = a.Id,
                            IsReturnAudit = a.IsReturnAudit ?? false,
                            IsStop = a.IsStop,
                            Status = a.Status,
                            ProjectSingleCode = a.SingleProjectCode,
                            ProjectSingleName = a.SingleProjectName,
                            CreatorUserId = a.CreatorUserId.Value,
                            CreationTime = a.CreationTime,
                        };

            var count = await query.CountAsync();
            var data = await query.ToListAsync();
            foreach (var item in data)
            {
                item.StatusSummary = _workFlowTaskManager.GetStatusTitle(AuditFlowId.ToGuid(), item.Status);
                if (item.CreatorUserId == AbpSession.UserId.Value)
                {
                    item.IsCanAdditionalProject = true;
                }
            }
            return data;
        }





        [AbpAuthorize]
        public async Task<ProjectTodoStaticDto> GetTodoStatic()
        {
            var model = new ProjectTodoStaticDto();
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
            var singleProject = from a in _singleProjectInfoRepository.GetAll()
                                join c in _projectAuditMemberRepository.GetAll() on a.Id equals c.ProjectBaseId
                                where underuserids.Contains(a.CreatorUserId.Value) || underuserids.Contains(c.UserId)
                                select a;
            singleProject = singleProject.Distinct();

            model.DoingCount = singleProject.Where(ite => ite.ProjectStatus.HasValue && (int)ite.ProjectStatus.Value > 0).Count();
            model.DoingSum = singleProject.Where(ite => ite.ProjectStatus.HasValue && (int)ite.ProjectStatus.Value > 0).Sum(ite => ite.SingleProjectSafaBudget);
            model.DoingSum = Math.Round(model.DoingSum / 10000, 0);
            model.DoneCount = singleProject.Where(ite => ite.Status == 1).Count();
            model.DoneSum = singleProject.Where(ite => ite.Status == 1).Sum(ite => ite.AuditAmount ?? 0);
            model.DoneSum = Math.Round(model.DoneSum / 10000, 0);
            model.TodoCount = singleProject.Where(ite => ite.ProjectStatus.HasValue == false || ite.ProjectStatus.Value == ProjectStatus.待审).Count();
            return model;



        }


        public string GetPy(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "";
            }
            string account = name.Trim().ToChineseSpell();
            var ret = account.IsNullOrEmpty() ? "" : account;
            return ret.ToUpper();
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task ReturnAuditProject(ReturnAuditProjectInput input)
        {
            var userId = AbpSession.GetUserId();
            var stepName = "";
            var taskModel = await _workFlowTaskRepository.FirstOrDefaultAsync(r => r.InstanceID == input.Id.ToString() && r.StepID == input.StepId);
            if (taskModel != null)
            {
                stepName = taskModel.StepName;
            }
            await _workTaskManager.InsertReturnAuditAsync(new ProjectWorkTask
            {
                UserId = userId,
                ProjectId = input.Id,
                StepId = input.StepId,
                StepName = stepName,
                Title = "退回审核"
            }, input.ReturnAuditSmmary);

            await ReturnProjectInWorkFlow(input.Id, AuditFlowId.ToGuid());

            var noticeService =
                Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>()
                    .IocManager.IocContainer.Resolve<IProjectNoticeAppService>();
            var noticeInput = new NoticePublishInputForWorkSpaceInput();
            noticeInput.ProjectId = input.Id;
            noticeInput.UserType = 1;
            noticeInput.Content = "完成退回审核,详情请在项目查询工作室查看";
            var projectName = from p in _singleProjectInfoRepository.GetAll()
                              where p.Id == input.Id
                              select p.SingleProjectName;
            if (projectName.Any())
            {
                noticeInput.Title = $"项目：{projectName.FirstOrDefault()} 退回审核";
                await noticeService.CreateProjectWorkSpaceNotice(noticeInput);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "获取项目数据异常");
            }
        }

        /// <summary>
        /// 查看退回信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetProjectAuditSmmaryOutput> GetReturnAuditSmmary(NullableIdDto<Guid> input)
        {
            var model = await _singleProjectInfoRepository.FirstOrDefaultAsync(x => x.Id == input.Id.Value && x.IsReturnAudit == true);
            var projectModel = await _projectBaseRepository.SingleAsync(r => r.Id == model.ProjectId);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = new GetProjectAuditSmmaryOutput();
            ret.ProjectName = projectModel.ProjectName;
            ret.SendTotalBudget = projectModel.SendTotalBudget;
            ret.ProjectCode = model.ProjectCode;
            ret.SingleProjectName = model.SingleProjectCode;
            ret.SingleProjectName = model.SingleProjectName;
            ret.Id = model.Id;
            ret.ReturnAuditSmmary = model.ReturnAuditSmmary;
            return ret;
        }

        /// <summary>
        /// 停滞申请专用   当项目没有指派项目经理时，返回true
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <returns></returns>
        [AbpAuthorize]
        public bool GetIsProjectLeader(Guid singleProjectId)
        {
            var query = from member in _projectAuditMemberRepository.GetAll()
                        join auditrole in _projectAuditRoleRepository.GetAll() on member.UserAuditRole equals auditrole.Id
                        where member.ProjectBaseId == singleProjectId && member.UserAuditRole == 1
                        select member;
            var members = query.FirstOrDefault();
            if (members == null) return true;
            if (AbpSession.UserId.HasValue && members.UserId == AbpSession.UserId.Value)
                return true;
            return false;
        }


        public bool GetIsCreateByProjecetLeader(Guid taskId)
        {
            var task = _workFlowTaskRepository.Get(taskId);
            var firstTask = _workFlowTaskRepository.FirstOrDefault(r => r.FlowID == task.FlowID && r.InstanceID == task.InstanceID && r.Sort == 1);
            var singleProjectId = task.InstanceID.ToGuid();
            var query = from member in _projectAuditMemberRepository.GetAll()
                        join auditrole in _projectAuditRoleRepository.GetAll() on member.UserAuditRole equals auditrole.Id
                        where member.ProjectBaseId == singleProjectId && member.UserAuditRole == 1
                        select member;
            var members = query.FirstOrDefault();
            if (members == null) return false;
            if (members.UserId == firstTask.ReceiveID)
                return true;
            else
                return false;
        }

        public async Task<List<CreateOrUpdateProjectFileInput>> GetProjectFiles(GetProjectFileInput input)
        {
            var projectFilemodel = from file in _projectFileRepository.GetAll()
                                   join appfile in _aappraisalFileTypeRepository.GetAll() on file.AappraisalFileType equals appfile.Id
                                   where file.SingleProjectId == input.SingleProjectId
                                   orderby appfile.Sort
                                   select new CreateOrUpdateProjectFileInput()
                                   {
                                       Id = file.Id,
                                       AappraisalFileType = appfile.Id,
                                       AappraisalFileTypeName = appfile.Name,
                                       FileName = file.FileName,
                                       HasUpload = file.HasUpload,
                                       IsPaperFile = file.IsPaperFile,
                                       ProjectBaseId = input.SingleProjectId,
                                       FilePath = file.FilePath,
                                       IsMust = file.IsMust,
                                       PaperFileNumber = file.PaperFileNumber,
                                   };
            projectFilemodel = projectFilemodel.WhereIf(!input.AappraisalFileType.IsNullOrWhiteSpace(), r => input.AappraisalFileType.Contains(r.AappraisalFileType.ToString()));
            var projectFilemodelList = projectFilemodel.ToList();

            foreach (var file in projectFilemodelList)
            {
                file.Files = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = file.Id.ToString(), BusinessType = (int)AbpFileBusinessType.送审资料 });

            }
            return projectFilemodelList;
        }


        public async Task<ValidateForPorjectResultOutput> ValilationProjectModel(
            Guid projectId)
        {
            var result = new ValidateForPorjectResultOutput();
            var projectbase = await _projectBaseRepository.GetAsync(projectId);
            var lackdata = new List<ValidateForPorjectResult>();
            var baseInfoValidate = _projectBaseRepository.GetValidateModelResult(projectbase);
            lackdata.AddRange(baseInfoValidate);
            if (lackdata.Count == 0)
                result.IsRight = true;
            else
            {
                result.IsRight = false;
                result.LackData = lackdata;
            }
            return result;
        }

        [AbpAuthorize]
        public void FollowProject(Guid input)
        {
            var project = _projectBaseRepository.Get(input);
            var isfollow =
                _userFollowProjectRepository.GetAll()
                    .Any(ite => ite.Projectid ==input && ite.Userid == AbpSession.UserId.Value);
            if (!isfollow)
            {
                var model = new UserFollowProject()
                {
                    Projectid = input,
                    Userid = AbpSession.UserId.Value,
                    ProjectCode = project.ProjectCode,
                };
                _userFollowProjectRepository.Insert(model);
            }
            else
            {
                _userFollowProjectRepository.Delete(ite => ite.ProjectCode == project.ProjectCode && ite.Userid == AbpSession.UserId.Value);
            }

        }

        protected virtual async Task ReturnProjectInWorkFlow(Guid instanceId, Guid flowId)
        {
            var maxSort =
                await _workFlowTaskRepository.GetAll()
                    .Where(r => r.FlowID == flowId && r.InstanceID == instanceId.ToString())
                    .MaxAsync(r => r.Sort);
            var tasks =
                _workFlowTaskRepository.GetAll()
                    .Where(r => r.FlowID == flowId && r.InstanceID == instanceId.ToString() && r.Sort == maxSort);
            foreach (var task in tasks)
            {
                task.Status = 8;
                task.CompletedTime1 = DateTime.Now;
                _workFlowTaskRepository.Update(task);

            }

        }




        protected virtual async Task UpdateProjectAsync(CreateOrUpdateProJectBudgetManagerInput input)
        {
            if (string.IsNullOrWhiteSpace(input.ProjectName))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "请填写项目名称");
            if (input.SendTime > DateTime.Now)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "送审时间不能超过当日");

            var inputmodel = new GetProjectBudgetForEditOutput(input);
            var db_projectBase = await _projectBaseRepository.GetAsync(input.Id.Value);
            inputmodel.BaseOutput.MapTo(db_projectBase);
            await _projectBaseRepository.UpdateAsync(db_projectBase);
            var db_ProjectModel = await GetProjectBudgetForEdit(new GetProjectForEditInput() { AppraisalTypeId = input.AppraisalTypeId, Id = input.Id.Value, });
            foreach (var item in db_ProjectModel.SingleProjectInfos)
            {
                if (input.SingleProjectInfos.Any(r => r.Id == item.Id))  //编辑
                {
                    var inputSingelInfo = input.SingleProjectInfos.FirstOrDefault(r => r.Id == item.Id);

                    var db_singleModel = _singleProjectInfoRepository.Get(item.Id);
                    db_singleModel.SingleProjectName = inputSingelInfo.SingleProjectName;
                    db_singleModel.SingleProjectCode = inputSingelInfo.SingleProjectCode;
                    db_singleModel.ProjectNature = inputSingelInfo.ProjectNature;
                    db_singleModel.Industry = inputSingelInfo.Industry;
                    db_singleModel.Industry_Name = inputSingelInfo.Industry_Name;
                    db_singleModel.SingleProjectbudget = inputSingelInfo.SingleProjectbudget;
                    db_singleModel.SingleProjectSafaBudget = inputSingelInfo.SingleProjectSafaBudget;
                    db_singleModel.ProjectCode = input.ProjectCode;


                    //费用
                    var FeeEntity = await _singleFeeRepository.SingleAsync(r => r.SingleProjectId == item.Id);
                    inputSingelInfo.SingleFee.MapTo(FeeEntity);
                    CalculateFee(ref FeeEntity);
                    await _singleFeeRepository.UpdateAsync(FeeEntity);
                    //控制信息
                    var add_projectBudgetControlModel = inputSingelInfo.ProjectBudgetControls.Where(r => r.Id == null).ToList();
                    add_projectBudgetControlModel.ForEach(r =>
                    {
                        var entity = r.MapTo<ProjectBudgetControl>();
                        entity.Pro_Id = input.Id.Value;
                        entity.SingleProjectId = item.Id;
                        _projectBudgetControlRepository.Insert(entity);
                    });
                    var less_projectBudgetControlIds =
                        item.ProjectBudgetControls.Select(r => r.Id)
                            .ToList()
                            .Except(
                                inputSingelInfo.ProjectBudgetControls.Where(r => r.Id.HasValue).Select(o => o.Id.Value).ToList())
                            .ToList();
                    less_projectBudgetControlIds.ForEach(r =>
                    {
                        _projectBudgetControlRepository.Delete(o => o.Id == r);
                    });
                    var update_projectBudgetControls =
                        inputSingelInfo.ProjectBudgetControls.Where(
                            r => r.Id != null && !less_projectBudgetControlIds.Contains(r.Id.Value)).ToList();
                    update_projectBudgetControls.ForEach(r =>
                    {
                        var db_projectBudgetControl = _projectBudgetControlRepository.FirstOrDefault(o => o.Id == r.Id.Value);
                        r.MapTo(db_projectBudgetControl);
                        _projectBudgetControlRepository.Update(db_projectBudgetControl);
                    });

                    //送审资料
                    foreach (var projectFile in item.ProjectFiles)
                    {
                        var db_ProjectFile = await _projectFileRepository.GetAsync(projectFile.Id.Value);
                        var newfile = inputSingelInfo.ProjectFiles.FirstOrDefault(r => r.Id == projectFile.Id);
                        if (newfile == null)
                        {
                            continue;
                        }
                        newfile.ProjectBaseId = input.Id.Value;
                        if (newfile.IsPaperFile || newfile.Files.Count > 0)
                        {
                            db_ProjectFile.HasUpload = true;
                        }
                        else
                            db_ProjectFile.HasUpload = false;

                        db_ProjectFile.IsPaperFile = newfile.IsPaperFile;
                        db_ProjectFile.PaperFileNumber = newfile.PaperFileNumber;
                        db_ProjectFile.Back = newfile.Back;
                        var fileList = new List<AbpFileListInput>();
                        foreach (var filemodel in newfile.Files)
                        {
                            var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                            fileList.Add(fileone);
                        }
                        _abpFileRelationAppService.Update(new CreateFileRelationsInput() { BusinessId = db_ProjectFile.Id.ToString(), BusinessType = (int)AbpFileBusinessType.送审资料, Files = fileList });
                        await _projectFileRepository.UpdateAsync(db_ProjectFile);
                    }

                }
                else   //删除
                {
                    await _singleFeeRepository.DeleteAsync(r => r.SingleProjectId == item.Id);
                    await _projectBudgetControlRepository.DeleteAsync(r => r.SingleProjectId == item.Id);
                    await _projectFileRepository.DeleteAsync(r => r.SingleProjectId == item.Id);
                    await _singleProjectInfoRepository.DeleteAsync(r => r.Id == item.Id);
                }

            }
            //新增
            foreach (var item in input.SingleProjectInfos.Where(r => !r.Id.HasValue))
            {
                var singleModel = item.MapTo<SingleProjectInfo>();
                singleModel.Id = Guid.NewGuid();
                singleModel.ProjectId = db_projectBase.Id;
                singleModel.ProjectCode = db_projectBase.ProjectCode;
                await _singleProjectInfoRepository.InsertAsync(singleModel);
                var singleFeeModel = item.SingleFee.MapTo<SingleProjectFee>();

                CalculateFee(ref singleFeeModel);
                singleFeeModel.Id = Guid.NewGuid();
                singleFeeModel.ProjectId = db_projectBase.Id;
                singleFeeModel.SingleProjectId = singleModel.Id;
                await _singleFeeRepository.InsertAsync(singleFeeModel);

                item.ProjectFiles.ForEach(r =>
                {
                    var entity = new ProjectFile();
                    r.MapTo(entity);
                    entity.Id = Guid.NewGuid();
                    entity.ProjectBaseId = singleModel.ProjectId;
                    entity.SingleProjectId = singleModel.Id;
                    if (r.IsPaperFile || r.Files.Count > 0)
                    {
                        r.HasUpload = true;
                    }
                    else
                        r.HasUpload = false;

                    var fileList = new List<AbpFileListInput>();
                    foreach (var filemodel in r.Files)
                    {
                        var fileone = new AbpFileListInput() { Id = filemodel.Id, Sort = filemodel.Sort };
                        fileList.Add(fileone);
                    }

                    _abpFileRelationAppService.Create(new CreateFileRelationsInput()
                    {
                        BusinessId = entity.Id.ToString(),
                        BusinessType = (int)AbpFileBusinessType.送审资料,
                        Files = fileList
                    });
                    _projectFileRepository.InsertAsync(entity);
                });


                var controls = item.ProjectBudgetControls.MapTo<List<ProjectBudgetControl>>();

                controls.ForEach(r =>
                {
                    r.Id = Guid.NewGuid();
                    r.Pro_Id = db_projectBase.Id;
                    r.SingleProjectId = singleModel.Id;
                    _projectBudgetControlRepository.InsertAsync(r);
                });
            }

        }

        protected void GetProjectFileByAppraisalTypeId(int appraisalTypeId, List<AappraisalFileType> data)
        {
            var query = from file in _aappraisalFileTypeRepository.GetAll()
                        where file.AppraisalTypeId == appraisalTypeId
                        select file;
            query.ToList().ForEach(r => data.Add(r));
            var model = _code_AppraisalTypeRepository.GetAll().FirstOrDefault(r => r.Id == appraisalTypeId);
            if (model == null) return;
            if (model.ParentId != 0)
                GetProjectFileByAppraisalTypeId(model.ParentId, data);
        }


        /// <summary>
        /// 项目负责人打开待办时  设置项目准备完成。
        /// </summary>
        /// <param name="input"></param>
        public void SetReadyEndTime(Guid input)
        {
            var project = _singleProjectInfoRepository.Get(input);
            if (project.ReadyEndTime.HasValue == false)
            {
                project.ReadyEndTime = DateTime.Now;
                _singleProjectInfoRepository.Update(project);
            }
        }
        /// <summary>
        /// 部门领导审核后 设置项目准备开始
        /// </summary>
        /// <param name="input"></param>
        public void SetReadyStartTime(Guid input)
        {
            var project = _singleProjectInfoRepository.Get(input);
            if (project.ReadyStartTime.HasValue == false)
            {
                project.ReadyStartTime = DateTime.Now;
                _singleProjectInfoRepository.Update(project);
            }
        }


        private ProjectChangeDto GetChangeModel(ProjectBase model)
        {
            var ret = new ProjectChangeDto()
            {
                ApprovalUnit_Name = "",
                CompetentUnit_Name = "",
                Industry_Name = "",
                ProjectNature1_Name = "",
                SendUnit_Name = "",
                UnitRoom_Name = "",
                //Controls = null,
                //ProjectFiles = null,
                #region
                //ACMF = model.ACMF,
                //AerialSurveyFee = model.AerialSurveyFee,
                //ApprovalNum = model.ApprovalNum,
                //AttorneyFee = model.AttorneyFee,
                //AuditFee = model.AuditFee,
                //BiddingFee = model.BiddingFee,
                //BuldManagerFee = model.BuldManagerFee,
                //ChannelMaintenanceFee = model.ChannelMaintenanceFee,
                //CombinedTestFee = model.CombinedTestFee,
                //ConstructionFee = model.ConstructionFee,
                Contacts = model.Contacts,
                ContactsTel = model.ContactsTel,
                //ContractNotarialFee = model.ContractNotarialFee,
                //CulturaRrelicsProtectionFee = model.CulturaRrelicsProtectionFee,
                Days = model.Days,
                Id = model.Id,
                //DesignFee = model.DesignFee,
                //DeviceFee = model.DeviceFee,
                //EngineeringInspectionFee = model.EngineeringInspectionFee,
                //EngineeringInsuranceFee = model.EngineeringInsuranceFee,
                EntrustmentNumber = model.EntrustmentNumber,
                //EnvironmentalImpactFee = model.EnvironmentalImpactFee,
                //EquipmentInspectionFee = model.EquipmentInspectionFee,
                //ExpropriationFee = model.ExpropriationFee,
                //FarmlandOccupationTax = model.FarmlandOccupationTax,
                //FeasibilityStudyFee = model.FeasibilityStudyFee,
                //ForestRrestorationFee = model.ForestRrestorationFee,
                //InstallFee = model.InstallFee,
                Is_Important = model.Is_Important,
                //LandAcquisitionFee = model.LandAcquisitionFee,
                //LandReclamationFee = model.LandReclamationFee,
                //LandUseTax = model.LandUseTax,
                //NavigationAidsFee = model.NavigationAidsFee,
                //NetworkRentalFee = model.NetworkRentalFee,
                //OtherFee = model.OtherFee,
                //OtherFee2 = model.OtherFee2,
                //OtherTotalFee = model.OtherTotalFee,
                //PreliminaryFee = model.PreliminaryFee,
                ProjectCode = model.ProjectCode,
                //ProjectDeviceFee = model.ProjectDeviceFee,
                ProjectName = model.ProjectName,
                Remark = model.Remark,
                //ResearchTestFee = model.ResearchTestFee,
                SafaBudget = model.SafaBudget,
                //SafetyAssessmentFee = model.SafetyAssessmentFee,
                //SafetyProductionFee = model.SafetyProductionFee,
                SendTime = model.SendTime,
                SendTotalBudget = model.SendTotalBudget,
                //StampDutyFee = model.StampDutyFee,
                //SupervisorFee = model.SupervisorFee,
                //SurveyFee = model.SurveyFee,
                //SystemoperationFee = model.SystemoperationFee,
                //TemporaryFacilitiesFee = model.TemporaryFacilitiesFee,
                //TotalFee = model.TotalFee,
                //UnforeseenFee = model.UnforeseenFee,
                //vehicleAndVesselTax = model.vehicleAndVesselTax,
                #endregion

            };

            if (!model.ApprovalUnit.IsNullOrWhiteSpace())
                ret.ApprovalUnit_Name = (_replyUnitProjectRepository.Get(model.ApprovalUnit.ToInt())).Name;
            if (model.CompetentUnit.HasValue)
                ret.CompetentUnit_Name = (_chargeOrganizations.Get(model.CompetentUnit.Value)).Name;
            if (!model.Industry.IsNullOrWhiteSpace())
                ret.Industry_Name = (_abpDictionary.Get(model.Industry.ToGuid())).Title;
            if (!model.ProjectNature1.IsNullOrWhiteSpace())
                ret.ProjectNature1_Name = model.ProjectNature1 == "1" ? "新建" : model.ProjectNature1 == "2" ? "改建" : "扩建";
            if (model.SendUnit != 0)
                ret.SendUnit_Name = (_constructionOrganizationsRepository.Get(model.SendUnit)).Name;
            if (!model.UnitRoom.IsNullOrWhiteSpace())
                ret.UnitRoom_Name = (_bdRepository.Get(model.UnitRoom.ToInt())).Name;
            return ret;

        }

        [RemoteService(IsEnabled = false)]
        public Dictionary<Guid, string> SingleProjectFlowActive(Guid projectId, long userId)
        {
            var userStr = "u_" + userId;
            var query = from a in _singleProjectInfoRepository.GetAll()
                        join b in _workFlowOrganizationUnitsRepository.GetAll() on a.DeparmentId.Replace("l_", "").Trim() equals b.Id.ToString()
                        where b.Leader.GetStrContainsArray(userStr) && a.ProjectId == projectId
                        select new { a.Id, a.SingleProjectName };
            if (query.Count() == 0)
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "单项实例错误");
            var ret = new Dictionary<Guid, string>();
            query.ToList().ForEach(r => ret.Add(r.Id, r.SingleProjectName));

            return ret;

        }
    }



}

