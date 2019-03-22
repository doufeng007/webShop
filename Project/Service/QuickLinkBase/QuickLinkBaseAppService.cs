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
using ZCYX.FRMSCore.Model;

namespace Project
{
    public class QuickLinkBaseAppService : FRMSCoreAppServiceBase, IQuickLinkBaseAppService
    {
        private readonly IRepository<QuickLinkBase, Guid> _repository;

        public QuickLinkBaseAppService(IRepository<QuickLinkBase, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<QuickLinkBaseListOutputDto>> GetList(GetQuickLinkBaseListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new QuickLinkBaseListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Link = a.Link,
                            Remark = a.Remark,
                            CreationTime = a.CreationTime,
                            Sort = a.Sort,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.Sort).PageBy(input).ToListAsync();

            return new PagedResultDto<QuickLinkBaseListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<QuickLinkBaseOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
            return model.MapTo<QuickLinkBaseOutputDto>();
        }
        /// <summary>
        /// 添加一个QuickLinkBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateQuickLinkBaseInput input)
        {
            if (_repository.GetAll().Any(r => r.Name == input.Name))
                throw new UserFriendlyException((int)ErrorCode.DataDuplication, "名称重复");
            var newmodel = new QuickLinkBase()
            {
                Name = input.Name,
                Link = input.Link,
                Remark = input.Remark,
                Sort = input.Sort,
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个QuickLinkBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateQuickLinkBaseInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Name == input.Name))
                    throw new UserFriendlyException((int)ErrorCode.DataDuplication, "名称重复");
                dbmodel.Name = input.Name;
                dbmodel.Link = input.Link;
                dbmodel.Remark = input.Remark;
                dbmodel.Sort = input.Sort;
                await _repository.UpdateAsync(dbmodel);

            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在！");
            }
        }

        // <summary>
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