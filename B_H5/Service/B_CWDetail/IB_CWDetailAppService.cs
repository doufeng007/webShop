using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    public interface IB_CWDetailAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<B_CWDetailListOutputDto>> GetList(GetB_CWDetailListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<B_CWDetailOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个B_CWDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task CreateAsync(CreateB_CWDetailInput input);


        void Create(CreateB_CWDetailInput input);

		/// <summary>
        /// 修改一个B_CWDetail
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateB_CWDetailInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}