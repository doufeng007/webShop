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
    public class OABidProjectCheckAppService : FRMSCoreAppServiceBase, IOABidProjectCheckAppService
    {
        private readonly IRepository<OABidProjectCheck, Guid> _oABidProjectCheckRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly WorkFlowOrganizationUnitsManager _workFlowOrganizationUnitsManager;

        public OABidProjectCheckAppService(IRepository<OABidProjectCheck, Guid> oABidProjectCheckRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , WorkFlowTaskManager workFlowTaskManager, IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowBusinessTaskManager workFlowBusinessTaskManager
            , WorkFlowOrganizationUnitsManager workFlowOrganizationUnitsManager)
        {
            _oABidProjectCheckRepository = oABidProjectCheckRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowOrganizationUnitsManager = workFlowOrganizationUnitsManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OABidProjectCheckDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oABidProjectCheck = await _oABidProjectCheckRepository.GetAsync(id);
            var output = oABidProjectCheck.MapTo<OABidProjectCheckDto>();
            if (output.ProjectId.HasValue)
                output.ProjectId_Name = (await _oABidProjectRepository.GetAsync(output.ProjectId.Value)).ProjectName;
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA投标项目勘察申请附件 });
            var userGuidId = output.ApplyUser.ToLong();
            var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            output.ApplyUser_Name = userModel.Name;
            output.Participant_Name = _workFlowOrganizationUnitsManager.GetNames(output.Participant);
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OABidProjectCheckListDto>> GetAll(GetOABidProjectCheckListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oABidProjectCheckRepository.GetAll()
                        join p in _oABidProjectRepository.GetAll() on m.ProjectId equals p.Id into g
                        from pro in g.DefaultIfEmpty()
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OABidProjectCheckListDto
                        {
                            ApplyDate = m.ApplyDate,
                            Code = m.Code,
                            Id = m.Id,
                            ProjectCode = pro == null ? "" : pro.ProjectCode,
                            ProjectName = pro == null ? "" : pro.ProjectName,
                            Status = m.Status,
                            CreationTime = m.CreationTime
    };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oABidProjectChecks = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oABidProjectCheckDtos = oABidProjectChecks.MapTo<List<OABidProjectCheckListDto>>();
            foreach (var item in oABidProjectCheckDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OABidProjectCheckListDto>(count, oABidProjectCheckDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OABidProjectCheckCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oABidProjectCheck = new OABidProjectCheck();
            input.MapTo(oABidProjectCheck);
            oABidProjectCheck.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oABidProjectCheck.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA投标项目勘察申请附件,
                    Files = fileList
                });
            }
            await _oABidProjectCheckRepository.InsertAsync(oABidProjectCheck);
            ret.InStanceId = oABidProjectCheck.Id.ToString();
            return ret;
        }


        public async Task Update(OABidProjectCheckUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oABidProjectCheck = await _oABidProjectCheckRepository.GetAsync(input.Id);
            input.MapTo(oABidProjectCheck);

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
                BusinessType = (int)AbpFileBusinessType.OA投标项目勘察申请附件,
                Files = fileList
            });

            await _oABidProjectCheckRepository.UpdateAsync(oABidProjectCheck);

        }
    }
}

