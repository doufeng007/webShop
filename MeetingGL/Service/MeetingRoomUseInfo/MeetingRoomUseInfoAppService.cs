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
using Abp.Application.Services;

namespace MeetingGL
{
    public class MeetingRoomUseInfoAppService : FRMSCoreAppServiceBase, IMeetingRoomUseInfoAppService
    {
        private readonly IRepository<MeetingRoomUseInfo, Guid> _repository;
        private readonly IRepository<XZGLMeetingRoom, Guid> _xZGLMeetingRoomRepository;

        public MeetingRoomUseInfoAppService(IRepository<MeetingRoomUseInfo, Guid> repository, IRepository<XZGLMeetingRoom, Guid> xZGLMeetingRoomRepository

        )
        {
            this._repository = repository;
            _xZGLMeetingRoomRepository = xZGLMeetingRoomRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<MeetingRoomUseInfoListOutputDto>> GetList(GetMeetingRoomUseInfoListInput input)
        {
            if (input.StartTime.HasValue && input.EndTime.HasValue && input.StartTime.Value >= input.EndTime.Value)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "会议开始时间不能超过会议结束时间");
            }
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in _xZGLMeetingRoomRepository.GetAll() on a.MeetingRoomId equals b.Id
                        join c in UserManager.Users on a.CreatorUserId.Value equals c.Id
                        select new MeetingRoomUseInfoListOutputDto()
                        {
                            Id = a.Id,
                            MeetingRoomId = a.MeetingRoomId,
                            BusinessName = a.BusinessName,
                            MeetingRoomName =b.Name,
                            BusinessId = a.BusinessId,
                            BusinessType = a.BusinessType,
                            StartTime = a.StartTime,
                            EndTime = a.EndTime,
                            Status = a.Status,
                            CreationTime = a.CreationTime,
                            CreateUserName = c.Name
                        };
            query = query.WhereIf(input.MeetingRoomId.HasValue, r => r.MeetingRoomId == input.MeetingRoomId)
                .WhereIf(input.StartTime.HasValue, r => r.StartTime >= input.StartTime.Value)
                .WhereIf(input.EndTime.HasValue, r => r.EndTime <= input.EndTime.Value).WhereIf(!string.IsNullOrEmpty(input.SearchKey),r=>r.BusinessName.Contains(input.SearchKey) || r.CreateUserName.Contains(input.SearchKey) || r.MeetingRoomName.Contains(input.SearchKey));
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<MeetingRoomUseInfoListOutputDto>(toalCount, ret);
        }

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<MeetingRoomUseInfoOutputDto> Get(EntityDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
            }
            var ret = model.MapTo<MeetingRoomUseInfoOutputDto>();
            ret.BusinessTypeName = ret.BusinessType.ToString();
            ret.CreateUserName = (await UserManager.GetUserByIdAsync(model.CreatorUserId.Value)).Name;
            return ret;
        }
        /// <summary>
        /// 添加一个MeetingRoomUseInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        [AbpAuthorize]
        public async Task Create(CreateMeetingRoomUseInfoInput input)
        {
            var newmodel = new MeetingRoomUseInfo()
            {
                MeetingRoomId = input.MeetingRoomId,
                BusinessId = input.BusinessId,
                BusinessType = input.BusinessType,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                BusinessName = input.BusinessName,
                Status = input.Status
            };


            var roomModel =await _xZGLMeetingRoomRepository.GetAsync(input.MeetingRoomId);
            roomModel.BookingStatus = 1;

            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个MeetingRoomUseInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(UpdateMeetingRoomUseInfoInput input)
        {
            if (input.Id != Guid.Empty)
            {
                var dbmodel = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                if (dbmodel == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "该数据不存在！");
                }

                dbmodel.MeetingRoomId = input.MeetingRoomId;
                dbmodel.BusinessId = input.BusinessId;
                dbmodel.BusinessName = input.BusinessName;
                dbmodel.BusinessType = input.BusinessType;
                dbmodel.StartTime = input.StartTime;
                dbmodel.EndTime = input.EndTime;
                dbmodel.Status = input.Status;

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
        [RemoteService(IsEnabled = false)]
        public MeetingRoomUseInfo GetMeetingRoomUseInfo(GetMeetingRoomUseInfoByInput input)
        {
            return _repository.GetAll().FirstOrDefault(x => x.MeetingRoomId == input.MeetingRoomId && x.BusinessId == input.BusinessId && x.BusinessType == input.BusinessType);
        }
    }
}