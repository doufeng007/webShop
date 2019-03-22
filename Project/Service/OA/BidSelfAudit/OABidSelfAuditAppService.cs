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
    public class OABidSelfAuditAppService : FRMSCoreAppServiceBase, IOABidSelfAuditAppService
    {
        private readonly IRepository<OABidSelfAudit, Guid> _oABidSelfAuditRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;

        public OABidSelfAuditAppService(IRepository<OABidSelfAudit, Guid> oABidSelfAuditRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , WorkFlowTaskManager workFlowTaskManager, IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oABidSelfAuditRepository = oABidSelfAuditRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OABidSelfAuditDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oABidSelfAudit = await _oABidSelfAuditRepository.GetAsync(id);
            var output = oABidSelfAudit.MapTo<OABidSelfAuditDto>();
            var oabid = await _oABidProjectRepository.GetAsync(oABidSelfAudit.ProjectId);
            output.ProjectId_Name = oabid.ProjectName;
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA投标资格自审申请附件 });
            var userGuidId = output.ApplyUser.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUser_Name = userModel.Name;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OABidSelfAuditListDto>> GetAll(GetOABidSelfAuditListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oABidSelfAuditRepository.GetAll()
                        join p in _oABidProjectRepository.GetAll() on m.ProjectId equals p.Id into g
                        from pro in g.DefaultIfEmpty()
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OABidSelfAuditListDto()
                        {
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
            var oABidSelfAudits = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oABidSelfAuditDtos = oABidSelfAudits.MapTo<List<OABidSelfAuditListDto>>();
            foreach (var item in oABidSelfAuditDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OABidSelfAuditListDto>(count, oABidSelfAuditDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OABidSelfAuditCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oABidSelfAudit = new OABidSelfAudit();
            input.MapTo(oABidSelfAudit);
            oABidSelfAudit.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oABidSelfAudit.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA投标资格自审申请附件,
                    Files = fileList
                });
            }
            await _oABidSelfAuditRepository.InsertAsync(oABidSelfAudit);
            ret.InStanceId = oABidSelfAudit.Id.ToString();
            return ret;
        }


        public async Task Update(OABidSelfAuditUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oABidSelfAudit = await _oABidSelfAuditRepository.GetAsync(input.Id);
            input.MapTo(oABidSelfAudit);

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
                BusinessType = (int)AbpFileBusinessType.OA投标资格自审申请附件,
                Files = fileList
            });
            await _oABidSelfAuditRepository.UpdateAsync(oABidSelfAudit);

        }
    }
}

