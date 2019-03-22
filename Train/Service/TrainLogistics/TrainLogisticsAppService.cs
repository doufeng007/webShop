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
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace Train
{
    public class TrainLogisticsAppService : FRMSCoreAppServiceBase, ITrainLogisticsAppService
    { 
        private readonly IRepository<TrainLogistics, Guid> _repository;
		
        public TrainLogisticsAppService(IRepository<TrainLogistics, Guid> repository
		
		)
        {
            this._repository = repository;
			
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<TrainLogisticsListOutputDto>> GetList(GetTrainLogisticsListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
                        select new TrainLogisticsListOutputDto()
                        {
                            Id = a.Id,
                            培训编号 = a.培训编号,
                            类型名 = a.类型名,
                            类型值 = a.类型值,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<TrainLogisticsListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<TrainLogisticsOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
            return model.MapTo<TrainLogisticsOutputDto>();
		}
		/// <summary>
        /// 添加一个TrainLogistics
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateTrainLogisticsInput input)
        {
                var newmodel = new TrainLogistics()
                {
                    培训编号 = input.培训编号,
                    类型名 = input.类型名,
                    类型值 = input.类型值
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个TrainLogistics
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateTrainLogisticsInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
               }
			   
			   dbmodel.培训编号 = input.培训编号;
			   dbmodel.类型名 = input.类型名;
			   dbmodel.类型值 = input.类型值;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
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