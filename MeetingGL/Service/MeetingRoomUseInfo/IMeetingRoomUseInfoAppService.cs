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
    public interface IMeetingRoomUseInfoAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<MeetingRoomUseInfoListOutputDto>> GetList(GetMeetingRoomUseInfoListInput input);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<MeetingRoomUseInfoOutputDto> Get(EntityDto<Guid> input);

        /// <summary>
        /// 添加一个MeetingRoomUseInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateMeetingRoomUseInfoInput input);

        /// <summary>
        /// 修改一个MeetingRoomUseInfo
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateMeetingRoomUseInfoInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);
        [RemoteService(IsEnabled = false)]
        MeetingRoomUseInfo GetMeetingRoomUseInfo(GetMeetingRoomUseInfoByInput input);
    }
}