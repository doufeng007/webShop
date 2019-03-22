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
    public class OABidProjectAppService : FRMSCoreAppServiceBase, IOABidProjectAppService
    {
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OABidFilePurchase, Guid> _oABidFilePurchaseRepository;
        private readonly IRepository<OABidSelfAudit, Guid> _oABidSelfAuditRepository;
        private readonly IRepository<OABidProjectCheck, Guid> _oABidProjectCheckRepository;
        private readonly IRepository<OATenderBuess, Guid> _oATenderBuessRepository;
        private readonly IRepository<OATenderAudit, Guid> _oATenderAuditRepository;
        private readonly IRepository<OATenderCash, Guid> _oATenderCashRepository;
        private readonly IRepository<OATenderEnemy, Guid> _oATenderEnemyRepository;

        public OABidProjectAppService(IRepository<OABidProject, Guid> oABidProjectRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService, WorkFlowTaskManager workFlowTaskManager
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, IRepository<OABidFilePurchase, Guid> oABidFilePurchaseRepository, IRepository<OABidSelfAudit, Guid> oABidSelfAuditRepository, IRepository<OABidProjectCheck, Guid> oABidProjectCheckRepository
            , IRepository<OATenderBuess, Guid> oATenderBuessRepository, IRepository<OATenderAudit, Guid> oATenderAuditRepository,
            IRepository<OATenderCash, Guid> oATenderCashRepository, IRepository<OATenderEnemy, Guid> oATenderEnemyRepository)
        {
            _oABidProjectRepository = oABidProjectRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oABidFilePurchaseRepository = oABidFilePurchaseRepository;
            _oABidSelfAuditRepository = oABidSelfAuditRepository;
            _oABidProjectCheckRepository = oABidProjectCheckRepository;
            _oATenderBuessRepository = oATenderBuessRepository;
            _oATenderAuditRepository = oATenderAuditRepository;
            _oATenderCashRepository = oATenderCashRepository;
            _oATenderEnemyRepository = oATenderEnemyRepository;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OABidProjectDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oABidProject = await _oABidProjectRepository.GetAsync(id);
            var output = oABidProject.MapTo<OABidProjectDto>();
            var usermodel = await UserManager.GetUserByIdAsync(output.WriteUser.ToLong());
            output.WriteUser_Name = usermodel.Name;
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA投标项目附件 });
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OABidProjectListDto>> GetAll(GetOABidProjectListInput input)
        {

            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oABidProjectRepository.GetAll()
                        join bidder in UserManager.Users on m.Bidder equals bidder.Id.ToString()
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OABidProjectListDto
                        {
                            Id = m.Id,
                            BidderName = bidder.Name,
                            ProjectCode = m.ProjectCode,
                            ProjectName = m.ProjectName,
                            BuildUnit = m.BuildUnit,
                            Status = m.Status,
                            CreationTime = m.CreationTime,
                            WriteDate = m.WriteDate,

                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.ProjectCode.Contains(input.SearchKey) || r.ProjectName.Contains(input.SearchKey));

            }




            var count = await query.CountAsync();
            var oABidProjects = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            foreach (var m in oABidProjects)
            {
                m.StatusTitle = m.Status == 0 ? "新增" : m.Status == 2 ? "正在购买招标文件" : m.Status == 3 ? "购买招标文件完成" : m.Status == 4 ? "正在资格自审" : m.Status == 5 ? "资格自审完成"
                : m.Status == 6 ? "正在项目勘察" : m.Status == 7 ? "项目勘察完成" : m.Status == 8 ? "投标文件审查" : m.Status == 9 ? "投标文件审查完成" : m.Status == 10 ? "投标保证金申请" : m.Status == 11 ? "投标保证金申请完成"
                : m.Status == 12 ? "分析竞争对手" : m.Status == 13 ? "分析竞争对手完成" : m.Status == 14 ? "申请项目业务费用" : m.Status == 15 ? "项目业务费用" : "完成";
            }
            return new PagedResultDto<OABidProjectListDto>(count, oABidProjects);
        }


        //[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OABidProjectCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oABidProject = new OABidProject();
            input.MapTo(oABidProject);
            oABidProject.Id = Guid.NewGuid();

            var fileList = new List<AbpFileListInput>();
            if (input.FileList != null)
            {
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
            }
            await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
            {
                BusinessId = oABidProject.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.OA投标项目附件,
                Files = fileList
            });

            await _oABidProjectRepository.InsertAsync(oABidProject);
            ret.InStanceId = oABidProject.Id.ToString();
            return ret;
        }


        public async Task Update(OABidProjectUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oABidProject = await _oABidProjectRepository.GetAsync(input.Id);
            input.MapTo(oABidProject);
            var fileList = new List<AbpFileListInput>();
            foreach (var item in input.FileList)
            {
                fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
            }
            await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
            {
                BusinessId = input.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.OA投标项目附件,
                Files = fileList
            });

            await _oABidProjectRepository.UpdateAsync(oABidProject);

        }


        public void UpdateOABidProjectStatus(UpdateOABidProjectStatusInput input)
        {
            var ids = new List<Guid>();
            if (input.FromStatus == OABidProjectStatus.新增)
            {
                var entity = _oABidFilePurchaseRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {
                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = (int)OABidFilePurchaseStatus.正在购买招标文件;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.正在购买招标文件)
            {
                var entity = _oABidFilePurchaseRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {
                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = (int)OABidFilePurchaseStatus.购买招标文件完成;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.购买招标文件完成)
            {
                var entity = _oABidSelfAuditRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = (int)OABidSelfAuditStatus.正在资格自审;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.正在资格自审)
            {
                var entity = _oABidSelfAuditRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = (int)OABidSelfAuditStatus.资格自审完成;
                }
            }

            else if (input.FromStatus == OABidProjectStatus.资格自审完成)
            {
                var entity = _oABidProjectCheckRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = (int)OABidProjectCheckStatus.正在项目勘察;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.正在项目勘察)
            {
                var entity = _oABidProjectCheckRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = (int)OABidProjectCheckStatus.项目勘察完成;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.项目勘察完成)
            {
                var entity = _oATenderAuditRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 2;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.投标文件审查)
            {
                var entity = _oATenderAuditRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 1;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.投标文件审查完成)
            {
                var entity = _oATenderCashRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 2;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.投标保证金申请)
            {
                var entity = _oATenderCashRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 1;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.投标保证金申请完成)
            {
                var entity = _oATenderEnemyRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 2;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.分析竞争对手)
            {
                var entity = _oATenderEnemyRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 1;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.分析竞争对手完成)
            {
                var entity = _oATenderBuessRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 2;
                }
            }
            else if (input.FromStatus == OABidProjectStatus.申请项目业务费用)
            {
                var entity = _oATenderBuessRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_BidProject = _oABidProjectRepository.GetAll().Where(r => r.Id == entity.ProjectId);
                    foreach (var item in entity_BidProject)
                    {
                        ids.Add(item.Id);
                    }
                    entity.Status = 1;
                }
            }



            var items = _oABidProjectRepository.GetAll().Where(r => ids.Contains(r.Id));
            foreach (var item in items)
            {
                item.Status = (int)input.ToStatus;
                _oABidProjectRepository.Update(item);
            }
            CurrentUnitOfWork.SaveChanges();
        }
    }
}

