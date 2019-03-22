using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TaskGL.Enum;
using TaskGL.Service.TaskManagementStatistics.Dto;

namespace TaskGL.Service.TaskManagementStatistics
{
    public interface ITaskManagementStatisticsAppService : IApplicationService
    {
        /// <summary>
        /// 获取日常任务的统计
        /// </summary>
        /// <returns></returns>
        Task<TaskStatisticResponse> GetTaskStatistic();
        /// <summary>
        /// 获取与我相关的统计
        /// </summary>
        Task<List<MyTaskStatisticDetailResponse>> GetMyTaskStatistic();

        /// <summary>
        /// 获取下拉菜单
        /// </summary>
        /// <returns></returns>
        Task<List<dynamic>> GetStatisticDroplist();
        /// <summary>
        /// 人员统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TaskUserStatisticsResponse>> GetTaskUserStatistics(TaskUserStatisticsRequest input);
        /// <summary>
        /// 任务类型统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<dynamic>> GetTaskTypeStatistics(TaskUserStatisticsRequest input);
    }
}
