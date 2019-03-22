using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IOrganizationUnitPostsAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<OrganizationUnitPostsListOutputDto>> GetList(GetOrganizationUnitPostsBianzhiListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<OrganizationUnitPostsOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个OrganizationUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateOrganizationUnitPostsInput input);

		/// <summary>
        /// 修改一个OrganizationUnitPosts
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateOrganizationUnitPostsInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
        /// <summary>
        /// 创建默认领导岗位（对指定部门默认添加“分管领导”和“部门领导”岗位）
        /// </summary>
        /// <returns></returns>
        Task CreateDefaultPost(long? orgid);
    }
}