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

namespace B_H5
{
    public class B_AgencyAppService : FRMSCoreAppServiceBase, IB_AgencyAppService
    { 
        private readonly IRepository<B_Agency, Guid> _repository;
		
        public B_AgencyAppService(IRepository<B_Agency, Guid> repository
		
		)
        {
            this._repository = repository;
			
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<B_AgencyListOutputDto>> GetList(GetB_AgencyListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
						
                        select new B_AgencyListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            AgencyLevel = a.AgencyLevel,
                            AgenCyCode = a.AgenCyCode,
                            Provinces = a.Provinces,
                            County = a.County,
                            City = a.City,
                            Address = a.Address,
                            Type = a.Type,
                            SignData = a.SignData,
                            Agreement = a.Agreement,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            OpenId = a.OpenId,
                            UnitId = a.UnitId
							
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<B_AgencyListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<B_AgencyOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_AgencyOutputDto>();
		}
		/// <summary>
        /// 添加一个B_Agency
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateB_AgencyInput input)
        {
                var newmodel = new B_Agency()
                {
                    UserId = input.UserId,
                    AgencyLevel = input.AgencyLevel,
                    AgenCyCode = input.AgenCyCode,
                    Provinces = input.Provinces,
                    County = input.County,
                    City = input.City,
                    Address = input.Address,
                    Type = input.Type,
                    SignData = input.SignData,
                    Agreement = input.Agreement,
                    Status = input.Status,
                    OpenId = input.OpenId,
                    UnitId = input.UnitId
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个B_Agency
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateB_AgencyInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.UserId = input.UserId;
			   dbmodel.AgencyLevel = input.AgencyLevel;
			   dbmodel.AgenCyCode = input.AgenCyCode;
			   dbmodel.Provinces = input.Provinces;
			   dbmodel.County = input.County;
			   dbmodel.City = input.City;
			   dbmodel.Address = input.Address;
			   dbmodel.Type = input.Type;
			   dbmodel.SignData = input.SignData;
			   dbmodel.Agreement = input.Agreement;
			   dbmodel.Status = input.Status;
			   dbmodel.OpenId = input.OpenId;
			   dbmodel.UnitId = input.UnitId;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
		/// <summary>
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