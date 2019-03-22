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
using Abp.Authorization;
using ZCYX.FRMSCore.Model;

namespace HR
{
    public class QuickLinkUserAppService : FRMSCoreAppServiceBase, IQuickLinkUserAppService
    {
        private readonly IRepository<QuickLinkUser, Guid> _repository;
        private readonly IRepository<QuickLinkBase, Guid> _baserepository;

        public QuickLinkUserAppService(IRepository<QuickLinkUser, Guid> repository, IRepository<QuickLinkBase, Guid> baserepository

        )
        {
            this._repository = repository;
            _baserepository = baserepository;


        }


        /// <summary>
        /// 获取当前用户的快捷入口
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<QuickLinkUserListOutputDto>> GetList(GetQuickLinkUserListInput input)
        {
            var query = from a in _repository.GetAll()
                        join b in _baserepository.GetAll() on a.QuickLinkId equals b.Id
                        join c in UserManager.Users on a.UserId equals c.Id
                        where c.Id == AbpSession.UserId
                        select new QuickLinkUserListOutputDto()
                        {
                            Id = a.Id,
                            QuickLinkId = a.QuickLinkId,
                            UserId = a.UserId,
                            Sort = a.Sort,
                            CreationTime = a.CreationTime,
                            Link = b.Link,
                            Name = b.Name,
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.Sort).PageBy(input).ToListAsync();

            return new PagedResultDto<QuickLinkUserListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 编辑用户快捷入口-获取数据
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<QuickLinkWithUserOutputDto>> GetAllList(GetQuickLinkUserListInput input)
        {
            var query = from a in _baserepository.GetAll()
                        join b in _repository.GetAll() on new { Id = a.Id, UserId = AbpSession.UserId.Value } equals new { Id = b.QuickLinkId, UserId = b.UserId } into g
                        from c in g.DefaultIfEmpty()
                        select new QuickLinkWithUserOutputDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Link = a.Link,
                            Sort = a.Sort,
                            IsSelect = c == null ? false : true
                        };

            var toalCount = await query.CountAsync();
            var ret = await query.OrderBy(r => r.Sort).PageBy(input).ToListAsync();

            return new PagedResultDto<QuickLinkWithUserOutputDto>(toalCount, ret);

        }
        /// <summary>
        /// 添加一个QuickLinkUser
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Save(List<Guid> input)
        {
            if (input.Count == 0)
                throw new UserFriendlyException((int)ErrorCode.NullPropertyCodeErr, "未选择快捷入口");
            await _repository.DeleteAsync(r => r.UserId == AbpSession.UserId.Value);
            var sort = 0;
            foreach (var item in input)
            {
                if (_baserepository.GetAll().Any(r => r.Id == item))
                {
                    await _repository.InsertAsync(new QuickLinkUser() { Id = Guid.NewGuid(), QuickLinkId = item, Sort = sort, UserId = AbpSession.UserId.Value });
                    sort++;
                }
            }


        }

       
    }
}