using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    public interface IEmployeeReceiptV2AppService : IApplicationService
    {
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeReceiptV2ListOutputDto>> GetList(GetEmployeeReceiptV2ListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		EmployeeReceiptV2OutputDto Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个EmployeeReceiptV2
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		InitWorkFlowOutput Create(CreateEmployeeReceiptV2Input input);

        /// <summary>
        /// 修改一个EmployeeReceiptV2
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
         Task Update(UpdateEmployeeReceiptV2Input input);
    }
}