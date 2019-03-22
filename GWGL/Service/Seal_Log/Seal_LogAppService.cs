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

namespace GWGL
{
    public class Seal_LogAppService : FRMSCoreAppServiceBase, ISeal_LogAppService
    {
        private readonly IRepository<Seal_Log, Guid> _repository;
        private readonly IRepository<GW_Seal, Guid> _gw_SealRepository;
        private readonly IRepository<WorkFlowOrganizationUnits, long> _organizationUnitRepository;
        private readonly IRepository<WorkFlowUserOrganizationUnits, long> _userOrganizationUnitRepository;
        public Seal_LogAppService(IRepository<Seal_Log, Guid> repository, IRepository<GW_Seal, Guid> gw_SealRepository,
            IRepository<WorkFlowOrganizationUnits, long> organizationUnitRepository, IRepository<WorkFlowUserOrganizationUnits, long> userOrganizationUnitRepository

        )
        {
            this._repository = repository;
            _gw_SealRepository = gw_SealRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<Seal_LogListOutputDto>> GetList(GetSeal_LogListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _gw_SealRepository.GetAll() on a.Seal_Id equals b.Id
                        join c in UserManager.Users on a.UserId equals c.Id
                        join uo in _userOrganizationUnitRepository.GetAll() on a.UserId equals uo.UserId
                        join o in _organizationUnitRepository.GetAll() on uo.OrganizationUnitId equals o.Id
                        where uo.IsMain == true
                        select new Seal_LogListOutputDto()
                        {
                            Id = a.Id,
                            Seal_Id = a.Seal_Id,
                            Seal_Name = b.Name,
                            UserId = a.UserId,
                            User_Name = c.Name,
                            User_OrgName = o.DisplayName,
                            Title = a.Title,
                            Copies = a.Copies,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime,
                            SealType = b.SealType,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            foreach (var item in ret)
            {
                item.SealType_Name = item.SealType.ToString();
            }

            return new PagedResultDto<Seal_LogListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<Seal_LogListOutputDto> Get(NullableIdDto<Guid> input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _gw_SealRepository.GetAll() on a.Seal_Id equals b.Id
                        join c in UserManager.Users on a.UserId equals c.Id
                        join uo in _userOrganizationUnitRepository.GetAll() on a.UserId equals uo.UserId
                        join o in _organizationUnitRepository.GetAll() on uo.OrganizationUnitId equals o.Id
                        where uo.IsMain == true && a.Id == input.Id.Value
                        select new Seal_LogListOutputDto()
                        {
                            Id = a.Id,
                            Seal_Id = a.Seal_Id,
                            Seal_Name = b.Name,
                            UserId = a.UserId,
                            User_Name = c.Name,
                            User_OrgName = o.DisplayName,
                            Title = a.Title,
                            Copies = a.Copies,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime,
                            SealType = b.SealType,
                        };

            var model = await query.FirstOrDefaultAsync();
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            model.SealType_Name = model.SealType.ToString();
            return model;
        }
        /// <summary>
        /// 添加一个Seal_Log
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateSeal_LogInput input)
        {
            var newmodel = new Seal_Log()
            {
                Seal_Id = input.Seal_Id,
                UserId = input.UserId,
                Title = input.Title,
                Copies = input.Copies,
                Remark = input.Remark
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个Seal_Log
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateSeal_LogInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Seal_Id = input.Seal_Id;
                dbmodel.UserId = input.UserId;
                dbmodel.Title = input.Title;
                dbmodel.Copies = input.Copies;
                dbmodel.Remark = input.Remark;

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
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}