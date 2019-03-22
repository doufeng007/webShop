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
    public interface ICWGLBorrowMoneyAppService : IApplicationService
    {	
	    /// <summary>
        /// 根据条件分页获取列表
        /// </summary>
        /// <param name="page">查询实体</param>
        /// <returns></returns>
		Task<PagedResultDto<CWGLBorrowMoneyListOutputDto>> GetList(GetCWGLBorrowMoneyListInput input);

		/// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="input">主键</param>
        /// <returns></returns>
		Task<CWGLBorrowMoneyOutputDto> Get(GetWorkFlowTaskCommentInput input);

		/// <summary>
        /// 添加一个CWGLBorrowMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
		Task<InitWorkFlowOutput> Create(CreateCWGLBorrowMoneyInput input);



        Task<InitWorkFlowOutput> CreateTest();

        /// <summary>
        /// 修改一个CWGLBorrowMoney
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        Task Update(UpdateCWGLBorrowMoneyInput input);
        /// <summary>
        /// 获取备用金
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CWGLBorrowMoneyMyListOutputDto>> GetMyList(CWGLBorrowMoneyMyListInput input);

        /// <summary>
        /// 检查当前人员权限
        /// </summary>
        /// <returns></returns>
        bool CheckPostFromUser(Guid TaskId, int type);

        bool IsNeedCWCLAudit(Guid flowID, Guid groupID, string InstanceID);
        bool IsRole(Guid flowID, Guid groupID, string InstanceID, int Type, string Field = "CreatorUserId");
        bool IsCommonLoan(Guid flowID, string InstanceID);
        void UpdateIsPayBack(Guid flowID, string InstanceID, bool isPay);
    }
}