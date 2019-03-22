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
    public class OAFixedAssetsPurchaseAppService : FRMSCoreAppServiceBase, IOAFixedAssetsPurchaseAppService
    {
        private readonly IRepository<OAFixedAssetsPurchase, Guid> _oAFixedAssetsPurchaseRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAFixedAssetsPurchaseAppService(IRepository<OAFixedAssetsPurchase, Guid> oAFixedAssetsPurchaseRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, WorkFlowTaskManager workFlowTaskManager
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAFixedAssetsPurchaseRepository = oAFixedAssetsPurchaseRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAFixedAssetsPurchaseDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAFixedAssetsPurchase = await _oAFixedAssetsPurchaseRepository.GetAsync(id);
            var output = oAFixedAssetsPurchase.MapTo<OAFixedAssetsPurchaseDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.固定资产采购申请附件 });
            var userGuidId = output.ApplyUserId.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUserId_Name = userModel.Name;
            var oAFixedAssetsEntities = await _oAFixedAssetsRepository.GetAll().Where(r => r.PurchaseId == id).ToListAsync();
            var models = oAFixedAssetsEntities.MapTo<List<OAFixedAssetsDto>>();
            output.FixedAssetss = models;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAFixedAssetsPurchaseListDto>> GetAll(GetOAFixedAssetsPurchaseListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAFixedAssetsPurchaseRepository.GetAll()
                        select m;

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            if (!input.ApplyTypeCode.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.ApplyTypeCode == input.ApplyTypeCode);
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAFixedAssetsPurchases = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAFixedAssetsPurchaseDtos = oAFixedAssetsPurchases.MapTo<List<OAFixedAssetsPurchaseListDto>>();
            foreach (var item in oAFixedAssetsPurchaseDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAFixedAssetsPurchaseListDto>(count, oAFixedAssetsPurchaseDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAFixedAssetsPurchaseCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAFixedAssetsPurchase = new OAFixedAssetsPurchase();
            input.MapTo(oAFixedAssetsPurchase);
            oAFixedAssetsPurchase.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAFixedAssetsPurchase.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固定资产采购申请附件,
                    Files = fileList
                });
            }
            await _oAFixedAssetsPurchaseRepository.InsertAsync(oAFixedAssetsPurchase);
            foreach (var item in input.FixedAssetss)
            {
                var entity = item.MapTo<OAFixedAssets>();
                entity.Id = Guid.NewGuid();
                entity.PurchaseId = oAFixedAssetsPurchase.Id;
                entity.PostingDate = DateTime.Now;
                entity.BuyDate = DateTime.Now;
                entity.DateOfManufacture = DateTime.Now;
                // entity.Status = (int)OAFixedAssetsStatus.采购;
                await _oAFixedAssetsRepository.InsertAsync(entity);

            }
            ret.InStanceId = oAFixedAssetsPurchase.Id.ToString();
            return ret;
        }


        public async Task Update(OAFixedAssetsPurchaseUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAFixedAssetsPurchase = await _oAFixedAssetsPurchaseRepository.GetAsync(input.Id);
            input.MapTo(oAFixedAssetsPurchase);
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
                BusinessType = (int)AbpFileBusinessType.固定资产采购申请附件,
                Files = fileList
            });

            await _oAFixedAssetsPurchaseRepository.UpdateAsync(oAFixedAssetsPurchase);
            var oAFixedAssets = _oAFixedAssetsRepository.GetAll().Where(r => r.PurchaseId == input.Id);
            await oAFixedAssets.ForEachAsync(r => _oAFixedAssetsRepository.Delete(r));
            foreach (var item in input.FixedAssetss)
            {
                var entity = item.MapTo<OAFixedAssets>();
                entity.Id = Guid.NewGuid();
                entity.PurchaseId = input.Id;
                entity.PostingDate = DateTime.Now;
                entity.BuyDate = DateTime.Now;
                entity.DateOfManufacture = DateTime.Now;
                await _oAFixedAssetsRepository.InsertAsync(entity);

            }
        }
    }
}

