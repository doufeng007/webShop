using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Train
{
    public interface ITrainUserExperienceAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<TrainUserExperienceSumOutputDto>> GetList(GetTrainUserExperienceListInput input);


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
        Task<TrainUserExperienceOutputDto> Get(NullableIdDto<Guid> input);
        Task<TrainUserExperienceOutputDto> GetIsExistence(GetTrainUserExperienceInput input);
        /// <summary>
        /// 添加一个TrainUserExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateTrainUserExperienceInput input);

		/// <summary>
        /// 修改一个TrainUserExperience
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateTrainUserExperienceInput input);

        /// <summary>
        /// 心得是否被采用
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task UpdateUse(TrainUserExperienceUseInput input);
        Task<List<TrainUserExperienceOrganListOutputDto>> GetOrganList(GetTrainUserExperienceInput input);
        Task<PagedResultDto<TrainUserExperienceListOutputDto>> GetListByUser(GetTrainUserExperienceListInput input);

    }
}