using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Microsoft.AspNetCore.Mvc;
using Train.Service.Course.Dto;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public interface ICourseAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CourseListOutputDto>> GetList(GetCourseListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CourseOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个Course
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateCourseInput input);

		/// <summary>
        /// 修改一个Course
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateCourseInput input);

        /// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        [HttpPost]
        Task Delete(CourseDelInput input);
        /// <summary>
        /// 课程推荐成功后给推荐人发消息
        /// </summary>
        /// <param name="eventParams"></param>
        void SendMessageForRecommendUser(WorkFlowCustomEventParams eventParams);

        /// <summary>
        /// 获取周排行榜
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<CourseWeekRankOutputDto>> GetCourseWeekRank();
        /// <summary>
        /// 获取当前用户的周排行
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CourseWeekRankOutputDto> GetCourseWeekRankById(long userId);

        /// <summary>
        /// 获取我的推荐记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MyCourseRecommandOutput>> GetMyCourseRecommand(MyCourseRecommandInput input);

        /// <summary>
        /// 删除课程类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DelCourseType(Guid id);
    }
}