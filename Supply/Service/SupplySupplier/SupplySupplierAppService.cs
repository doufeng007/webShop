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
using ZCYX.FRMSCore.Model;
using Abp.Extensions;

namespace Supply
{
    public class SupplySupplierAppService : FRMSCoreAppServiceBase, ISupplySupplierAppService
    {
        private readonly IRepository<SupplySupplier, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;
        public SupplySupplierAppService(IRepository<SupplySupplier, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService)
        {
            _repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="input">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<SupplySupplierListOutputDto>> GetList(GetSupplySupplierListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new SupplySupplierListOutputDto()
                        {
                            Id = a.Id,
                            CreationTime = a.CreationTime,
                            Type = a.Type,
                            Name = a.Name,
                            MainBusiness = a.MainBusiness,
                            SalesContact = a.SalesContact,
                            SalesContactTel = a.SalesContactTel,
                            Email = a.Email,
                            Remark = a.Remark
                        };
            query = query.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) || x.SalesContact.Contains(input.SearchKey) || x.SalesContactTel.Contains(input.SearchKey) || x.MainBusiness.Contains(input.SearchKey) || x.Remark.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<SupplySupplierListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task<SupplySupplierDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var entity = new SupplySupplierDto();
            model.MapTo(entity);
            entity.FileList = await _abpFileRelationAppService.GetListAsync(new GetAbpFilesInput()
            {
                BusinessId = model.Id.ToString(),
                BusinessType = (int)AbpFileBusinessType.供应商合同
            });
            return entity.MapTo<SupplySupplierDto>();
        }

        /// <summary>
        /// 添加一个SupplySupplier
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Create(CreateSupplySupplierInput input)
        {
            if (_repository.GetAll().Any(x => x.Name == input.Name))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该供应商已存在。");
            }
            var id = Guid.NewGuid();
            var newmodel = new SupplySupplier()
            {
                Id = id,
                Type = input.Type,
                Name = input.Name,
                MainBusiness = input.MainBusiness,
                Address = input.Address,
                LegalPerson = input.LegalPerson,
                LegalPersonTel = input.LegalPersonTel,
                SalesContact = input.SalesContact,
                SalesContactTel = input.SalesContactTel,
                Email = input.Email,
                Remark = input.Remark
            };
            if (input.FileList != null)
            {
                var fileList = new List<AbpFileListInput>();
                foreach (var item in input.FileList)
                {
                    fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                }
                await _abpFileRelationAppService.CreateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.供应商合同,
                    Files = fileList
                });
            }
            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个SupplySupplier
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(SupplySupplierUpdateInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
                }
                if (_repository.GetAll().Any(x => x.Name == input.Name && x.Id != input.Id))
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该供应商已存在。");
                }
                dbmodel.Type = input.Type;
                dbmodel.Name = input.Name;
                dbmodel.MainBusiness = input.MainBusiness;
                dbmodel.Address = input.Address;
                dbmodel.LegalPerson = input.LegalPerson;
                dbmodel.LegalPersonTel = input.LegalPersonTel;
                dbmodel.SalesContact = input.SalesContact;
                dbmodel.SalesContactTel = input.SalesContactTel;
                dbmodel.Remark = input.Remark;
                dbmodel.Email = input.Email;
                var fileList = new List<AbpFileListInput>();
                if (input.FileList != null)
                {
                    foreach (var item in input.FileList)
                    {
                        fileList.Add(new AbpFileListInput() { Id = item.Id, Sort = item.Sort });
                    }
                }
                await _abpFileRelationAppService.UpdateAsync(new CreateFileRelationsInput()
                {
                    BusinessId = input.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.供应商合同,
                    Files = fileList
                });
                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
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