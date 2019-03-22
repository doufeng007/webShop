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
using HR.Enum;
using System.Dynamic;

namespace HR
{
    public class HrSystemAppService : FRMSCoreAppServiceBase, IHrSystemAppService
    { 
        private readonly IRepository<HrSystem, Guid> _repository;
        private readonly IRepository<HrSystemRead, Guid> _systemReadrepository;
		
        public HrSystemAppService(IRepository<HrSystem, Guid> repository, IRepository<HrSystemRead, Guid> systemReadrepository)
        {
            this._repository = repository;
            _systemReadrepository = systemReadrepository;
        }
        public List<ExpandoObject> GetHrSystemType(string value = null, string setEmpty = null)
        {
            return EnumExtensions.GetEnumList<HrSystemType>(value, setEmpty);
        }


        public List<HrSystemTypeOutput> GetHrSystemTypeMeList()
        {
            var list = EnumExtensions.GetEnumList<HrSystemType>();
            var newList = new List<HrSystemTypeOutput>();
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var orgid = organizeManager.GetDeptByUserID(AbpSession.UserId.Value);
            foreach (var item in list)
            {
                dynamic def = item;
                var typeId = (HrSystemType)Convert.ToInt32(def.Value);
                var text = (string)def.Text;
                var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.TypeId == typeId)
                            where a.IsAll || a.UserIds.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "") ||
                            a.OrgIds.GetStrContainsArray(orgid.ToString())
                            let isnew = !_systemReadrepository.GetAll().Any(x => x.SystemId == a.Id && x.UserId == AbpSession.UserId.Value)
                            select new HrSystemListOutputDto()
                            {
                                IsNew = isnew
                            };
                var count = query.Count(x=>x.IsNew);
                newList.Add(new HrSystemTypeOutput
                {
                    Id = typeId,
                    Name = text,
                    Count = count,
                });
            }
            return newList;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<HrSystemListOutputDto>> GetList(GetHrSystemListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.TypeId == input.TypeId)

                        select new HrSystemListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Content = a.Content,
                            TypeId = a.TypeId,
                            TypeName = a.TypeId.ToString(),
                            UserIds = a.UserIds,
                            IsAll = a.IsAll,
                            OrgIds = a.OrgIds,
                            CreationTime = a.CreationTime,
                            UserNames = a.UserNames,
                            OrgNames = a.OrgNames
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<HrSystemListOutputDto>(toalCount, ret);
        }
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<HrSystemListOutputDto>> GetListByMe(GetHrSystemListInput input)
        {
            var organizeManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowOrganizationUnitsManager>();
            var orgid = organizeManager.GetDeptByUserID(AbpSession.UserId.Value);
            var query = from a in _repository.GetAll().Where(x=>!x.IsDeleted && x.TypeId==input.TypeId)
                        where a.IsAll || a.UserIds.GetStrContainsArray(AbpSession.UserId.HasValue ? AbpSession.UserId.Value.ToString() : "") || 
                        a.OrgIds.GetStrContainsArray(orgid.ToString())
                        let isnew= !_systemReadrepository.GetAll().Any(x => x.SystemId == a.Id && x.UserId == AbpSession.UserId.Value)
                        select new HrSystemListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Content = a.Content,
                            TypeId = a.TypeId,
                            TypeName = a.TypeId.ToString(),
                            UserIds = a.UserIds,
                            IsAll = a.IsAll,
                            OrgIds = a.OrgIds,
                            CreationTime = a.CreationTime,
                            UserNames = a.UserNames,
                            IsNew = isnew,
                            OrgNames = a.OrgNames
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
			
            return new PagedResultDto<HrSystemListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<HrSystemOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<HrSystemOutputDto>();
            ret.TypeName = ret.TypeId.ToString();

            if (!_systemReadrepository.GetAll().Any(x => x.SystemId == model.Id && x.UserId == AbpSession.UserId.Value))
            {
                var newModel = new HrSystemRead();
                newModel.Id = Guid.NewGuid();
                newModel.SystemId = model.Id;
                newModel.UserId = AbpSession.UserId.Value;
                await _systemReadrepository.InsertAsync(newModel);
            }
            return ret;
        }
		/// <summary>
        /// 添加一个HrSystem
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		
		public async Task Create(CreateHrSystemInput input)
        {
                var newmodel = new HrSystem()
                {
                    Title = input.Title,
                    Content = input.Content,
                    TypeId = input.TypeId,
                    UserIds = input.UserIds,
                    IsAll = input.IsAll,
                    OrgIds = input.OrgIds,
                    UserNames = input.UserNames,
                    OrgNames = input.OrgNames
		        };
				
                await _repository.InsertAsync(newmodel);
				
        }

		/// <summary>
        /// 修改一个HrSystem
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		public async Task Update(UpdateHrSystemInput input)
        {
		    if (input.Id != Guid.Empty)
            {
               var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
               if (dbmodel == null)
               {
                   throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
               }
			   
			   dbmodel.Title = input.Title;
			   dbmodel.Content = input.Content;
			   dbmodel.TypeId = input.TypeId;
			   dbmodel.UserIds = input.UserIds;
			   dbmodel.IsAll = input.IsAll;
			   dbmodel.OrgIds = input.OrgIds;
			   dbmodel.UserNames = input.UserNames;
			   dbmodel.OrgNames = input.OrgNames;

               await _repository.UpdateAsync(dbmodel);
			   
            }
            else
            {
               throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }
		
		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x=>x.Id == input.Id);
        }
    }
}