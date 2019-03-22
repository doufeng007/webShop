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
    public class OATenderBuessAppService : FRMSCoreAppServiceBase, IOATenderBuessAppService
    {
        private readonly IRepository<OATenderBuess, Guid> _oATenderBuessRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizeRepository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;

        public OATenderBuessAppService(IRepository<OATenderBuess, Guid> oATenderBuessRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizeRepository, IAbpFileRelationAppService abpFileRelationAppService
            , WorkFlowTaskManager workFlowTaskManager, IRepository<OABidProject, Guid> oABidProjectRepository
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _oATenderBuessRepository = oATenderBuessRepository;
            _organizeRepository = organizeRepository;
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowTaskManager = workFlowTaskManager;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowCommitFilterAttribute))]
        public async Task<OATenderBuessDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var oATenderBuess = await _oATenderBuessRepository.GetAsync(id);
            var output = oATenderBuess.MapTo<OATenderBuessDto>();
            output.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA投标费用附件 });
            //var userGuidId = output.ApplyUser.ToLong();
            //var userModel = await UserManager.GetUserByIdAsync(userGuidId);
            //output.ApplyUserName = userModel.Name;
            var projectModel = await _oABidProjectRepository.GetAsync(output.ProjectId);
            output.ProjectId_Name = projectModel.ProjectName;
            return output;
        }



        [AbpAuthorize]
        public async Task<PagedResultDto<OATenderBuessListDto>> GetAll(GetOATenderBuessListInput input)
        {
            var currentUserId = AbpSession.UserId.Value;
            var query = from m in _oATenderBuessRepository.GetAll()
                        join p in _oABidProjectRepository.GetAll() on m.ProjectId equals p.Id
                        where m.CreatorUserId == AbpSession.UserId.Value
                        select new OATenderBuessListDto
                        {
                            Id = m.Id,
                            Code = m.Code,
                            //cre = m.ApplyDate,
                            ProjectCode = p.ProjectCode,
                            ProjectName = p.ProjectName,
                            Status = m.Status,
                            CashPrice = m.CashPrice,
                            CreationTime = m.CreationTime
                        };

            if (!input.SearchKey.IsNullOrWhiteSpace())
            {
                query = query.Where(r => r.Code.Contains(input.SearchKey));
            }
            query = query.WhereIf(!input.Status.IsNullOrWhiteSpace(), r => input.Status.Contains(r.Status.ToString()));
            var count = await query.CountAsync();
            var oATenderBuesss = await query
             .OrderBy(input.Sorting)
             .PageBy(input)
             .ToListAsync();
            var oATenderBuessDtos = oATenderBuesss.MapTo<List<OATenderBuessListDto>>();
            foreach (var item in oATenderBuessDtos)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OATenderBuessListDto>(count, oATenderBuessDtos);
        }


        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OATenderBuessCreateInput input)
        {
            var ret = new InitWorkFlowOutput();
            var oATenderBuess = new OATenderBuess();
            input.MapTo(oATenderBuess);
            oATenderBuess.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = oATenderBuess.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA投标费用附件,
                    Files = fileList
                });
            }
            await _oATenderBuessRepository.InsertAsync(oATenderBuess);
            ret.InStanceId = oATenderBuess.Id.ToString();
            return ret;
        }


        public async Task Update(OATenderBuessUpdateInput input)
        {
            Debug.Assert(input.Id != null, "input Id should be set.");

            var oATenderBuess = await _oATenderBuessRepository.GetAsync(input.Id);
            input.MapTo(oATenderBuess);
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
                BusinessType = (int)AbpFileBusinessType.OA投标费用附件,
                Files = fileList
            });

            await _oATenderBuessRepository.UpdateAsync(oATenderBuess);

        }
    }
}

