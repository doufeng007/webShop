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
    public interface IMeetingTypeBaseAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<MeetingTypeBaseListOutputDto>> GetList(GetMeetingTypeBaseListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<MeetingTypeBaseOutputDto> Get(EntityDto<Guid> input);

		/// <summary>
        /// 添加一个MeetingTypeBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateMeetingTypeBaseInput input);

		/// <summary>
        /// 修改一个MeetingTypeBase
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateMeetingTypeBaseInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}