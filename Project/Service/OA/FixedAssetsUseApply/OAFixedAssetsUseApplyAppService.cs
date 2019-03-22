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
    public class OAFixedAssetsUseApplyAppService : FRMSCoreAppServiceBase, IOAFixedAssetsUseApplyAppService
    {
        private readonly IRepository<OAFixedAssetsUseApply, Guid> _oAFixedAssetsUseApplyRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAFixedAssetsUseApplyAppService(IRepository<OAFixedAssetsUseApply, Guid> oAFixedAssetsUseApplyRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAFixedAssetsUseApplyRepository = oAFixedAssetsUseApplyRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAFixedAssetsUseApplyDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAFixedAssetsUseApply = await _oAFixedAssetsUseApplyRepository.GetAsync(id);
            var output = oAFixedAssetsUseApply.MapTo<OAFixedAssetsUseApplyDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.固定资产领用申请附件 });
            var userGuidId = output.ApplyUserId.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUserId_Name = userModel.Name;
            var oAFixedAssetsEntities = await _oAFixedAssetsRepository.GetAll().Where(r => r.PurchaseId == id).ToListAsync();
            var models = oAFixedAssetsEntities.MapTo<List<OAFixedAssetsDto>>();
            var oAFixedAssetsEntitie = await _oAFixedAssetsRepository.FirstOrDefaultAsync(r => r.Id == oAFixedAssetsUseApply.FAId);
            output.FAId_Name = oAFixedAssetsEntitie.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAFixedAssetsUseApplyListDto>> GetAll(GetOAFixedAssetsUseApplyListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAFixedAssetsUseApplyRepository.GetAll()
                        join a in _oAFixedAssetsRepository.GetAll() on m.FAId equals a.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OAFixedAssetsUseApplyListDto
                        {
                            ApplyDate = m.ApplyDate,
                            Code = m.Code,
                            FAName = a.Name,
                            Id = m.Id,
                            Reason = m.Reason,
                            Status = m.Status,
                            CreationTime = m.CreationTime,
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }

            var searchIds = new List<int>();
            //if (!input.Status.IsNullOrWhiteSpace())
            //{
            //    if (input.Status.Contains("1"))
            //        searchIds.Add((int));
            //    if (input.Status.Contains("2"))
            //        searchIds.Add(1);
            //    if (input.Status.Contains("3"))
            //        searchIds.Add(2);
            //    if (searchIds.Count > 0)
            //        query = query.Where(r => searchIds.Contains(r.Status));
            //}
            var count = await query.CountAsync();
            var oAFixedAssetsUseApplys = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAFixedAssetsUseApplyDtos = oAFixedAssetsUseApplys.MapTo<List<OAFixedAssetsUseApplyListDto>>();
            foreach (var item in oAFixedAssetsUseApplyDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAFixedAssetsUseApplyListDto>(count, oAFixedAssetsUseApplyDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAFixedAssetsUseApplyCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAFixedAssetsUseApply = new OAFixedAssetsUseApply();
            input.MapTo(oAFixedAssetsUseApply);
            oAFixedAssetsUseApply.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAFixedAssetsUseApply.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.固定资产维修申请附件,
                    Files = fileList
                });
            }
            await _oAFixedAssetsUseApplyRepository.InsertAsync(oAFixedAssetsUseApply);
            ret.InStanceId = oAFixedAssetsUseApply.Id.ToString();
            return ret;
        }


        public async Task Update(OAFixedAssetsUseApplyUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");
            var oAFixedAssetsUseApply = await _oAFixedAssetsUseApplyRepository.GetAsync(input.Id);
            input.MapTo(oAFixedAssetsUseApply);
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
                BusinessType = (int)AbpFileBusinessType.固定资产领用申请附件,
                Files = fileList
            });
            await _oAFixedAssetsUseApplyRepository.UpdateAsync(oAFixedAssetsUseApply);
        }
    }
}

