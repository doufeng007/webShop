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

namespace Project.Service.OA.TenderCash
{
    public class OATenderCashAppService : ApplicationService, IOATenderCashAppService
    {
        private readonly IRepository<OATenderCash, Guid> _oaTenderCashRepository;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        public OATenderCashAppService(IRepository<OATenderCash, Guid> oaTenderCashRepository, IAbpFileRelationAppService abpFileRelationAppService,IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowTaskManager workFlowTaskManager
            , WorkFlowBusinessTaskManager workFlowBusinessTaskManager)
        {
            _abpFileRelationAppService = abpFileRelationAppService;
            _oaTenderCashRepository = oaTenderCashRepository;
            _oABidProjectRepository = oABidProjectRepository;
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowTaskManager = workFlowTaskManager;
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task<InitWorkFlowOutput> Create(OATenderCashInputDto input)
        {
            var model = input.MapTo<OATenderCash>();
            model.Status = 0;
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
                    BusinessType = (int)AbpFileBusinessType.OA投标文件申请附件,
                    Files = fileList
                });
            }
            _oaTenderCashRepository.InsertOrUpdate(model);
            return new InitWorkFlowOutput() {  InStanceId=model.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public async Task< OATenderCashDto> Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaTenderCashRepository.Get(id);
            var model = ret.MapTo<OATenderCashDto>();
            model.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput() { BusinessId = input.InstanceId, BusinessType = (int)AbpFileBusinessType.OA投标文件审查附件 });
            model.ProjectName = _oABidProjectRepository.Get(ret.ProjectId).ProjectName;
            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OATenderCashListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = from a in _oaTenderCashRepository.GetAll()
                      join b in _oABidProjectRepository.GetAll() on a.ProjectId equals b.Id
                      select new OATenderCashListDto()
                      {
                          AuditUser = a.AuditUser,

                          CreationTime = a.CreationTime,
                         
                          Id = a.Id,
                          BankName=a.BankName,
                          ProjectId = a.ProjectId,
                          ProjectName = b.ProjectName,
                          ProjectType = a.ProjectType,
                          Status = a.Status,
                          CashPrice = a.CashPrice,
                          CashPriceUp = a.CashPriceUp,
                          
                          EndDate = a.EndDate,
                          StartDate = a.StartDate,
                          ToCompany = a.ToCompany
                      };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.ProjectName.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OATenderCashListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OATenderCashListDto>(total, model);
        }

        public async Task Update(OATenderCashInputDto input)
        {
            var ret = _oaTenderCashRepository.Get(input.Id.Value);
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
                BusinessType = (int)AbpFileBusinessType.OA投标文件申请附件,
                Files = fileList
            });
            _oaTenderCashRepository.Update(ret);
        }
    }
}
