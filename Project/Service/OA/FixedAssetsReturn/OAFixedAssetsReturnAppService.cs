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
    public class OAFixedAssetsReturnAppService : FRMSCoreAppServiceBase, IOAFixedAssetsReturnAppService
    {
        private readonly IRepository<OAFixedAssetsReturn, Guid> _oAFixedAssetsReturnRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OAFixedAssetsUseApply, Guid> _oAFixedAssetsUseApplyRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAFixedAssetsReturnAppService(IRepository<OAFixedAssetsReturn, Guid> oAFixedAssetsReturnRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, WorkFlowTaskManager workFlowTaskManager, IRepository<OAFixedAssetsUseApply, Guid> oAFixedAssetsUseApplyRepository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAFixedAssetsReturnRepository = oAFixedAssetsReturnRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _oAFixedAssetsUseApplyRepository = oAFixedAssetsUseApplyRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAFixedAssetsReturnDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAFixedAssetsReturn = await _oAFixedAssetsReturnRepository.GetAsync(id);
            var output = oAFixedAssetsReturn.MapTo<OAFixedAssetsReturnDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.固定资产归还申请附件 });
            var userGuidId = output.UserId.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.UserId_Name = userModel.Name;
            var oAFixedAssetsEntitie = await _oAFixedAssetsRepository.FirstOrDefaultAsync(r => r.Id == oAFixedAssetsReturn.FAId);
            output.FAId_Name = oAFixedAssetsEntitie.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAFixedAssetsReturnListDto>> GetAll(GetOAFixedAssetsReturnListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAFixedAssetsReturnRepository.GetAll()
                        join a in _oAFixedAssetsRepository.GetAll() on m.FAId equals a.Id
                        where m.Status > 0
                        select new OAFixedAssetsReturnListDto
                        {
                            Code = m.Code,
                            FAName = a.Name,
                            Id = m.Id,
                            Remark = m.Remark,
                            ReturnDate = m.ReturnDate,
                            Status = m.Status,
                            CreationTime = m.CreationTime
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAFixedAssetsReturns = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAFixedAssetsReturnDtos = oAFixedAssetsReturns.MapTo<List<OAFixedAssetsReturnListDto>>();
            foreach (var item in oAFixedAssetsReturnDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAFixedAssetsReturnListDto>(count, oAFixedAssetsReturnDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAFixedAssetsReturnCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAFixedAssetsReturn = new OAFixedAssetsReturn();
            input.MapTo(oAFixedAssetsReturn);
            oAFixedAssetsReturn.Id = Guid.NewGuid();
            var oaFxApplyModel = await _oAFixedAssetsUseApplyRepository.GetAsync(input.UseApplyId);
            oAFixedAssetsReturn.FAId = oaFxApplyModel.FAId;
            oAFixedAssetsReturn.FAName = oaFxApplyModel.FAName;
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAFixedAssetsReturn.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固定资产归还申请附件,
                    Files = fileList
                });
            }
            await _oAFixedAssetsReturnRepository.InsertAsync(oAFixedAssetsReturn);
            ret.InStanceId = oAFixedAssetsReturn.Id.ToString();
            return ret;
        }


        public async Task Update(OAFixedAssetsReturnUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAFixedAssetsReturn = await _oAFixedAssetsReturnRepository.GetAsync(input.Id);
            input.MapTo(oAFixedAssetsReturn);
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
                BusinessType = (int)AbpFileBusinessType.固定资产归还申请附件,
                Files = fileList
            });

            await _oAFixedAssetsReturnRepository.UpdateAsync(oAFixedAssetsReturn);

        }
    }
}

