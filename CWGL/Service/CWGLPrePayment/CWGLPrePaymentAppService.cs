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
using CWGL.Enums;
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLPrePaymentAppService : FRMSCoreAppServiceBase, ICWGLPrePaymentAppService
    { 
        private readonly IRepository<CWGLPrePayment, Guid> _repository;
        private readonly IRepository<CWGLPrePaymentDetail, Guid> _prePaymentDetailRepository;
		
        public CWGLPrePaymentAppService(IRepository<CWGLPrePayment, Guid> repository
		, IRepository<CWGLPrePaymentDetail, Guid> prePaymentDetailRepository
        )
        {
            this._repository = repository;
            _prePaymentDetailRepository = prePaymentDetailRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<CWGLPrePaymentListOutputDto>> GetList(GetCWGLPrePaymentListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        let m = (from b in _prePaymentDetailRepository.GetAll().Where(x => !x.IsDeleted && x.PrePaymentId == a.Id) select b).Sum(x => x.Money)
                        select new CWGLPrePaymentListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Name = a.Name,
                            Cause = a.Cause,
                            Money = a.Money,
                            SettleState = a.SettleState,
                            PrePaymentMoney = m,
                            OpenModel = a.SettleState!=2 && a.CreatorUserId==AbpSession.UserId.Value?1:2,
                            SettleState_Name =Enum.GetName(typeof(SettleState),a.SettleState)
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<CWGLPrePaymentListOutputDto>(toalCount, ret);
        }

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		
		public async Task<CWGLPrePaymentOutputDto> Get(NullableIdDto<Guid> input)
		{
			
		    var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var m = (from b in _prePaymentDetailRepository.GetAll().Where(x => !x.IsDeleted && x.PrePaymentId == model.Id) select b).Sum(x => x.Money);
            var tmp = model.MapTo<CWGLPrePaymentOutputDto>();
            tmp.SettleState_Name = Enum.GetName(typeof(SettleState), model.SettleState);
            tmp.PrePaymentMoney = m;
            return tmp;
		}
		/// <summary>
        /// 添加一个CWGLPrePayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateCWGLPrePaymentInput input)
        {
                var newmodel = new CWGLPrePayment()
                {
                    Name = input.Name,
                    Cause = input.Cause,
                    Money = input.Money,
                    SettleState =0
		        };				
                await _repository.InsertAsync(newmodel);				
        }

        /// <summary>
        /// 修改一个CWGLPrePayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [Microsoft.AspNetCore.Mvc.ServiceFilter(typeof(FinancialAccountingCertificateFilterAttribute))]
        public async Task Update(UpdateCWGLPrePaymentInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
               }
			   
			   dbmodel.Name = input.Name;
			   dbmodel.Cause = input.Cause;
			   dbmodel.Money = input.Money;
                input.FACData.BusinessId = input.Id.ToString();
                await _repository.UpdateAsync(dbmodel);			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }
		
    }
}