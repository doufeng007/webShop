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
    public class B_StoreSignUpAppService : FRMSCoreAppServiceBase, IB_StoreSignUpAppService
    { 
        private readonly IRepository<B_StoreSignUp, Guid> _repository;
		
        public B_StoreSignUpAppService(IRepository<B_StoreSignUp, Guid> repository
		
		)
        {
            this._repository = repository;
			
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<B_StoreSignUpListOutputDto>> GetList(GetB_StoreSignUpListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
						
                        select new B_StoreSignUpListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Provinces = a.Provinces,
                            County = a.County,
                            City = a.City,
                            Address = a.Address,
                            BankNumber = a.BankNumber,
                            BankUserName = a.BankUserName,
                            BankName = a.BankName,
                            OpenDate = a.OpenDate,
                            StorArea = a.StorArea,
                            Goods = a.Goods,
                            CreationTime = a.CreationTime
							
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<B_StoreSignUpListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<B_StoreSignUpOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_StoreSignUpOutputDto>();
		}
		/// <summary>
        /// 添加一个B_StoreSignUp
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateB_StoreSignUpInput input)
        {
                var newmodel = new B_StoreSignUp()
                {
                    UserId = input.UserId,
                    Provinces = input.Provinces,
                    County = input.County,
                    City = input.City,
                    Address = input.Address,
                    BankNumber = input.BankNumber,
                    BankUserName = input.BankUserName,
                    BankName = input.BankName,
                    OpenDate = input.OpenDate,
                    StorArea = input.StorArea,
                    Goods = input.Goods
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个B_StoreSignUp
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateB_StoreSignUpInput input)
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
			   dbmodel.Address = input.Address;
			   dbmodel.BankNumber = input.BankNumber;
			   dbmodel.BankUserName = input.BankUserName;
			   dbmodel.BankName = input.BankName;
			   dbmodel.OpenDate = input.OpenDate;
			   dbmodel.StorArea = input.StorArea;
			   dbmodel.Goods = input.Goods;

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