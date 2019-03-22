using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace CWGL
{
    public interface ICWGLPrePaymentAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CWGLPrePaymentListOutputDto>> GetList(GetCWGLPrePaymentListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CWGLPrePaymentOutputDto> Get(NullableIdDto<Guid> input);

		/// <summary>
        /// 添加一个CWGLPrePayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Create(CreateCWGLPrePaymentInput input);

		/// <summary>
        /// 修改一个CWGLPrePayment
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateCWGLPrePaymentInput input);

    }
}