using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment
{
    public interface IDocmentBorrowAppService: IApplicationService
    {
        /// <summary>
        /// 创建借阅申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> Create(CreateDocmentBorrowInput input);
        /// <summary>
        /// 更新借阅申请
        /// </summary>
        /// <param name="input"></param>
        Task Update(UpdateeDocmentBorrowInput input);
        /// <summary>
        /// 获取借阅申请详情
        /// </summary>
        /// <returns></returns>
        Task<DocmentBorrowDto> Get(GetWorkFlowTaskCommentInput input);
        /// <summary>
        /// 获取借阅记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocmentBorrowListDto>> GetAll(DocmentBorrowSearchInput input);
        /// <summary>
        /// 获取借阅验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetVerify(Guid id);
        /// <summary>
        /// 档案管理员作废申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task StopBorrow(Guid id, Guid flowerid, string reason);
        /// <summary>
        /// 扫码确认领取
        /// </summary>
        /// <param name="docmengId"></param>
        /// <returns></returns>
        Task<bool> SureGet(BorrowBackInput input);
        /// <summary>
        /// 扫码确认归还
        /// </summary>
        /// <param name="docmengId"></param>
        /// <returns></returns>
        Task<bool> SureBack(Guid qrCodeId);
        /// <summary>
        /// 领导同意借出
        /// </summary>
        /// <param name="docmengId"></param>
        /// <param name="borrowId"></param>
        /// <param name="agree"></param>
        /// <returns></returns>
        Task<bool> Agree(AgreeInput input);
        /// <summary>
        /// 档案管理员借出
        /// </summary>
        /// <param name="docmengId"></param>
        /// <param name="borrowId"></param>
        /// <returns></returns>
        Task<bool> SureBorrow(AgreeInput input);
        /// <summary>
        /// 扫码领取档案
        /// </summary>
        /// <param name="qrCodeId"></param>
        /// <returns></returns>
        Task<DocmentDto> ScanBorrowGet(Guid qrCodeId);

        Task<bool> SureIn(Guid qrCodeId);
    }
}
