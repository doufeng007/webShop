using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Train.Enum;
using Train.Service.UserCourseRecord.Dto;

namespace Train
{
    public interface IUserCourseRecordAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<UserCourseRecordListOutputDto>> GetList(GetUserCourseRecordListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<UserCourseRecordOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个UserCourseRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateUserCourseRecordInput input);

		/// <summary>
        /// 修改一个UserCourseRecord
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateUserCourseRecordInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 获取我的课程积分统计
        /// </summary>
        /// <returns></returns>
        Task<UserCourseStatisticsOutputDto> GetMyStatisticsScore(bool refresh = false, long? userId = null);

        /// <summary>
        /// 执行减扣积分命令
        /// </summary>
        /// <param name="courseId"></param>
        Task DeductionScore(Guid courseId);
    }
}