using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;
using Abp.Linq.Extensions;
using Abp.Authorization;

namespace Project
{
    public class OATenderEnemyAppService : ApplicationService, IOATenderEnemyAppService
    {
        private readonly IRepository<OATenderEnemy, Guid> _oaTenderEnemyRepository;
        private readonly IRepository<OABidProject, Guid> _oABidProjectRepository;
        private readonly WorkFlowTaskManager _workFlowTaskManager;
        private readonly WorkFlowBusinessTaskManager _workFlowBusinessTaskManager;
        public OATenderEnemyAppService(IRepository<OATenderEnemy, Guid> oaTenderEnemyRepository,
             WorkFlowBusinessTaskManager workFlowBusinessTaskManager,IRepository<OABidProject, Guid> oABidProjectRepository, WorkFlowTaskManager workFlowTaskManager)
        {
            _workFlowBusinessTaskManager = workFlowBusinessTaskManager;
            _workFlowTaskManager = workFlowTaskManager;
            _oaTenderEnemyRepository = oaTenderEnemyRepository;
            _oABidProjectRepository = oABidProjectRepository;
        }

        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public InitWorkFlowOutput Create(OATenderEnemyInputDto input)
        {
            var model = input.MapTo<OATenderEnemy>();
            model.Status = 0;
            if (input.EnemyList != null && input.EnemyList.Count > 0)
            {
                foreach (var x in input.EnemyList)
                {
                    x.Id = Guid.NewGuid();
                }
                model.Enemy = JsonConvert.SerializeObject(input.EnemyList);

            }
            _oaTenderEnemyRepository.Insert(model);
            return new InitWorkFlowOutput() { InStanceId=model.Id.ToString()};
        }
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(WorkFlowRunFilterAttribute))]
        public OATenderEnemyDto Get(GetWorkFlowTaskCommentInput input)
        {
            var id = Guid.Parse(input.InstanceId);
            var ret = _oaTenderEnemyRepository.Get(id);
            var model = ret.MapTo<OATenderEnemyDto>();
            if (string.IsNullOrWhiteSpace(ret.Enemy) == false)
            {
                model.EnemyList = JsonConvert.DeserializeObject<List<OATenderEnemyItem>>(ret.Enemy);
            }
            if (model.EnemyList == null)
            {
                model.EnemyList = new List<OATenderEnemyItem>();
            }
            model.ProjectName = _oABidProjectRepository.Get(ret.ProjectId).ProjectName;

            return model;
        }
        [AbpAuthorize]
        public PagedResultDto<OATenderEnemyListDto> GetAll(WorkFlowPagedAndSortedInputDto input)
        {
            var ret = from a in _oaTenderEnemyRepository.GetAll()
                      join b in _oABidProjectRepository.GetAll() on a.ProjectId equals b.Id
                      select new OATenderEnemyDto()
                      {
                          AuditUser = a.AuditUser,
                          Content = a.Content,
                          CreationTime = a.CreationTime,
                          Id = a.Id,
                          ProjectId = a.ProjectId,
                          ProjectName = b.ProjectName,
                          ProjectType = a.ProjectType,
                          Status = a.Status,


                      };
            if (string.IsNullOrWhiteSpace(input.SearchKey) == false)
            {
                ret = ret.Where(ite => ite.ProjectName.Contains(input.SearchKey));
            }
            var total = ret.Count();
            var model = ret.OrderByDescending(ite => ite.CreationTime).PageBy(input).ToList().MapTo<List<OATenderEnemyListDto>>();
            foreach (var item in model)
            {
                item.InstanceId = item.Id.ToString();
                _workFlowBusinessTaskManager.SupplementWorkFlowBusinessList(input.FlowId, item as BusinessWorkFlowListOutput);
            }
            return new PagedResultDto<OATenderEnemyListDto>(total, model);
        }

        public void Update(OATenderEnemyInputDto input)
        {
            var ret = _oaTenderEnemyRepository.Get(input.Id.Value);
            ret = input.MapTo(ret);
            ret.Enemy= JsonConvert.SerializeObject(input.EnemyList);
            _oaTenderEnemyRepository.Update(ret);
        }
    }
}
