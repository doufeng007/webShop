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
using Abp.Dependency;
using Abp.Application.Services;

namespace B_H5
{
    [RemoteService(IsEnabled = false)]
    public class B_CategroyManager : ApplicationService
    {
        private readonly IRepository<B_Goods, Guid> _repository;
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;
        private readonly IRepository<B_Agency, Guid> _b_AgencyRepository;
        private readonly IRepository<B_AgencyLevel, Guid> _b_AgencyLevelRepository;

        public B_CategroyManager(IRepository<B_Goods, Guid> repository, IRepository<B_Agency, Guid> b_AgencyRepository
            , IRepository<B_AgencyLevel, Guid> b_AgencyLevelRepository, IRepository<B_Categroy, Guid> b_CategroyRepository
        )
        {
            this._repository = repository;
            _b_AgencyRepository = b_AgencyRepository;
            _b_AgencyLevelRepository = b_AgencyLevelRepository;
            _b_CategroyRepository = b_CategroyRepository;
        }




        public decimal GetCategroyPriceForUser(long userId, decimal price)
        {
            var b_Agency = _b_AgencyRepository.FirstOrDefault(r => r.UserId == userId);
            if (b_Agency == null)
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "代理不存在");
            var model = _b_AgencyLevelRepository.Get(b_Agency.AgencyLevelId);
            return price * model.Discount;
        }


        public decimal GetProfitForUser(Guid leavelIdHight, Guid leavelIdLow, Guid categroyId)
        {
            var modelHight = _b_AgencyLevelRepository.Get(leavelIdHight);
            var modelLow = _b_AgencyLevelRepository.Get(leavelIdLow);
            var categroyModel = _b_CategroyRepository.Get(categroyId);
            return categroyModel.Price * (modelHight.Discount - modelLow.Discount);
        }

    }
}