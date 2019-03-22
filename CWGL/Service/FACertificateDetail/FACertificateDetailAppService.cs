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

//namespace CWGL
//{
//    public class FACertificateDetailAppService : FRMSCoreAppServiceBase, IFACertificateDetailAppService
//    { 
//        private readonly IRepository<FACertificateDetail, Guid> _repository;
		
//        public FACertificateDetailAppService(IRepository<FACertificateDetail, Guid> repository
		
//		)
//        {
//            this._repository = repository;
			
//        }
		
//	    /// <summary>
//        /// 根据条件分页获取列表
//        /// </summary>
//        /// <param name="page">查询实体</param>
//        /// <returns></returns>
//		public async Task<PagedResultDto<FACertificateDetailListOutputDto>> GetList(GetFACertificateDetailListInput input)
//        {
//			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted)
//                        select new FACertificateDetailListOutputDto()
//                        {
//                            Id = a.Id,
//                            MainId = a.MainId,
//                            AccountingCourseId = a.AccountingCourseId,
//                            BusinessType = a.BusinessType,
//                            Amount = a.Amount,
//                            CreationTime = a.CreationTime
//                        };
//            var toalCount = await query.CountAsync();
//            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
//            return new PagedResultDto<FACertificateDetailListOutputDto>(toalCount, ret);
//        }

//		/// <summary>
//        /// 根据主键获取实体
//        /// </summary>
//        /// <param name="input">主键</param>
//        /// <returns></returns>
		
//		public async Task<FACertificateDetailOutputDto> Get(NullableIdDto<Guid> input)
//		{
			
//		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
//            if (model == null)
//            {
//                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
//            }
//            return model.MapTo<FACertificateDetailOutputDto>();
//		}
//		/// <summary>
//        /// 添加一个FACertificateDetail
//        /// </summary>
//        /// <param name="input">实体</param>
//        /// <returns></returns>
		
//		public async Task Create(CreateFACertificateDetailInput input)
//        {
//                var newmodel = new FACertificateDetail()
//                {
//                    MainId = input.MainId,
//                    AccountingCourseId = input.AccountingCourseId,
//                    BusinessType = input.BusinessType,
//                    Amount = input.Amount
//		        };
				
//                await _repository.InsertAsync(newmodel);
				
//        }

//		/// <summary>
//        /// 修改一个FACertificateDetail
//        /// </summary>
//        /// <param name="input">实体</param>
//        /// <returns></returns>
//		public async Task Update(UpdateFACertificateDetailInput input)
//        {
//		    if (input.Id != Guid.Empty)
//            {
//               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
//               if (dbmodel == null)
//               {
//                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
//               }
			   
//			   dbmodel.MainId = input.MainId;
//			   dbmodel.AccountingCourseId = input.AccountingCourseId;
//			   dbmodel.BusinessType = input.BusinessType;
//			   dbmodel.Amount = input.Amount;

//               await _repository.UpdateAsync(dbmodel);
			   
//            }
//            else
//            {
//               throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
//            }
//        }
		
//		// <summary>
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