using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Train.Enum;

namespace Train
{
    public interface ICourseSettingAppService : IApplicationService
    {	
	    /// <summary>
        /// 获取视频配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		Task Set(CourseSet input);
        /// <summary>
        /// 设置视频设置
        /// </summary>
        /// <returns></returns>
        Task<CourseSet> Get();
        /// <summary>
        /// 根据必修选修专业难度返回对应的课程设置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isSpecial"></param>
        /// <returns></returns>
        Task<CourseSetScore> GetSetVal(CourseLearnType type, bool isSpecial);
    }
}