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
using System.ComponentModel;
using ZCYX.FRMSCore.Model;

namespace MeetingGL
{
    public class XZGLMeetingRoomAppService : FRMSCoreAppServiceBase, IXZGLMeetingRoomAppService
    {
        private readonly IRepository<XZGLMeetingRoom, Guid> _repository;

        private readonly IRepository<XZGLMeeting, Guid> _meetingRepository;
        private readonly IRepository<MeetingRoomUseInfo, Guid> _meetingRoomUseInfoRepository;

        public XZGLMeetingRoomAppService(IRepository<XZGLMeetingRoom, Guid> repository, IRepository<XZGLMeeting, Guid> meetingRepository
            , IRepository<MeetingRoomUseInfo, Guid> meetingRoomUseInfoRepository)
        {
            this._repository = repository;
            this._meetingRepository = meetingRepository;
            _meetingRoomUseInfoRepository = meetingRoomUseInfoRepository;
        }

        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="input">查询实体</param>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLMeetingRoomListOutputDto>> GetList(GetXZGLMeetingRoomListInput input)
        {
            var dbmodel = _repository.GetAll().FirstOrDefault();
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted)
                        join b in UserManager.Users on a.UserId equals b.Id into g
                        from b in g.DefaultIfEmpty()
                        let meeting = (from b in _meetingRepository.GetAll().Where(x => !x.IsDeleted && x.RoomId == a.Id) select b)
                        select new XZGLMeetingRoomListOutputDto()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Address = a.Address,
                            IsEnable = a.IsEnable,
                            MeetingCount = meeting.Count(),
                            CreationTime = a.CreationTime,
                            MeetingRoomSizeType = a.MeetingRoomSizeType == MeetingRoomSizeType.L ? "大型" : a.MeetingRoomSizeType == MeetingRoomSizeType.M ? "中型" : "小型",
                            Number = a.Number,
                            AdminUserName = b == null ? "" : b.Name,
                            BookingStatus = a.BookingStatus,
                        };
            query = query.WhereIf(input.IsEnable.HasValue, r => r.IsEnable == input.IsEnable.Value);
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).PageBy(input).ToListAsync();
            return new PagedResultDto<XZGLMeetingRoomListOutputDto>(toalCount, ret);
        }



        ///// <summary>
        ///// 根据条件分页获取列表
        ///// </summary>
        ///// <returns></returns>
        //public async Task<PagedResultDto<XZGLMeetingRoomTimeListOutput>> GetTimeList()
        //{
        //    var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.IsEnable)

        //                select new XZGLMeetingRoomTimeListOutput()
        //                {
        //                    Name = a.Name,
        //                    Id = a.Id,
        //                    CreationTime = a.CreationTime

        //                };
        //    var toalCount = await query.CountAsync();
        //    var ret = await query.OrderByDescending(r => r.CreationTime).ToListAsync();
        //    foreach (var item in ret)
        //    {
        //        var list = _meetingRepository.GetAll().Where(r => r.RoomId == item.Id && r.Status != -2).Select(r =>
        //               new XZGLMeetingListTimeOutput()
        //               {
        //                   Id = r.Id,
        //                   Type = MeetingRoomType.Meeting,
        //                   StartTime = r.StartTime,
        //                   EndTime = r.EndTime
        //               }).ToList();
        //        list.AddRange(_trainRepository.GetAll().Where(r => r.MeetingRoomId == item.Id && r.Status != -2).Select(r =>
        //                 new XZGLMeetingListTimeOutput()
        //                 {
        //                     Id = r.Id,
        //                     Type = MeetingRoomType.Train,
        //                     StartTime = r.StartTime,
        //                     EndTime = r.EndTime
        //                 }).ToList());
        //        item.Times = list;
        //    }
        //    return new PagedResultDto<XZGLMeetingRoomTimeListOutput>(toalCount, ret);
        //}



        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>

        public async Task<XZGLMeetingRoomOutputDto> Get(NullableIdDto<Guid> input)
        {

            var model = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id.Value);
            if (model == null)
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var ret = model.MapTo<XZGLMeetingRoomOutputDto>();
            if (ret.UserId.HasValue)
                ret.AdminUserName = (await UserManager.GetUserByIdAsync(ret.UserId.Value)).Name;
            ret.MeetingRoomSizeTypeName = ret.MeetingRoomSizeType.ToString();
            return ret;
        }

        /// <summary>
        /// 添加一个XZGLMeetingRoom
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>

        public async Task Create(CreateXZGLMeetingRoomInput input)
        {
            if (_repository.GetAll().Any(x => x.Name == input.Name))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该会议室已存在。");
            }
            if (_repository.GetAll().Any(x => x.Address == input.Address))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该会议室位置已存在。");
            }
            var newmodel = new XZGLMeetingRoom()
            {
                Name = input.Name,
                Address = input.Address,
                MeetingRoomSizeType = input.MeetingRoomSizeType,
                Number = input.Number,
                IsEnable = input.IsEnable,
                UserId = input.UserId
            };
            await _repository.InsertAsync(newmodel);

        }

        /// <summary>
        /// 修改一个XZGLMeetingRoom
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task Update(XZGLMeetingRoomUpdateInput input)
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
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该会议室已存在。");
                }
                if (_repository.GetAll().Any(x => x.Address == input.Address && x.Id != input.Id))
                {
                    throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该会议室位置已存在。");
                }
                dbmodel.Name = input.Name;
                dbmodel.Address = input.Address;
                dbmodel.MeetingRoomSizeType = input.MeetingRoomSizeType;
                dbmodel.Number = input.Number;
                dbmodel.UserId = input.UserId;
                dbmodel.IsEnable = input.IsEnable;


                await _repository.UpdateAsync(dbmodel);
            }
            else
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
        }

        /// <summary>
        /// 会议室启用或禁用
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Enable(EntityDto<Guid> input)
        {
            var dbmodel = _repository.Get(input.Id);
            dbmodel.IsEnable = !dbmodel.IsEnable;
            await _repository.UpdateAsync(dbmodel);
        }
        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		public async Task Delete(EntityDto<Guid> input)
        {
            var model = await _repository.GetAsync(input.Id);
            if (model.BookingStatus == 1)
                throw new UserFriendlyException((int)ErrorCode.BussinessDataException, "使用中的会议室不能删除。");
            await _repository.DeleteAsync(x => x.Id == input.Id);
        }


        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<XZGLMeetingRoomTimeListOutput>> GetTimeList()
        {
            var query = from a in _repository.GetAll().Where(x => !x.IsDeleted && x.IsEnable)

                        select new XZGLMeetingRoomTimeListOutput()
                        {
                            Name = a.Name,
                            Id = a.Id,
                            CreationTime = a.CreationTime

                        };
            var toalCount = await query.CountAsync();
            var ret = await query.OrderByDescending(r => r.CreationTime).ToListAsync();
            foreach (var item in ret)
            {
                var query_UseInfo = from a in _meetingRoomUseInfoRepository.GetAll()
                                    where a.MeetingRoomId == item.Id
                                    select new XZGLMeetingListTimeOutput
                                    {
                                        Id = a.Id,
                                        BusinessId = a.BusinessId,
                                        StartTime = a.StartTime,
                                        EndTime = a.EndTime,
                                        Type = a.BusinessType
                                    };

                item.Times = query_UseInfo.ToList();
            }
            return new PagedResultDto<XZGLMeetingRoomTimeListOutput>(toalCount, ret);
        }
    }
}