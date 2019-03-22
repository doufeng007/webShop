using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Application
{
    public class WorkListAppService : FRMSCoreAppServiceBase, IWorkListAppService
    { 
        private readonly IRepository<WorkList, Guid> _repository;
		
        public WorkListAppService(IRepository<WorkList, Guid> repository
		
		)
        {
            this._repository = repository;
			
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<WorkListListOutputDto>> GetList(GetWorkListListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
						
                        select new WorkListListOutputDto()
                        {
                            Id = a.Id,
                            BackId = a.BackId,
                            Content = a.Content,
                            CreationTime = a.CreationTime
                        };
            if (!string.IsNullOrEmpty(input.SearchKey))
                query = query.Where(x => x.BackId.Contains(input.SearchKey) || x.Content.Contains(input.SearchKey));
            if (input.StartTime.HasValue)
                query = query.Where(x => x.CreationTime>=input.StartTime.Value);
            if (input.EndTime.HasValue)
                query = query.Where(x => x.CreationTime<=input.EndTime.Value);

            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<WorkListListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 添加一个WorkList
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateWorkListInput input)
        {
                var newmodel = new WorkList()
                {
                    Id = Guid.NewGuid(),
                    BackId = input.BackId,
                    Content = input.Content
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }


    }
}