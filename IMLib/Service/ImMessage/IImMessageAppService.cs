using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using SearchAll;

namespace IMLib
{
    public interface IImMessageAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<ImMessageListOutputDto>> GetList(GetImMessageListInput input);

        Task<List<ImSearchCount>> GetSearchList(GetImSearchInput input);
        Task<List<ImSearch>> GetListByIds(Guid[] input);
        /// <summary>
        /// 添加一个ImMessage
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Create(CreateImMessageInput input);

        Task Delete(EntityDto<Guid> input);
    }
}