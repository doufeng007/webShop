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
using Abp.Application.Services;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Model;

namespace HR
{
    [RemoteService(IsEnabled = false)]
    public class EmployeeTrainingSystemUnitPostsAppService : FRMSCoreAppServiceBase, IEmployeeTrainingSystemUnitPostsAppService
    { 
        private readonly IRepository<EmployeeTrainingSystemUnitPosts, Guid> _repository;

        public EmployeeTrainingSystemUnitPostsAppService(IRepository<EmployeeTrainingSystemUnitPosts, Guid> repository)
        {
            this._repository = repository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<EmployeeTrainingSystemUnitPostsListOutputDto>> GetList(GetEmployeeTrainingSystemUnitPostsListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new EmployeeTrainingSystemUnitPostsListOutputDto()
                        {
                            Id = a.Id,
                            SysId = a.SysId,
                            PortsId = a.PortsId,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<EmployeeTrainingSystemUnitPostsListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task<EmployeeTrainingSystemUnitPostsOutputDto> Get(NullableIdDto<Guid> input)
		{
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return model.MapTo<EmployeeTrainingSystemUnitPostsOutputDto>();
		}
		/// <summary>
        /// 添加一个EmployeeTrainingSystemUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Create(EmployeeTrainingSystemUnitPosts input)
        {
                var newmodel = new EmployeeTrainingSystemUnitPosts()
                {
                    SysId = input.SysId,
                    PortsId = input.PortsId
		        };
                await _repository.InsertAsync(newmodel);
        }

		/// <summary>
        /// 修改一个EmployeeTrainingSystemUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(EmployeeTrainingSystemUnitPosts input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
               }
			   dbmodel.SysId = input.SysId;
			   dbmodel.PortsId = input.PortsId;

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