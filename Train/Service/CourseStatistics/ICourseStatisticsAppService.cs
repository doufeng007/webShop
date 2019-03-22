using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Train
{
    public interface ICourseStatisticsAppService : IApplicationService
    {
        /// <summary>
        /// 获取统计指标-学员统计
        /// </summary>
        /// <returns></returns>
        Task<RecommendedIndexOutputDto> GetRecommendedIndexByUser();


        /// <summary>
        /// 根据部门统计学员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DepartmentOutputDto>> GetDepartmentCourseStatistics(DepartmentInput input);

        /// <summary>
        /// 根据学员id获取学员课程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DepartmentUserOutputDto>> GetDepartmentUserCourseStatistics(DepartmentUserInput input);
        
        /// <summary>
        /// 根据部门获取统计图表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DepartmentImageOutputDto>> GetRecommendedIndexImageByUser(DepartmentImageInput input);

        /// <summary>
        /// 获取统计指标-课程统计
        /// </summary>
        /// <returns></returns>
        Task<RecommendedIndexCourseOutputDto> GetRecommendedIndexByCourse();

        /// <summary>
        /// 根据课程分类获取统计图表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<CourseTypeImageOutputDto>> GetRecommendedIndexImageByCourse(CourseTypeImageInput input);

        /// <summary>
        /// 根据课程分类统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CourseTypeOutputDto>> GetCourseTypeStatistics(CourseTypeInput input);



        /// <summary>
        /// 根据课程id获取学员课程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CourseUserOutputDto>> GetUserCourseStatistics(CourseUserInput input);

        /// <summary>
        /// 课程绩效统计-前端
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<FrontEndCourseScoreOutputDto>> GetFrontEndCourseScoreStatistics(FrontEndCourseScoreInput input);
    }
}
