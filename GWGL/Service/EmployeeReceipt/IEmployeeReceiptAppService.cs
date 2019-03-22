using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace GWGL
{
    public interface IEmployeeReceiptAppService : IApplicationService
    {
        /// <summary>
        /// 获取公文属性枚举
        /// </summary>
        /// <param name="value"></param>
        /// <param name="setEmpty"></param>
        /// <returns></returns>
        List<ExpandoObject> GetReceiptDocProperty(string value = null, string setEmpty = null);
        /// <summary>
        /// 获取密级枚举
        /// </summary>
        /// <param name="value"></param>
        /// <param name="setEmpty"></param>
        /// <returns></returns>
        List<ExpandoObject> GetReceiptRankProperty(string value = null, string setEmpty = null);
        /// <summary>
        /// 获取紧急程度枚举
        /// </summary>
        /// <param name="value"></param>
        /// <param name="setEmpty"></param>
        /// <returns></returns>
        List<ExpandoObject> GetEmergencyDegreeProperty(string value = null, string setEmpty = null);
        /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
        Task<PagedResultDto<EmployeeReceiptListOutputDto>> GetList(GetEmployeeReceiptListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<EmployeeReceiptOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个EmployeeReceipt
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<EmployeeReceiptOutput> Create(CreateEmployeeReceiptInput input);

		/// <summary>
        /// 修改一个EmployeeReceipt
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task Update(UpdateEmployeeReceiptInput input);
        Task CreateWrite(EmployeeReceiptAddWriteInput input);
        Task<WorkFlowCustomEventParams> EnableTask(EntityDto<Guid> input);
        Task UpdateCopyFor(UpdateEmployeeReceiptInput input);
        Task<long> GetNextUserId(EmployeeReceiptNextInput input);
    }
}