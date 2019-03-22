using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using ZCYX.FRMSCore.Application;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace Abp.WorkFlowDictionary
{
    public class WorkFlowDictionaryAppService : AsyncCrudAppService<AbpDictionary, WorkFlowDictionaryDto, Guid, PagedResultRequestDto, CreateWorkFlowDictionaryDto, WorkFlowDictionaryDto>, IWorkFlowDictionaryAppService
    {
        private readonly IRepository<AbpDictionary, Guid> _repository;
        public WorkFlowDictionaryAppService(IRepository<AbpDictionary, Guid> repository)
            : base(repository)
        {
            _repository = repository;
        }

        public async override Task<WorkFlowDictionaryDto> Create(CreateWorkFlowDictionaryDto input)
        {
            if (_repository.GetAll().Any(r => r.ParentID == input.ParentID && r.Title == input.Title))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "同一类别下标题重复");
            return await base.Create(input);
        }

        public async override Task<WorkFlowDictionaryDto> Update(WorkFlowDictionaryDto input)
        {
            if (_repository.GetAll().Any(r => r.ParentID == input.ParentID && r.Id != input.Id && r.Title == input.Title))
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "同一类别下标题重复");
            return await base.Update(input);
        }



        public async Task<PagedResultDto<WorkFlowDictionaryDto>> GetWorkFlowDictionaries(GetWorkFlowDictionariesInput input)
        {
            var query = _repository.GetAll().WhereIf(input.RootId.HasValue, r => r.ParentID == input.RootId.Value);
            var totalCount = await query.CountAsync();
            var datas = await query.OrderBy(r => r.Sort).PageBy(input).ToListAsync();
            return new PagedResultDto<WorkFlowDictionaryDto>(totalCount, datas.MapTo<List<WorkFlowDictionaryDto>>());
        }


        public override async Task<PagedResultDto<WorkFlowDictionaryDto>> GetAll(PagedResultRequestDto input)
        {
            var query = from a in _repository.GetAll()
                        select a;
            var count = await query.CountAsync();
            var data = await query.OrderBy(r => r.Sort).PageBy(input).ToListAsync();
            return new PagedResultDto<WorkFlowDictionaryDto>(count, data.MapTo<List<WorkFlowDictionaryDto>>());
        }

        /// <summary>
        /// 给指定id顺序的字典项 排序 
        /// </summary>
        /// <param name="input">，分隔id的字符串</param>
        /// <returns></returns>
        public async Task Sort(string input)
        {
            var arry = input.Split(',');
            var sort = 1;
            foreach (var item in arry)
            {
                var model = await _repository.GetAsync(item.ToGuid());
                model.Sort = sort;
                sort++;
            }
        }
    }
}
