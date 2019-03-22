using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace MeetingGL
{
    public interface IXZGLMeetingRoomAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="input">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<XZGLMeetingRoomListOutputDto>> GetList(GetXZGLMeetingRoomListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<XZGLMeetingRoomOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个XZGLMeetingRoom
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateXZGLMeetingRoomInput input);

		/// <summary>
        /// 修改一个XZGLMeetingRoom
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(XZGLMeetingRoomUpdateInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 会议室启用或禁用
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Enable(EntityDto<Guid> input);


        Task<PagedResultDto<XZGLMeetingRoomTimeListOutput>> GetTimeList();
    }
}