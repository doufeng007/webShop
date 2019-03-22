using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application.Dto;

namespace Train
{
    public interface IUserCourseExperienceAppService : IApplicationService
    {	

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<UserCourseExperienceOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个UserCourseExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateUserCourseExperienceInput input);

		/// <summary>
        /// 修改一个UserCourseExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateUserCourseExperienceInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 获取上传用户的领导
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        string GetCouserExperienceUserLeader(string InstanceID);

        /// <summary>
        /// 心得汇总课程列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CourseExperienceSummaryOutputDto>> GetAllCourseExperienceSummary(PagedAndSortedInputDto input);

        Task<PagedResultDto<CourseExperienceDetailOutputDto>> GetCourseExperienceDetail(
            CourseExperienceDetailInput input);
        /// <summary>
        /// 获取我的心得体会（业务表）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkFlowTrainUserExperienceOutputDto> GetMyExperience(GetTrainUserExperienceInput input);
    }
}