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
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Application
{
    public class QrCodeAppService : FRMSCoreAppServiceBase, IQrCodeAppService
    { 
        private readonly IRepository<QrCode, Guid> _repository;
		
        public QrCodeAppService(IRepository<QrCode, Guid > repository)
        {
            this._repository = repository;
        }

        public async Task<QrCode> Get(EntityDto<Guid> input)
        {
            if (!_repository.GetAll().Any(x => x.Id ==input.Id))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return _repository.GetAll().First(x => x.Id == input.Id);
        }

	
    }
}