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
    public class OAContractAppService : FRMSCoreAppServiceBase, IOAContractAppService
    {
        private readonly IRepository<OAContract, Guid> _oAContractRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OACustomer, Guid> _oaCustomerRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OAContractAppService(IRepository<OAContract, Guid> oAContractRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , WorkFlowTaskManager workFlowTaskManager, IRepository<OACustomer, Guid> oaCustomerRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oAContractRepository = oAContractRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oaCustomerRepository = oaCustomerRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OAContractDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oAContract = await _oAContractRepository.GetAsync(id);
            var output = oAContract.MapTo<OAContractDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA合同附件 });
            var user = await UserManager.GetUserByIdAsync(output.SigningUser);
            output.SigningUser_Name = user.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OAContractListDto>> GetAll(GetOAContractListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oAContractRepository.GetAll()
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select m;

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oAContracts = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oAContractDtos = oAContracts.MapTo<List<OAContractListDto>>();
            foreach (var item in oAContractDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OAContractListDto>(count, oAContractDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OAContractCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oAContract = new OAContract();
            input.MapTo(oAContract);
            oAContract.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oAContract.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA合同附件,
                    Files = fileList
                });
            }
            var oACustomer = new OACustomer();
            oACustomer.Id = Guid.NewGuid();
            oACustomer.Name = input.UnitA;
            oACustomer.Address = input.UnitAContractAddress;
            oACustomer.Phone = input.UnitAContractTel;
            oACustomer.Contact = input.UnitAContract;
            oACustomer.OAContractId = oAContract.Id;
            await _oAContractRepository.InsertAsync(oAContract);
            await _oaCustomerRepository.InsertAsync(oACustomer);
            ret.InStanceId = oAContract.Id.ToString();
            return ret;
        }


        public async Task Update(OAContractUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oAContract = await _oAContractRepository.GetAsync(input.Id);
            input.MapTo(oAContract);

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
                BusinessType = (int)AbpFileBusinessType.OA合同附件,
                Files = fileList
            });



            await _oAContractRepository.UpdateAsync(oAContract);

            var customer = await _oaCustomerRepository.FirstOrDefaultAsync(r => r.OAContractId == input.Id);
            if (customer != null)
            {
                customer.Name = input.UnitA;
                customer.Phone = input.UnitAContractTel;
                customer.Address = input.UnitAContractAddress;
                customer.Contact = input.UnitAContract;
            }
            await _oaCustomerRepository.UpdateAsync(customer);
        }
    }
}

