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

namespace B_H5
{
    public class B_ManagerPayAccountAppService : FRMSCoreAppServiceBase, IB_ManagerPayAccountAppService
    {
        private readonly IRepository<B_ManagerPayAccount, Guid> _repository;

        public B_ManagerPayAccountAppService(IRepository<B_ManagerPayAccount, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 后台-账户管理
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_ManagerPayAccountListOutputDto>> GetList(GetB_ManagerPayAccountListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new B_ManagerPayAccountListOutputDto()
                        {
                            Id = a.Id,
                            Type = a.Type,
                            Account = a.Account,
                            BankName = a.BankName,
                            BankUserName = a.BankUserName,
                            WxName = a.WxName,
                            Remark = a.Remark,
                            Status = a.Status,
                            CreationTime = a.CreationTime

                        };

            query = query.WhereIf(input.Type.HasValue, r => r.Type == input.Type.Value).WhereIf(!input.SearchKey.IsNullOrEmpty(), r => r.Account.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_ManagerPayAccountListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_ManagerPayAccountOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_ManagerPayAccountOutputDto>();
        }


        /// <summary>
        /// 新增一个账户
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_ManagerPayAccountInput input)
        {
            var newmodel = new B_ManagerPayAccount()
            {
                Type = input.Type,
                Account = input.Account,
                BankName = input.BankName,
                BankUserName = input.BankUserName,
                WxName = input.WxName,
                Remark = input.Remark,
                Status = PayAccountStatus.上线,
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个账户
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_ManagerPayAccountInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Type = input.Type;
                dbmodel.Account = input.Account;
                dbmodel.BankName = input.BankName;
                dbmodel.BankUserName = input.BankUserName;
                dbmodel.WxName = input.WxName;
                dbmodel.Remark = input.Remark;

                await _repository.UpdateAsync(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }
    }
}