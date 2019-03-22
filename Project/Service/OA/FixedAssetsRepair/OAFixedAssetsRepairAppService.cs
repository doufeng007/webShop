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
    public class OAFixedAssetsRepairAppService : FRMSCoreAppServiceBase, IOAFixedAssetsRepairAppService
    {
        private readonly IRepository<OAFixedAssetsRepair, Guid> _oAFixedAssetsRepairRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAFixedAssetsRepairAppService(IRepository<OAFixedAssetsRepair, Guid> oAFixedAssetsRepairRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAFixedAssetsRepairRepository = oAFixedAssetsRepairRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAFixedAssetsRepairDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAFixedAssetsRepair = await _oAFixedAssetsRepairRepository.GetAsync(id);
            var output = oAFixedAssetsRepair.MapTo<OAFixedAssetsRepairDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.固定资产维修申请附件 });
            var userGuidId = output.ApplyUserId.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUserId_Name = userModel.Name;
            var oAFixedAssetsEntitie = await _oAFixedAssetsRepository.FirstOrDefaultAsync(r => r.Id == oAFixedAssetsRepair.FAId);
            output.FAId_Name = oAFixedAssetsEntitie.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAFixedAssetsRepairListDto>> GetAll(GetOAFixedAssetsRepairListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAFixedAssetsRepairRepository.GetAll()
                        join a in _oAFixedAssetsRepository.GetAll() on m.FAId equals a.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OAFixedAssetsRepairListDto
                        {
                            ApplyDate = m.ApplyDate,
                            Code = m.Code,
                            FAName = a.Name,
                            Id = m.Id,
                            Reason = m.Reason,
                            Status = m.Status,
                            CreationTime = m.CreationTime
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAFixedAssetsRepairs = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAFixedAssetsRepairDtos = oAFixedAssetsRepairs.MapTo<List<OAFixedAssetsRepairListDto>>();
            foreach (var item in oAFixedAssetsRepairDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAFixedAssetsRepairListDto>(count, oAFixedAssetsRepairDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAFixedAssetsRepairCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAFixedAssetsRepair = new OAFixedAssetsRepair();
            input.MapTo(oAFixedAssetsRepair);
            oAFixedAssetsRepair.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAFixedAssetsRepair.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固定资产维修申请附件,
                    Files = fileList
                });
            }
            await _oAFixedAssetsRepairRepository.InsertAsync(oAFixedAssetsRepair);
            ret.InStanceId = oAFixedAssetsRepair.Id.ToString();
            return ret;
        }


        public async Task Update(OAFixedAssetsRepairUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");
            var oAFixedAssetsRepair = await _oAFixedAssetsRepairRepository.GetAsync(input.Id);
            input.MapTo(oAFixedAssetsRepair);
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
                BusinessType = (int)AbpFileBusinessType.固定资产维修申请附件,
                Files = fileList
            });
            await _oAFixedAssetsRepairRepository.UpdateAsync(oAFixedAssetsRepair);
        }

        public async Task BeginRepairOAFA(Guid fAId)
        {
            var model = await _oAFixedAssetsRepairRepository.GetAsync(fAId);
            model.Status = (int)OAFixedAssetsRepairStatus.维修开始;
            var fAModel = await _oAFixedAssetsRepository.GetAsync(model.FAId);
            fAModel.Status = (int)OAFixedAssetsStatus.维修;
            await _oAFixedAssetsRepairRepository.UpdateAsync(model);
            await _oAFixedAssetsRepository.UpdateAsync(fAModel);
        }
    }
}

