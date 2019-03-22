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
    public class OAFixedAssetsAppService : FRMSCoreAppServiceBase, IOAFixedAssetsAppService
    {
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly IRepository<OAFixedAssetsRepair, Guid> _oAFixedAssetsRepairRepository;
        private readonly IRepository<OAFixedAssetsScrap, Guid> _oAFixedAssetsScrapRepository;
        private readonly IRepository<OAFixedAssetsReturn, Guid> _oAFixedAssetsReturnRepository;
        private readonly IRepository<OAFixedAssetsUseApply, Guid> _oAFixedAssetsUseApplyRepository;
        private readonly IRepository<OAFixedAssetsPurchase, Guid> _oAFixedAssetsPurchaseRepository;

        public OAFixedAssetsAppService(IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService,
            IRepository<OAFixedAssetsUseApply, Guid> oAFixedAssetsUseApplyRepository, IRepository<OAFixedAssetsReturn, Guid> oAFixedAssetsReturnRepository
            , IRepository<OAFixedAssetsScrap, Guid> oAFixedAssetsScrapRepository, IRepository<OAFixedAssetsRepair, Guid> oAFixedAssetsRepairRepository,
            IRepository<OAFixedAssetsPurchase, Guid> oAFixedAssetsPurchaseRepository)
        {
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsUseApplyRepository = oAFixedAssetsUseApplyRepository;
            _oAFixedAssetsReturnRepository = oAFixedAssetsReturnRepository;
            _oAFixedAssetsScrapRepository = oAFixedAssetsScrapRepository;
            _oAFixedAssetsRepairRepository = oAFixedAssetsRepairRepository;
            _oAFixedAssetsPurchaseRepository = oAFixedAssetsPurchaseRepository;
        }

        //[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAFixedAssetsDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var entity = new OAFixedAssetsDto();
            var model = await _oAFixedAssetsRepository.GetAsync(id);
            model.MapTo(entity);
            entity.StatusTitle = ((OAFixedAssetsStatus)entity.Status).ToString();
            entity.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.固定资产附件 });
            return entity;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAFixedAssetsListDto>> GetAll(GetOAFixedAssetsListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAFixedAssetsRepository.GetAll()
                        where m.Status > 1
                        select m;

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey));

            }
            var count = await query.CountAsync();
            var oAFixedAssetss = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAFixedAssetsDtos = new List<OAFixedAssetsListDto>();
            var data = oAFixedAssetss.MapTo<List<OAFixedAssetsListDto>>();
            foreach (var item in data)
            {
                item.StatusTitle = ((OAFixedAssetsStatus)item.Status).ToString();
            }
            return new PagedResultDto<OAFixedAssetsListDto>(count, data);

        }


        //[Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAFixedAssetsCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAFixedAssets = new OAFixedAssets();
            input.MapTo(oAFixedAssets);
            var id = Guid.NewGuid();
            oAFixedAssets.Id = id;
            oAFixedAssets.Status = (int)OAFixedAssetsStatus.在库;
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固定资产附件,
                    Files = fileList
                });
            }
            await _oAFixedAssetsRepository.InsertAsync(oAFixedAssets);
            ret.InStanceId = id.ToString();
            return ret;

        }


        public async Task Update(OAFixedAssetsUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAFixedAssets = await _oAFixedAssetsRepository.GetAsync(input.Id.Value);
            input.MapTo(oAFixedAssets);
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
                BusinessType = (int)AbpFileBusinessType.固定资产附件,
                Files = fileList
            });
            await _oAFixedAssetsRepository.UpdateAsync(oAFixedAssets);
        }

        public void UpdateOAFixedAssetsStatusAsync(UpdateOAFixedAssetsStatusInput input)
        {
            var ids = new List<Guid>();
            if (input.Status == OAFixedAssetsStatus.采购)
            {
                var entity = _oAFixedAssetsPurchaseRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {
                    var entity_FA = _oAFixedAssetsRepository.GetAll().Where(r => r.PurchaseId == input.BusinessId);
                    foreach (var item in entity_FA)
                    {
                        ids.Add(item.Id);
                    }
                }
            }
            if (input.Status == OAFixedAssetsStatus.领用申请)
            {
                var entity = _oAFixedAssetsUseApplyRepository.FirstOrDefault(input.BusinessId);
                if (entity != null)
                {

                    var entity_FA = _oAFixedAssetsRepository.GetAll().Where(r => r.Id == entity.FAId);
                    foreach (var item in entity_FA)
                    {
                        ids.Add(item.Id);
                    }
                }
            }

            if (input.Status == OAFixedAssetsStatus.归还申请)
            {
                var entity = _oAFixedAssetsReturnRepository.FirstOrDefault(input.BusinessId);

                if (entity != null)
                {
                    var entity_Apply = _oAFixedAssetsUseApplyRepository.GetAll().FirstOrDefault(r => r.Id == entity.UseApplyId);
                    if (entity_Apply != null)
                    {
                        entity_Apply.Status = 2;
                        _oAFixedAssetsUseApplyRepository.Update(entity_Apply);
                    }
                    var entity_FA = _oAFixedAssetsRepository.GetAll().Where(r => r.Id == entity.FAId);
                    foreach (var item in entity_FA)
                    {
                        ids.Add(item.Id);
                    }
                }
            }

            if (input.Status == OAFixedAssetsStatus.报废申请)
            {
                var entity = _oAFixedAssetsScrapRepository.FirstOrDefault(input.BusinessId);

                if (entity != null)
                {
                    var entity_FA = _oAFixedAssetsRepository.GetAll().Where(r => r.Id == entity.FAId);
                    foreach (var item in entity_FA)
                    {
                        ids.Add(item.Id);
                    }
                }
            }
            if (input.Status == OAFixedAssetsStatus.维修申请)
            {
                var entity = _oAFixedAssetsRepairRepository.FirstOrDefault(input.BusinessId);

                if (entity != null)
                {
                    var entity_FA = _oAFixedAssetsRepository.GetAll().Where(r => r.Id == entity.FAId);
                    foreach (var item in entity_FA)
                    {
                        ids.Add(item.Id);
                    }
                }
            }
            var items = _oAFixedAssetsRepository.GetAll().Where(r => ids.Contains(r.Id));
            foreach (var item in items)
            {
                item.Status = (int)input.ToStatus;
                _oAFixedAssetsRepository.Update(item);
            }
            CurrentUnitOfWork.SaveChanges();
        }
    }
}

