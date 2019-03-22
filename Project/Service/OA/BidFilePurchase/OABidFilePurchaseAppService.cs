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
    public class OABidFilePurchaseAppService : FRMSCoreAppServiceBase, IOABidFilePurchaseAppService
    {
        private readonly IRepository<OABidFilePurchase, Guid> _oABidFilePurchaseRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;

        public OABidFilePurchaseAppService(IRepository<OABidFilePurchase, Guid> oABidFilePurchaseRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowTaskManager workFlowTaskManager, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oABidFilePurchaseRepository = oABidFilePurchaseRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OABidFilePurchaseDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oABidFilePurchase = await _oABidFilePurchaseRepository.GetAsync(id);
            var output = oABidFilePurchase.MapTo<OABidFilePurchaseDto>();
            var oabid = await _oABidProjectRepository.GetAsync(oABidFilePurchase.ProjectId);
            output.ProjectId_Name = oabid.ProjectName;
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA招标文件购买申请附件 });
            var userGuidId = output.ApplyUser.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUser_Name = userModel.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OABidFilePurchaseListDto>> GetAll(GetOABidFilePurchaseListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oABidFilePurchaseRepository.GetAll()
                        join p in _oABidProjectRepository.GetAll() on m.ProjectId equals p.Id into g
                        from pro in g.DefaultIfEmpty()
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OABidFilePurchaseListDto()
                        {
                            Amount = m.Amount,
                            ApplyDate = m.ApplyDate,
                            Code = m.Code,
                            Id = m.Id,
                            ProjectCode = pro.ProjectCode,
                            ProjectName = pro.ProjectName,
                            Status = m.Status,
                            CreationTime = m.CreationTime

                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oABidFilePurchases = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oABidFilePurchaseDtos = oABidFilePurchases.MapTo<List<OABidFilePurchaseListDto>>();
            foreach (var item in oABidFilePurchaseDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OABidFilePurchaseListDto>(count, oABidFilePurchaseDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OABidFilePurchaseCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oABidFilePurchase = new OABidFilePurchase();
            input.MapTo(oABidFilePurchase);
            oABidFilePurchase.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oABidFilePurchase.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA招标文件购买申请附件,
                    Files = fileList
                });
            }

            await _oABidFilePurchaseRepository.InsertAsync(oABidFilePurchase);
            ret.InStanceId = oABidFilePurchase.Id.ToString();
            return ret;
        }


        public async Task Update(OABidFilePurchaseUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oABidFilePurchase = await _oABidFilePurchaseRepository.GetAsync(input.Id);
            input.MapTo(oABidFilePurchase);

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
                BusinessType = (int)AbpFileBusinessType.OA招标文件购买申请附件,
                Files = fileList
            });


            await _oABidFilePurchaseRepository.UpdateAsync(oABidFilePurchase);

        }
    }
}

