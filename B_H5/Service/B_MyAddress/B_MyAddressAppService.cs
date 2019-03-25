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
    public class B_MyAddressAppService : FRMSCoreAppServiceBase, IB_MyAddressAppService
    { 
        private readonly IRepository<B_MyAddress, Guid> _repository;
		
        public B_MyAddressAppService(IRepository<B_MyAddress, Guid> repository
		
		)
        {
            this._repository = repository;
			
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<B_MyAddressListOutputDto>> GetList(GetB_MyAddressListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
						
                        select new B_MyAddressListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Provinces = a.Provinces,
                            County = a.County,
                            City = a.City,
                            Addres = a.Addres,
                            Consignee = a.Consignee,
                            Tel = a.Tel,
                            IsDefault = a.IsDefault,
                            CreationTime = a.CreationTime
							
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<B_MyAddressListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<B_MyAddressOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_MyAddressOutputDto>();
		}
		/// <summary>
        /// 添加一个B_MyAddress
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateB_MyAddressInput input)
        {
                var newmodel = new B_MyAddress()
                {
                    UserId = input.UserId,
                    Provinces = input.Provinces,
                    County = input.County,
                    City = input.City,
                    Addres = input.Addres,
                    Consignee = input.Consignee,
                    Tel = input.Tel,
                    IsDefault = input.IsDefault
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个B_MyAddress
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateB_MyAddressInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.UserId = input.UserId;
			   dbmodel.Provinces = input.Provinces;
			   dbmodel.County = input.County;
			   dbmodel.City = input.City;
			   dbmodel.Addres = input.Addres;
			   dbmodel.Consignee = input.Consignee;
			   dbmodel.Tel = input.Tel;
			   dbmodel.IsDefault = input.IsDefault;

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