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
    public interface ITrainSignInAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<TrainSignInListOutputDto>> GetList(GetTrainSignInListInput input);
        Task<PagedResultDto<TrainSignInListOutputDto>> GetListByTime(GetTrainSignInListByTimeInput input);

        /// <summary>
        /// 添加一个TrainSignIn
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task<TrainSignInOutputDto> SignIn(CreateTrainSignInInput input);

    }
}