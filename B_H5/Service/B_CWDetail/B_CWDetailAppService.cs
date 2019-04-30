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
using Abp.Authorization;

namespace B_H5
{
    public class B_CWDetailAppService : FRMSCoreAppServiceBase, IB_CWDetailAppService
    {
        private readonly IRepository<B_CWDetail, Guid> _repository;
        private readonly IRepository<B_Categroy, Guid> _b_CategroyRepository;

        public B_CWDetailAppService(IRepository<B_CWDetail, Guid> repository, IRepository<B_Categroy, Guid> b_CategroyRepository

        )
        {
            this._repository = repository;
            _b_CategroyRepository = b_CategroyRepository;

        }

        /// <summary>
        ///  H5 云仓进出明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_CWDetailListOutputDto>> GetList(GetB_CWDetailListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_CategroyRepository.GetAll() on a.CategroyId equals b.Id
                        join c in UserManager.Users on a.RelationUserId equals c.Id into g
                        from u in g.DefaultIfEmpty()
                        where a.UserId == AbpSession.UserId.Value
                        select new B_CWDetailListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            RelationUserId = a.RelationUserId,
                            Type = a.Type,
                            BusinessType = a.BusinessType,
                            CategroyId = a.CategroyId,
                            Number = a.Number,
                            CreationTime = a.CreationTime,
                            CategroyName = b.Name,
                            RelationUserName = u == null ? "" : u.Name

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_CWDetailListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_CWDetailOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_CWDetailOutputDto>();
        }
        /// <summary>
        /// 添加一个B_CWDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_CWDetailInput input)
        {
            var newmodel = new B_CWDetail()
            {
                UserId = input.UserId,
                RelationUserId = input.RelationUserId,
                Type = input.Type,
                BusinessType = input.BusinessType,
                CategroyId = input.CategroyId,
                Number = input.Number,
                IsDefault = input.IsDefault
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个B_CWDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_CWDetailInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.UserId = input.UserId;
            //    dbmodel.RelationUserId = input.RelationUserId;
            //    dbmodel.Type = input.Type;
            //    dbmodel.BusinessType = input.BusinessType;
            //    dbmodel.CategroyId = input.CategroyId;
            //    dbmodel.Number = input.Number;
            //    dbmodel.IsDefault = input.IsDefault;

            //    await _repository.UpdateAsync(dbmodel);

            //}
            //else
            //{
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //}
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