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
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class ProjectProgressConfigAppService : FRMSCoreAppServiceBase, IProjectProgressConfigAppService
    { 
        private readonly IRepository<ProjectProgressConfig, Guid> _repository;

        public ProjectProgressConfigAppService(IRepository<ProjectProgressConfig, Guid> repository)
        {
            this._repository = repository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<ProjectProgressConfigListOutputDto>> GetList(GetProjectProgressConfigListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new ProjectProgressConfigListOutputDto()
                        {
                            Id = a.Id,
                            ProjectBaseId = a.ProjectBaseId,
                            FirstAuditKey = a.FirstAuditKey,
                            JiliangKey = a.JiliangKey,
                            JijiaKey = a.JijiaKey,
                            SelfAuditKey = a.SelfAuditKey,
                            SecondAuditKey = a.SecondAuditKey,
                            LastAuditKey = a.LastAuditKey,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<ProjectProgressConfigListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task<ProjectProgressConfigOutputDto> Get(NullableIdDto<Guid> input)
		{
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<ProjectProgressConfigOutputDto>();
		}
		/// <summary>
        /// 添加一个ProjectProgressConfig
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Create(CreateProjectProgressConfigInput input)
        {
                var newmodel = new ProjectProgressConfig()
                {
                    ProjectBaseId = input.ProjectBaseId,
                    FirstAuditKey = input.FirstAuditKey,
                    JiliangKey = input.JiliangKey,
                    JijiaKey = input.JijiaKey,
                    SelfAuditKey = input.SelfAuditKey,
                    SecondAuditKey = input.SecondAuditKey,
                    LastAuditKey = input.LastAuditKey
		        };
                await _repository.InsertAsync(newmodel);
        }

		/// <summary>
        /// 修改一个ProjectProgressConfig
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(ProjectProgressConfig input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
               }
			   dbmodel.ProjectBaseId = input.ProjectBaseId;
			   dbmodel.FirstAuditKey = input.FirstAuditKey;
			   dbmodel.JiliangKey = input.JiliangKey;
			   dbmodel.JijiaKey = input.JijiaKey;
			   dbmodel.SelfAuditKey = input.SelfAuditKey;
			   dbmodel.SecondAuditKey = input.SecondAuditKey;
			   dbmodel.LastAuditKey = input.LastAuditKey;

               await _repository.UpdateAsync(dbmodel);
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

		// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}