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
    public class B_OrderAppService : FRMSCoreAppServiceBase, IB_OrderAppService
    {
        private readonly IRepository<B_Order, Guid> _repository;
        private readonly IRepository<B_OrderIn, Guid> _b_OrderInRepository;

        public B_OrderAppService(IRepository<B_Order, Guid> repository , IRepository<B_OrderIn, Guid> b_OrderInRepository

        )
        {
            this._repository = repository;
            _b_OrderInRepository = b_OrderInRepository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_OrderListOutputDto>> GetList(GetB_OrderListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)

                        select new B_OrderListOutputDto()
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Amout = a.Amout,
                            Stauts = a.Stauts,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_OrderListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_OrderOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_OrderOutputDto>();
        }
        /// <summary>
        /// 添加一个B_Order
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task CreateAsync(CreateB_OrderInput input)
        {
            var newmodel = new B_Order()
            {
                UserId = input.UserId,
                Amout = input.Amout,
                Stauts = input.Stauts,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                InOrOut = input.InOrOut,
                OrderNo = input.OrderNo,
            };

            await _repository.InsertAsync(newmodel);

        }


        public void Create(CreateB_OrderInput input)
        {
            var newmodel = new B_Order()
            {
                UserId = input.UserId,
                Amout = input.Amout,
                Stauts = input.Stauts,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                InOrOut = input.InOrOut,
                OrderNo = input.OrderNo,
            };
            _repository.Insert(newmodel);

        }

        /// <summary>
        /// 修改一个B_Order
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_OrderInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.UserId = input.UserId;
            //    dbmodel.Amout = input.Amout;
            //    dbmodel.Stauts = input.Stauts;

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
            //await _repository.DeleteAsync(x => x.Id == input.Id);
        }


       
    }
}