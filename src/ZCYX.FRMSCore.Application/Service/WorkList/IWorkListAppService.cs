using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IWorkListAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<WorkListListOutputDto>> GetList(GetWorkListListInput input);


		/// <summary>
        /// 添加一个WorkList
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateWorkListInput input);

    }
}