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
using Abp.Application.Services;

namespace ZCYX.FRMSCore.Application
{
    [RemoteService(IsEnabled = false)]
    public class RoleRelationManager : ApplicationService
    { 
        private readonly IRepository<RoleRelation, Guid> _repository;
		
        public RoleRelationManager(IRepository<RoleRelation, Guid> repository)
        {
            this._repository = repository;			
        }
		/// <summary>
        /// 添加一个RoleRelation
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public void Create(CreateRoleRelationInput input)
        {
                var newmodel = new RoleRelation()
                {
                    RelationId = input.RelationId,
                    Type = input.Type,
                    RelationUserId = input.RelationUserId,
                    UserId = input.UserId,
                    Roles = input.Roles,
                    StartTime = input.StartTime,
                    EndTime = input.EndTime
		        };
                 _repository.Insert(newmodel);
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            organizeManager.UpdateRelationRoleAndPermission(newmodel.Id, newmodel.UserId, newmodel.RelationUserId);
        }
        public bool IsExistence(long userId,DateTime startTime,DateTime endTime)
        {
            return
                  _repository.GetAll().Count(x => x.RelationUserId == userId && ((x.StartTime <= startTime && x.EndTime >= startTime) ||
           (x.StartTime <= endTime && x.EndTime >= endTime))) > 0;
        }
        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public  void Delete(EntityDto<Guid> input)
        {
             _repository.Delete(x=>x.Id == input.Id);
        }
    }
}