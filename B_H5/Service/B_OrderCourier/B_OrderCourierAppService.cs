//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Abp.Application.Services.Dto;
//using Abp.AutoMapper;
//using Abp.Linq.Extensions;
//using System.Linq.Dynamic;
//using System.Diagnostics;
//using Abp.Domain.Repositories;
//using System.Web;
//using Castle.Core.Internal;
//using Abp.UI;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Dynamic.Core;
//using ZCYX.FRMSCore;
//using Abp.File;
//using Abp.WorkFlow;
//using ZCYX.FRMSCore.Application;
//using ZCYX.FRMSCore.Extensions;
//using ZCYX.FRMSCore.Model;

//namespace B_H5
//{
//    public class B_OrderCourierAppService : FRMSCoreAppServiceBase, IB_OrderCourierAppService
//    { 
//        private readonly IRepository<B_OrderCourier, Guid> _repository;
		
//        public B_OrderCourierAppService(IRepository<B_OrderCourier, Guid> repository
		
//		)
//        {
//            this._repository = repository;
			
//        }
		
//	    /// <summary>
//        /// 根据条件分页获取列表
//        /// </summary>
//        /// <param name="page">查询实体</param>
//        /// <returns></returns>
//		public async Task<PagedResultDto<B_OrderCourierListOutputDto>> GetList(GetB_OrderCourierListInput input)
//        {
//			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
						
//                        select new B_OrderCourierListOutputDto()
//                        {
//                            Id = a.Id,
//                            OrderId = a.OrderId,
//                            CourierNum = a.CourierNum,
//                            CourierName = a.CourierName,
//                            DeliveryFee = a.DeliveryFee,
//                            Status = a.Status,
//                            CreationTime = a.CreationTime
							
//                        };
//            var toalCount = await query.CountAsync();
//            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
//            return new PagedResultDto<B_OrderCourierListOutputDto>(toalCount, ret);
//        }

//		/// <summary>
//        /// 根据主键获取实体
//        /// </summary>
//        /// <param name="input">主键</param>
//        /// <returns></returns>
		
//		public async Task<B_OrderCourierOutputDto> Get(NullableIdDto<Guid> input)
//		{
			
//		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
//            if (model == null)
//            {
//                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
//            }
//            return model.MapTo<B_OrderCourierOutputDto>();
//		}
//		/// <summary>
//        /// 添加一个B_OrderCourier
//        /// </summary>
//        /// <param name="input">实体</param>
//        /// <returns></returns>
		
//		public async Task Create(CreateB_OrderCourierInput input)
//        {
//                var newmodel = new B_OrderCourier()
//                {
//                    OrderId = input.OrderId,
//                    CourierNum = input.CourierNum,
//                    CourierName = input.CourierName,
//                    DeliveryFee = input.DeliveryFee,
//                    Status = input.Status
//		        };
				
//                await _repository.InsertAsync(newmodel);
				
//        }

//		/// <summary>
//        /// 修改一个B_OrderCourier
//        /// </summary>
//        /// <param name="input">实体</param>
//        /// <returns></returns>
//		public async Task Update(UpdateB_OrderCourierInput input)
//        {
//		    if (input.Id != Guid.Empty)
//            {
//               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
//               if (dbmodel == null)
//               {
//                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
//               }
			   
//			   dbmodel.OrderId = input.OrderId;
//			   dbmodel.CourierNum = input.CourierNum;
//			   dbmodel.CourierName = input.CourierName;
//			   dbmodel.DeliveryFee = input.DeliveryFee;
//			   dbmodel.Status = input.Status;

//               await _repository.UpdateAsync(dbmodel);
			   
//            }
//            else
//            {
//               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
//            }
//        }
		
//		/// <summary>
//        /// 逻辑删除实体
//        /// </summary>
//        /// <param name="input">主键</param>
//        /// <returns></returns>
//		public async Task Delete(EntityDto<Guid> input)
//        {
//            await _repository.DeleteAsync(x=>x.Id == input.Id);
//        }
//    }
//}