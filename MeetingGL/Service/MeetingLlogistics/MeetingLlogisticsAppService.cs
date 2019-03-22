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

namespace MeetingGL
{
    public class MeetingLlogisticsAppService : FRMSCoreAppServiceBase, IMeetingLlogisticsAppService
    {
        private readonly IRepository<MeetingLlogistics, Guid> _repository;
        private readonly IRepository<MeetingTypeBase, Guid> _meetingTypeBaseRepository;

        public MeetingLlogisticsAppService(IRepository<MeetingLlogistics, Guid> repository, IRepository<MeetingTypeBase, Guid> meetingTypeBaseRepository

        )
        {
            this._repository = repository;
            _meetingTypeBaseRepository = meetingTypeBaseRepository;


        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<MeetingLlogisticsListOutputDto>> GetList(GetMeetingLlogisticsListInput input)
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _meetingTypeBaseRepository.GetAll() on a.MeetingTypeId equals b.Id into g
                        join c in UserManager.Users on a.UserId equals c.Id
                        from b in g.DefaultIfEmpty()
                        select new MeetingLlogisticsListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            MeetingTypeId = b == null ? Guid.Empty : b.Id,
                            MeetingTypeName = b == null ? "" : b.Name,
                            UserName = c.Name,
                            CreationTime = a.CreationTime,
                            UserId = c.Id,

                        };
            if (input.MeetingTypeId.HasValue)
                query = query.Where(r => r.MeetingTypeId == input.MeetingTypeId.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();

            return new PagedResultDto<MeetingLlogisticsListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<MeetingLlogisticsOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            return model.MapTo<MeetingLlogisticsOutputDto>();
        }
        /// <summary>
        /// 添加一个MeetingLlogistics
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateMeetingLlogisticsInput input)
        {
            var newmodel = new MeetingLlogistics()
            {
                Name = input.Name,
                MeetingTypeId = input.MeetingTypeId,
                UserId = input.UserId
            };

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个MeetingLlogistics
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateMeetingLlogisticsInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.Name = input.Name;
                dbmodel.MeetingTypeId = input.MeetingTypeId;
                dbmodel.UserId = input.UserId;

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