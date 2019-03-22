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
    public class GW_GWTemplateAppService : FRMSCoreAppServiceBase, IGW_GWTemplateAppService
    {
        private readonly IRepository<GW_GWTemplate, Guid> _repository;

        public GW_GWTemplateAppService(IRepository<GW_GWTemplate, Guid> repository

        )
        {
            this._repository = repository;

        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<GW_GWTemplateListOutputDto>> GetList(GetGW_GWTemplateListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.CreatorUserId equals b.Id
                        select new GW_GWTemplateListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Content = a.Content,
                            CreationTime = a.CreationTime,
                            CreateUserName = b.Name

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<GW_GWTemplateListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<GW_GWTemplateOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<GW_GWTemplateOutputDto>();
        }
        /// <summary>
        /// 添加一个GW_GWTemplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateGW_GWTemplateInput input)
        {
            if (_repository.GetAll().Any(r => r.Title == input.Title))
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "标题重复。");

            var newmodel = new GW_GWTemplate()
            {
                Title = input.Title,
                Content = input.Content
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个GW_GWTemplate
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateGW_GWTemplateInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }
                if (_repository.GetAll().Any(r => r.Id != input.Id && r.Title == input.Title))
                    throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "标题重复。");

                dbmodel.Title = input.Title;
                dbmodel.Content = input.Content;

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