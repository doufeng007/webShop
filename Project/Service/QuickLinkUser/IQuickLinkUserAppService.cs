using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Project
{
    public interface IQuickLinkUserAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<QuickLinkUserListOutputDto>> GetList(GetQuickLinkUserListInput input);



        Task<PagedResultDto<QuickLinkWithUserOutputDto>> GetAllList(GetQuickLinkUserListInput input);



        Task Save(List<Guid> input);
    }
}