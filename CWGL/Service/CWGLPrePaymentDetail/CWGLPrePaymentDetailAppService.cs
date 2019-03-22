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
using ZCYX.FRMSCore.Model;

namespace CWGL
{
    public class CWGLPrePaymentDetailAppService : FRMSCoreAppServiceBase, ICWGLPrePaymentDetailAppService
    { 
        private readonly IRepository<CWGLPrePaymentDetail, Guid> _repository;
        private readonly IRepository<CWGLPrePayment, Guid> _prePaymentRepository;
		
        public CWGLPrePaymentDetailAppService(IRepository<CWGLPrePaymentDetail, Guid> repository, IRepository<CWGLPrePayment, Guid> prePaymentRepository)
        {
            this._repository = repository;
            _prePaymentRepository = prePaymentRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<CWGLPrePaymentDetailListOutputDto>> GetList(GetCWGLPrePaymentDetailListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.PrePaymentId==input.PrePaymentId)
                        select new CWGLPrePaymentDetailListOutputDto()
                        {
                            Id = a.Id,
                            Money = a.Money,
                            Mode = a.Mode,
                            BankName = a.BankName,
                            CardNumber = a.CardNumber,
                            BankOpenName = a.BankOpenName,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<CWGLPrePaymentDetailListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 添加一个CWGLPrePaymentDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateCWGLPrePaymentDetailInput input)
        {
            var model = _prePaymentRepository.Get(input.PrePaymentId);
            var sumMoney = _repository.GetAll().Where(x => x.PrePaymentId == input.PrePaymentId).Sum(x => x.Money);
            if (model.Money < sumMoney + input.Money)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "已收金额不得大于应收金额。");
            }
            if (model.SettleState == 0)
            {
                model.SettleState = 1;
                await _prePaymentRepository.UpdateAsync(model);
            }
            if (model.Money == sumMoney + input.Money)
            {
                model.SettleState = 2;
                await _prePaymentRepository.UpdateAsync(model);
            }
           
            
            var newmodel = new CWGLPrePaymentDetail()
            {
                PrePaymentId = input.PrePaymentId,
                Money = input.Money,
                Mode = input.Mode,
                BankName = input.BankName,
                CardNumber = input.CardNumber,
                BankOpenName = input.BankOpenName
            };
            await _repository.InsertAsync(newmodel);

        }
    }
}