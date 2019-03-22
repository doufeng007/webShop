using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAll
{
    public interface ISearchAllAppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<SearchAllListOutputDto>> GetList(GetSearchAllListInput input);
        Task Create(SearchInput input);
        Task CreateOffice(Search input);
        Task Update(SearchInput input);
        Task Delete(EntityDto<Guid> input);
    }
}