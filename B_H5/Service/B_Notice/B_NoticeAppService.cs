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
    /// <summary>
    /// 公司消息
    /// </summary>
    public class B_NoticeAppService : FRMSCoreAppServiceBase, IB_NoticeAppService
    {
        private readonly IRepository<B_Notice, Guid> _repository;
        private readonly IRepository<B_NoticeUserReadLog, Guid> _b_NoticeUserReadLogRepository;

        public B_NoticeAppService(IRepository<B_Notice, Guid> repository, IRepository<B_NoticeUserReadLog, Guid> b_NoticeUserReadLogRepository

        )
        {
            this._repository = repository;
            _b_NoticeUserReadLogRepository = b_NoticeUserReadLogRepository;

        }

        /// <summary>
        /// 后台-获取公告列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<B_NoticeListOutputDto>> GetList(GetB_NoticeListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        select new B_NoticeListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Status = a.Status,
                            CreationTime = a.CreationTime
                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_NoticeListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// H5 获取公告列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<PagedResultDto<B_NoticeListOutputDto>> GetListForWx(GetB_NoticeListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_NoticeUserReadLogRepository.GetAll() on new { a.Id, UserId = AbpSession.UserId.Value } equals new { Id = b.NoticeId, UserId = b.UserId } into g
                        from c in g.DefaultIfEmpty()
                        where a.Status == NoticeStatusEnum.正常
                        select new B_NoticeListOutputDto()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            IsRead = c == null ? false : true
                        };
            var totalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<B_NoticeListOutputDto>(totalCount, ret);
        }


        /// <summary>
        /// H5 获取用户未读消息数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task<int> GetListForWxNotRead()
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _b_NoticeUserReadLogRepository.GetAll() on new { a.Id, UserId = AbpSession.UserId.Value } equals new { Id = b.NoticeId, UserId = b.UserId } into g
                        from c in g.DefaultIfEmpty()
                        where a.Status == NoticeStatusEnum.正常
                        select new B_NoticeListOutputDto()
                        {
                            Id = a.Id,
                            IsRead = c == null ? false : true
                        };
            var totalCount = await query.Where(r => r.IsRead == false).CountAsync();
            return totalCount;
        }


        /// <summary>
        /// 第一次打开公告链接时调用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize]
        public async Task ReadNotice(EntityDto<Guid> input)
        {
            var model = await _repository.GetAsync(input.Id);
            if (_b_NoticeUserReadLogRepository.GetAll().Any(r => r.Id == model.Id && r.UserId == AbpSession.UserId.Value))
            {
                return;
            }
            else
            {
                await _b_NoticeUserReadLogRepository.InsertAsync(new B_NoticeUserReadLog()
                {
                    Id = Guid.NewGuid(),
                    NoticeId = model.Id,
                    UserId = AbpSession.UserId.Value
                });
            }

        }


        /// <summary>
        /// 获取公告详情
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<B_NoticeOutputDto> Get(EntityDto<Guid> input)
        {
            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<B_NoticeOutputDto>();
        }
        /// <summary>
        /// 添加一个公告
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateB_NoticeInput input)
        {
            var newmodel = new B_Notice()
            {
                Title = input.Title,
                Content = input.Content,
                Status = NoticeStatusEnum.草稿
            };
            await _repository.InsertAsync(newmodel);
        }

        /// <summary>
        /// 修改一个公告
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateB_NoticeInput input)
        {
            //if (input.Id != Guid.Empty)
            //{
            //    var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            //    if (dbmodel == null)
            //    {
            //        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //    }

            //    dbmodel.Title = input.Title;
            //    dbmodel.Content = input.Content;
            //    dbmodel.Status = input.Status;

            //    await _repository.UpdateAsync(dbmodel);

            //}
            //else
            //{
            //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            //}
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            if (dbmodel.Status != NoticeStatusEnum.正常)
                await _repository.DeleteAsync(x => x.Id == input.Id);
        }



        /// <summary>
        /// 发送、撤销公告
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendOrCancle(EntityDto<Guid> input)
        {
            var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (dbmodel == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            if (dbmodel.Status == NoticeStatusEnum.草稿)
                dbmodel.Status = NoticeStatusEnum.正常;
            else if (dbmodel.Status == NoticeStatusEnum.正常)
                dbmodel.Status = NoticeStatusEnum.撤销;
            else { }

            await _repository.UpdateAsync(dbmodel);
        }
    }
}