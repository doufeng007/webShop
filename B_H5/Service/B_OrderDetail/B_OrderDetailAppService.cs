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
  //  public class B_OrderDetailAppService : FRMSCoreAppServiceBase, IB_OrderDetailAppService
  //  { 
  //      private readonly IRepository<B_OrderDetail, Guid> _repository;
		
  //      public B_OrderDetailAppService(IRepository<B_OrderDetail, Guid> repository
		
		//)
  //      {
  //          this._repository = repository;
			
  //      }
		
	 //   /// <summary>
  //      /// 根据条件分页获取列表
  //      /// </summary>
  //      /// <param name="page">查询实体</param>
  //      /// <returns></returns>
		//public async Task<PagedResultDto<B_OrderDetailListOutputDto>> GetList(GetB_OrderDetailListInput input)
  //      {
		//	var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
						
  //                      select new B_OrderDetailListOutputDto()
  //                      {
  //                          Id = a.Id,
  //                          BId = a.BId,
  //                          BType = a.BType,
  //                          Number = a.Number,
  //                          CategroyId = a.CategroyId,
  //                          GoodsId = a.GoodsId,
  //                          Amout = a.Amout,
  //                          CreationTime = a.CreationTime
							
  //                      };
  //          var toalCount = await query.CountAsync();
  //          var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
  //          return new PagedResultDto<B_OrderDetailListOutputDto>(toalCount, ret);
  //      }

		///// <summary>
  //      /// 根据主键获取实体
  //      /// </summary>
  //      /// <param name="input">主键</param>
  //      /// <returns></returns>
		
		//public async Task<B_OrderDetailOutputDto> Get(NullableIdDto<Guid> input)
		//{
			
		//    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
  //          if (model == null)
  //          {
  //              throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
  //          }
  //          return model.MapTo<B_OrderDetailOutputDto>();
		//}
		///// <summary>
  //      /// 添加一个B_OrderDetail
  //      /// </summary>
  //      /// <param name="input">实体</param>
  //      /// <returns></returns>
		
		//public async Task Create(CreateB_OrderDetailInput input)
  //      {
  //              var newmodel = new B_OrderDetail()
  //              {
  //                  BId = input.BId,
  //                  BType = input.BType,
  //                  Number = input.Number,
  //                  CategroyId = input.CategroyId,
  //                  GoodsId = input.GoodsId,
  //                  Amout = input.Amout
		//        };
				
  //              await _repository.InsertAsync(newmodel);
				
  //      }

		///// <summary>
  //      /// 修改一个B_OrderDetail
  //      /// </summary>
  //      /// <param name="input">实体</param>
  //      /// <returns></returns>
		//public async Task Update(UpdateB_OrderDetailInput input)
  //      {
		//    if (input.Id != Guid.Empty)
  //          {
  //             var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
  //             if (dbmodel == null)
  //             {
  //                 throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
  //             }
			   
		//	   dbmodel.BId = input.BId;
		//	   dbmodel.BType = input.BType;
		//	   dbmodel.Number = input.Number;
		//	   dbmodel.CategroyId = input.CategroyId;
		//	   dbmodel.GoodsId = input.GoodsId;
		//	   dbmodel.Amout = input.Amout;

  //             await _repository.UpdateAsync(dbmodel);
			   
  //          }
  //          else
  //          {
  //             throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
  //          }
  //      }
		
		///// <summary>
  //      /// 逻辑删除实体
  //      /// </summary>
  //      /// <param name="input">主键</param>
  //      /// <returns></returns>
		//public async Task Delete(EntityDto<Guid> input)
  //      {
  //          await _repository.DeleteAsync(x=>x.Id == input.Id);
  //      }
  //  }
}