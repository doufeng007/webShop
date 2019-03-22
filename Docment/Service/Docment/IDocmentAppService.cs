using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Docment
{
    public  interface IDocmentAppService: IApplicationService
    {
        /// <summary>
        /// 创建归档申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task< InitWorkFlowOutput> Create(CreateDocmentInput input);
        /// <summary>
        /// 更新归档申请
        /// </summary>
        /// <param name="input"></param>
        Task Update(UpdateDocmentInput input);
        /// <summary>
        /// 接受流转档案
        /// </summary>
        /// <param name="docmentId"></param>
        /// <returns></returns>
        Task GetFlowingDocment(Guid docmentId);
        /// <summary>
        /// 获取档案详情
        /// </summary>
        /// <param name="docmentId"></param>
        /// <returns></returns>
        Task<DocmentDto> GetDetail(Guid qrcodeIid);
        /// <summary>
        /// 获取归档申请详情
        /// </summary>
        /// <returns></returns>
        Task<DocmentDto> Get(GetWorkFlowTaskCommentInput input);
        /// <summary>
        /// 获取档案列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DocmentListDto>> GetAll(DocmentSearchInput input);
        Task<PagedResultDto<DocmentListDto>> GetAllForWorkflow(DocmentSearchInput input);
        Guid Add(CreateDocmentInput input);
        Task<PagedResultDto<DocmentListDto>> GetAllForWait(DocmentSearchInput input);
        Task Delete(Guid input);

        /// <summary>
        /// 批量移交档案
        /// </summary>
        /// <param name="docmentIds"></param>
        /// <returns></returns>
        Task Moves(List<Guid> docmentIds);

        /// <summary>
        /// 批量销毁档案
        /// </summary>
        /// <param name="docmentIds"></param>
        /// <returns></returns>
        Task Destories(List<Guid> docmentIds);
        /// <summary>
        /// 档案导入
        /// </summary>
        /// <param name="fildId"></param>
        /// <returns></returns>
        Task<int> Export(Guid fildId);
        /// <summary>
        /// 其他流程发起档案归档
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> CreateDocment(DocmentInput input);

        void ApplyStorge(ApplyDocmentInput input);
        Task ApplyStorgeAgree(Guid qrcodeId);
        /// <summary>
        /// 当前用户是否领导
        /// </summary>
        /// <returns></returns>
        bool IsLeader();
        Task ApplyStorgeForShouwen(Guid QrCodeId);

        /// <summary>
        /// 档案袋流转-公文顺序传阅（电子档），
        /// </summary>
        /// <param name="docmentid"></param>
        /// <returns></returns>
        Task AutoFlowingDocment(Guid qrCodeId, string userid);

    }
}
