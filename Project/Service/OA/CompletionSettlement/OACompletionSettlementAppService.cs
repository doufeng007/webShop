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
    public class OACompletionSettlementAppService : FRMSCoreAppServiceBase, IOACompletionSettlementAppService
    {
        private readonly IRepository<OACompletionSettlement, Guid> _oACompletionSettlementRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OAContract, Guid> _oaContractRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        public OACompletionSettlementAppService(IRepository<OACompletionSettlement, Guid> oACompletionSettlementRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OAContract, Guid> oaContractRepository, WorkFlowTaskManager workFlowTaskManager)
        {
            _oACompletionSettlementRepository = oACompletionSettlementRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _oaContractRepository = oaContractRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OACompletionSettlementDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oACompletionSettlement = await _oACompletionSettlementRepository.GetAsync(id);
            var output = oACompletionSettlement.MapTo<OACompletionSettlementDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA合同完工收款附件 });
            var userModel = await UserManager.GetUserByIdAsync(output.WriteUser);
            output.WriteUser_Name = userModel.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OACompletionSettlementListDto>> GetAll(GetOACompletionSettlementListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oACompletionSettlementRepository.GetAll()
                        join u in UserManager.Users on m.WriteUser equals u.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OACompletionSettlementListDto()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            ProjectName = m.ProjectName,
                            WriteUser = m.WriteUser,
                            WriteUserName = u.Name,
                            WriteData = m.WriteData,
                            ContractName = m.ContractName,
                            UnitA = m.UnitA,
                            SettlementAmount = m.SettlementAmount,
                            CreationTime = m.CreationTime,
                            Status = m.Status,
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oACompletionSettlements = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oACompletionSettlementDtos = oACompletionSettlements.MapTo<List<OACompletionSettlementListDto>>();
            foreach (var item in oACompletionSettlementDtos)
            {
                item.StatusTitle = _workFlowTaskManager.GetStatusTitle(input.FlowId, item.Status);
            }
            return new PagedResultDto<OACompletionSettlementListDto>(count, oACompletionSettlementDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OACompletionSettlementCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oACompletionSettlement = new OACompletionSettlement();
            input.MapTo(oACompletionSettlement);
            oACompletionSettlement.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oACompletionSettlement.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA合同完工收款附件,
                    Files = fileList
                });
            }
            await _oACompletionSettlementRepository.InsertAsync(oACompletionSettlement);
            ret.InStanceId = oACompletionSettlement.Id.ToString();
            return ret;
        }


        public async Task Update(OACompletionSettlementUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oACompletionSettlement = await _oACompletionSettlementRepository.GetAsync(input.Id);
            input.MapTo(oACompletionSettlement);

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
                BusinessType = (int)AbpFileBusinessType.OA合同完工收款附件,
                Files = fileList
            });

            await _oACompletionSettlementRepository.UpdateAsync(oACompletionSettlement);

        }
    }
}

