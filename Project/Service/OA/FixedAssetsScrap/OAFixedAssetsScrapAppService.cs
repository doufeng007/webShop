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
    public class OAFixedAssetsScrapAppService : FRMSCoreAppServiceBase, IOAFixedAssetsScrapAppService
    {
        private readonly IRepository<OAFixedAssetsScrap, Guid> _oAFixedAssetsScrapRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OAFixedAssetsUseApply, Guid> _oAFixedAssetsUseApplyRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAFixedAssetsScrapAppService(IRepository<OAFixedAssetsScrap, Guid> oAFixedAssetsScrapRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, WorkFlowTaskManager workFlowTaskManager, IRepository<OAFixedAssetsUseApply, Guid> oAFixedAssetsUseApplyRepository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAFixedAssetsScrapRepository = oAFixedAssetsScrapRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _oAFixedAssetsUseApplyRepository = oAFixedAssetsUseApplyRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAFixedAssetsScrapDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAFixedAssetsScrap = await _oAFixedAssetsScrapRepository.GetAsync(id);
            var output = oAFixedAssetsScrap.MapTo<OAFixedAssetsScrapDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.固定资产报废申请附件 });
            var userGuidId = output.ApplyUserId.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUserId_Name = userModel.Name;
            var oAFixedAssetsEntitie = await _oAFixedAssetsRepository.FirstOrDefaultAsync(r => r.Id == oAFixedAssetsScrap.FAId);
            output.FAId_Name = oAFixedAssetsEntitie.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAFixedAssetsScrapListDto>> GetAll(GetOAFixedAssetsScrapListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAFixedAssetsScrapRepository.GetAll()
                        join a in _oAFixedAssetsRepository.GetAll() on m.FAId equals a.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OAFixedAssetsScrapListDto
                        {
                            ApplyDate = m.ApplyDate,
                            Code = m.Code,
                            FAName = a.Name,
                            Id = a.Id,
                            Remark = m.Remark,
                            Status = m.Status,
                            CreationTime = m.CreationTime,

                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAFixedAssetsScraps = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAFixedAssetsScrapDtos = oAFixedAssetsScraps.MapTo<List<OAFixedAssetsScrapListDto>>();
            foreach (var item in oAFixedAssetsScrapDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAFixedAssetsScrapListDto>(count, oAFixedAssetsScrapDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAFixedAssetsScrapCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAFixedAssetsScrap = new OAFixedAssetsScrap();
            input.MapTo(oAFixedAssetsScrap);
            oAFixedAssetsScrap.Id = Guid.NewGuid();
            //var oaFxApplyModel = await _oAFixedAssetsUseApplyRepository.GetAsync(input.ApplyUserId);
            //oAFixedAssetsScrap.FAId = oaFxApplyModel.FAId;
            //oAFixedAssetsScrap.FAName = oaFxApplyModel.FAName;
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAFixedAssetsScrap.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固定资产报废申请附件,
                    Files = fileList
                });
            }
            await _oAFixedAssetsScrapRepository.InsertAsync(oAFixedAssetsScrap);
            ret.InStanceId = oAFixedAssetsScrap.Id.ToString();
            return ret;
        }


        public async Task Update(OAFixedAssetsScrapUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAFixedAssetsScrap = await _oAFixedAssetsScrapRepository.GetAsync(input.Id);
            input.MapTo(oAFixedAssetsScrap);
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
                BusinessType = (int)AbpFileBusinessType.固定资产报废申请附件,
                Files = fileList
            });

            await _oAFixedAssetsScrapRepository.UpdateAsync(oAFixedAssetsScrap);

        }
    }
}

