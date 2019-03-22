using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;
using System.Linq;
using Abp.Linq.Extensions;
using Abp.Authorization;
using Abp.File;
using System.Threading.Tasks;

namespace Project.Service.OA.TenderAudit
{
    public class OATenderAuditAppService : ApplicationService, IOATenderAuditAppService
    {
        private readonly IRepository<OATenderAudit, Guid> _oaTenderAuditRepository;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OATenderAuditAppService(IRepository<OATenderAudit, Guid> oaTenderAuditRepository, IAbpFileRelationAppService abpFileRelationAppService, IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowTaskManager workFlowTaskManager
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _abpFileRelationAppService = abpFileRelationAppService;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _oaTenderAuditRepository = oaTenderAuditRepository;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task< InitWorkFlowOutput> Create(OATenderAuditInputDto input)
        {
            var model = input.MapTo<OATenderAudit>();
            model.Id = Guid.NewGuid();
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = model.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.OA投标文件审查附件,
                    Files = fileList
                });
            }
            model.Status = 0;

            _oaTenderAuditRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId=model.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task< OATenderAuditDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaTenderAuditRepository.Get(id);
            var model = ret.MapTo<OATenderAuditDto>();
            model.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA投标文件审查附件 });
            model.ProjectName = _oABidProjectRepository.Get(ret.ProjectId).ProjectName;
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OATenderAuditListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = from a in _oaTenderAuditRepository.GetAll()
                      join b in _oABidProjectRepository.GetAll() on a.ProjectId equals b.Id
                      orderby a.CreationTime descending
                      select new OATenderAuditListDto()
                      {
                          CreationTime=a.CreationTime,
                          AuditUser = a.AuditUser,
                          Builder = a.Builder,
                          Id = a.Id,
                          ProjectId = a.ProjectId,
                          ProjectName = b.ProjectName,
                          ProjectType = a.ProjectType,
                          Status = a.Status,
                          TenderPrice = a.TenderPrice
                      };


            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.ProjectName.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.PageBy(input).ToList().MapTo<List<OATenderAuditListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OATenderAuditListDto>(total, model);
        }

        public async Task Update(OATenderAuditInputDto input)
        {
            var ret = _oaTenderAuditRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
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
                BusinessType = (int)AbpFileBusinessType.OA投标文件审查附件,
                Files = fileList
            });
            _oaTenderAuditRepository.Update(ret);
        }
    }
}
