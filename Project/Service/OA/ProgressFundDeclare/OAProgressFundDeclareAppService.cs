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
    public class OAProgressFundDeclareAppService : FRMSCoreAppServiceBase, IOAProgressFundDeclareAppService
    {
        private readonly IRepository<OAProgressFundDeclare, Guid> _oAProgressFundDeclareRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAFixedAssets, Guid> _oAFixedAssetsRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OAFixedAssetsUseApply, Guid> _oAFixedAssetsUseApplyRepository;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAProgressFundDeclareAppService(IRepository<OAProgressFundDeclare, Guid> oAProgressFundDeclareRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAFixedAssets, Guid> oAFixedAssetsRepository, WorkFlowTaskManager workFlowTaskManager, IRepository<OAFixedAssetsUseApply, Guid> oAFixedAssetsUseApplyRepository
            , IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAProgressFundDeclareRepository = oAProgressFundDeclareRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oAFixedAssetsRepository = oAFixedAssetsRepository;
            _workFlowTaskManager = workFlowTaskManager;
            _oAFixedAssetsUseApplyRepository = oAFixedAssetsUseApplyRepository;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAProgressFundDeclareDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAProgressFundDeclare = await _oAProgressFundDeclareRepository.GetAsync(id);
            var output = oAProgressFundDeclare.MapTo<OAProgressFundDeclareDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA进度款申报附件 });
            var userModel = await UserManager.GetUserByIdAsync(output.WriteUser);
            output.WriteUser_Name = userModel.Name;
            if (oAProgressFundDeclare.ProjectId.HasValue)
            {
                var oabid = await _oABidProjectRepository.GetAsync(oAProgressFundDeclare.ProjectId.Value);
                output.ProjectId_Name = oabid.ProjectName;
            }
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAProgressFundDeclareListDto>> GetAll(GetOAProgressFundDeclareListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAProgressFundDeclareRepository.GetAll()
                        join u in UserManager.Users on m.WriteUser equals u.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OAProgressFundDeclareListDto()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            ProjectName = m.ProjectName,
                            WriteUser = m.WriteUser,
                            WriteUserName = u.Name,
                            WriteData = m.WriteData,
                            ContractName = m.ContractName,
                            UnitA = m.UnitA,
                            Amount = m.Amount,
                            CreationTime = m.CreationTime,
                            Status = m.Status,
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAProgressFundDeclares = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAProgressFundDeclareDtos = oAProgressFundDeclares.MapTo<List<OAProgressFundDeclareListDto>>();
            foreach (var item in oAProgressFundDeclareDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAProgressFundDeclareListDto>(count, oAProgressFundDeclareDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAProgressFundDeclareCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAProgressFundDeclare = new OAProgressFundDeclare();
            input.MapTo(oAProgressFundDeclare);
            oAProgressFundDeclare.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAProgressFundDeclare.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA进度款申报附件,
                    Files = fileList
                });
            }
            await _oAProgressFundDeclareRepository.InsertAsync(oAProgressFundDeclare);
            ret.InStanceId = oAProgressFundDeclare.Id.ToString();
            return ret;
        }


        public async Task Update(OAProgressFundDeclareUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAProgressFundDeclare = await _oAProgressFundDeclareRepository.GetAsync(input.Id);
            input.MapTo(oAProgressFundDeclare);
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
                BusinessType = (int)AbpFileBusinessType.OA进度款申报附件,
                Files = fileList
            });

            await _oAProgressFundDeclareRepository.UpdateAsync(oAProgressFundDeclare);

        }
    }
}

