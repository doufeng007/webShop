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

        public B_CategroyManager(IRepository<B_Goods, Guid> repository

        )
        {
            this._repository = repository;

        }

        
        

        public decimal GetCategroyPriceForUser(long userId, Guid goodsId)
        {
            return 0;
        }

    }
}