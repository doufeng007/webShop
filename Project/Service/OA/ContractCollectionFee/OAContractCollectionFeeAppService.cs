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
    public class OAContractCollectionFeeAppService : FRMSCoreAppServiceBase, IOAContractCollectionFeeAppService
    {
        private readonly IRepository<OAContractCollectionFee, Guid> _oAContractCollectionFeeRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OAContract, Guid> _oaContractRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAContractCollectionFeeAppService(IRepository<OAContractCollectionFee, Guid> oAContractCollectionFeeRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , WorkFlowTaskManager workFlowTaskManager, IRepository<OAContract, Guid> oaContractRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAContractCollectionFeeRepository = oAContractCollectionFeeRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oaContractRepository = oaContractRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAContractCollectionFeeDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAContractCollectionFee = await _oAContractCollectionFeeRepository.GetAsync(id);
            var output = oAContractCollectionFee.MapTo<OAContractCollectionFeeDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA合同收款附件 });
            var user = await UserManager.GetUserByIdAsync(output.WriteUser);
            output.WriteUser_Name = user.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAContractCollectionFeeListDto>> GetAll(GetOAContractCollectionFeeListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAContractCollectionFeeRepository.GetAll()
                        join u in UserManager.Users on m.WriteUser equals u.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OAContractCollectionFeeListDto()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            ProjectName = m.ProjectName,
                            WriteUser = m.WriteUser,
                            WriteUserName = u.Name,
                            WriteData = m.WriteData,
                            ContractName = m.ContractName,
                            UnitA = m.UnitA,
                            CollectionAmount = m.CollectionAmount,
                            CreationTime = m.CreationTime,
                            Status = m.Status,
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Name.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAContractCollectionFees = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAContractCollectionFeeDtos = oAContractCollectionFees.MapTo<List<OAContractCollectionFeeListDto>>();
         
            foreach (var item in oAContractCollectionFeeDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAContractCollectionFeeListDto>(count, oAContractCollectionFeeDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAContractCollectionFeeCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAContractCollectionFee = new OAContractCollectionFee();
            input.MapTo(oAContractCollectionFee);
            oAContractCollectionFee.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAContractCollectionFee.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA合同收款附件,
                    Files = fileList
                });
            }
            await _oAContractCollectionFeeRepository.InsertAsync(oAContractCollectionFee);
            ret.InStanceId = oAContractCollectionFee.Id.ToString();
            return ret;
        }


        public async Task Update(OAContractCollectionFeeUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAContractCollectionFee = await _oAContractCollectionFeeRepository.GetAsync(input.Id);
            input.MapTo(oAContractCollectionFee);

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
                BusinessType = (int)AbpFileBusinessType.OA合同收款附件,
                Files = fileList
            });

            await _oAContractCollectionFeeRepository.UpdateAsync(oAContractCollectionFee);
        }
    }
}

