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
using Abp.WorkFlowDictionary;
using Abp.Domain.Uow;
using Abp.Application.Services;

namespace B_H5
{
    public class B_AgencyManager : ApplicationService
    {
        private readonly IRepository<B_Agency, Guid> _repository;
        private readonly IRepository<AbpDictionary, Guid> _abpDictionaryrepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        


        public B_AgencyManager(IRepository<B_Agency, Guid> repository, IRepository<AbpDictionary, Guid> abpDictionaryrepository
            , IUnitOfWorkManager unitOfWorkManager, IAbpFileRelationAppService abpFileRelationAppService

        )
        {
            this._repository = repository;
            _abpDictionaryrepository = abpDictionaryrepository;
            _unitOfWorkManager = unitOfWorkManager;
            _abpFileRelationAppService = abpFileRelationAppService;

        }

     
    }

    public class B_AgencyUserManager
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }


        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 我的云仓可提取数
        /// </summary>
        public int CW_Number { get; set; }

        /// <summary>
        /// 我的云仓缺货数
        /// </summary>
        public int CW_LessCount { get; set; }

        /// <summary>
        /// 代理级别
        /// </summary>
        public int B_AgencyLeavel { get; set; }

        /// <summary>
        /// 上级代理
        /// </summary>
        public B_AgencyUserManager Parent_Agency { get; set; }

        /// <summary>
        /// 直接下级代理
        /// </summary>
        public List<B_AgencyUserManager> Childe_B_Agencys { get; set; }

        /// <summary>
        /// 进货
        /// </summary>
        /// <param name="goodsAmout"></param>
        public void OrderIn(decimal goodsAmout)
        {
            if ((this.GoodsPayment + this.Balance) < goodsAmout)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "余额不足，无法支付");
            else
            {
                if (GoodsPayment < goodsAmout)
                {
                    GoodsPayment = 0;
                    Balance = Balance - (goodsAmout - GoodsPayment);
                }
                else
                    GoodsPayment = GoodsPayment - goodsAmout;
            }


        }

    }
}