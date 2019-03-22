using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace IMLib
{
    public interface IIM_InquiryResultAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<IM_InquiryResultListOutputDto>> GetList(GetIM_InquiryResultListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<IM_InquiryResultOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个IM_InquiryResult
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateIM_InquiryResultInput input);

		/// <summary>
        /// 修改一个IM_InquiryResult
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateIM_InquiryResultInput input);

		/// <summary>
        /// 逻辑删除实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task Delete(EntityDto<Guid> input);
    }
}