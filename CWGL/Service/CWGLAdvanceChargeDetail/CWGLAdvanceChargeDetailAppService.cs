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
    public class CWGLAdvanceChargeDetailAppService : FRMSCoreAppServiceBase, ICWGLAdvanceChargeDetailAppService
    { 
        private readonly IRepository<CWGLAdvanceChargeDetail, Guid> _repository;
        private readonly IRepository<CWGLAdvanceCharge, Guid> _advanceChargeRepository;
		
        public CWGLAdvanceChargeDetailAppService(IRepository<CWGLAdvanceChargeDetail, Guid> repository, IRepository<CWGLAdvanceCharge, Guid> advanceChargeRepository)
        {
            this._repository = repository;
            _advanceChargeRepository = advanceChargeRepository;
        }
		
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		public async Task<PagedResultDto<CWGLAdvanceChargeDetailListOutputDto>> GetList(GetCWGLAdvanceChargeDetailListInput input)
        {
			var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.AdvanceChargeId==input.AdvanceChargeId)
                        select new CWGLAdvanceChargeDetailListOutputDto()
                        {
                            Id = a.Id,
                            AdvanceChargeId = a.AdvanceChargeId,
                            Money = a.Money,
                            Mode = a.Mode,
                            BankName = a.BankName,
                            CardNumber = a.CardNumber,
                            BankOpenName = a.BankOpenName,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<CWGLAdvanceChargeDetailListOutputDto>(toalCount, ret);
        }
		/// <summary>
        /// 添加一个CWGLAdvanceChargeDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateCWGLAdvanceChargeDetailInput input)
        {
            var model = _advanceChargeRepository.Get(input.AdvanceChargeId);
            var sumMoney = _repository.GetAll().Where(x => x.AdvanceChargeId == input.AdvanceChargeId).Sum(x => x.Money);
            if (model.Money < sumMoney + input.Money)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "已付金额不得大于应付金额。");
            }
            if (model.SettleState == 0)
            {
                model.SettleState = 1;
                await _advanceChargeRepository.UpdateAsync(model);
            }
            if (model.Money == sumMoney + input.Money)
            {
                model.SettleState = 2;
                await _advanceChargeRepository.UpdateAsync(model);
            }

            var newmodel = new CWGLAdvanceChargeDetail()
                {
                    AdvanceChargeId = input.AdvanceChargeId,
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