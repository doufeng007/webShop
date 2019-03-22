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
    public class FollowAppService : FRMSCoreAppServiceBase, IFollowAppService
    { 
        private readonly IRepository<Follow,Guid> _repository;
		
        public FollowAppService(IRepository<Follow,Guid > repository)
        {
            this._repository = repository;
        }


        /// <summary>
        /// 添加一个Follow
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateFollowInput input)
        {
            if (_repository.GetAll().Any(x => x.BusinessType == input.BusinessType && x.BusinessId == input.BusinessId && x.CreatorUserId== AbpSession.UserId.Value))
            {
                _repository.Delete(x => x.BusinessType == input.BusinessType && x.BusinessId == input.BusinessId && x.CreatorUserId== AbpSession.UserId.Value);
            }
            else
            {
                var newmodel = new Follow()
                {
                    Id = Guid.NewGuid(),
                    BusinessType = input.BusinessType,
                    BusinessId = input.BusinessId,
                    CreationTime = DateTime.Now,
                    CreatorUserId = AbpSession.UserId.Value,
                    Parameter = input.Parameter
                };
                await _repository.InsertAsync(newmodel);
            }
        }

	
    }
}