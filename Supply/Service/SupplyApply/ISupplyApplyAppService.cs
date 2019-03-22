using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply.Service.SupplyApply
{
    /// <summary>
    /// 员工申请个人用品
    /// </summary>
    public interface ISupplyApplyAppService : IApplicationService
    {
        /// <summary>
        /// 申领个人用品
        /// </summary>
        InitWorkFlowOutput Create(CreateApplyDto input);
        void Update(CreateApplyDto input);


        /// <summary>
        ///  修改申领用品数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateOne(UpdateApplySubDto input);

        /// <summary>
        /// 新增申领用品明细数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOneSupplyApplySub(CreateOneSupplyApplySubInput input);

        Task<SupplyApplyDto> Get(GetWorkFlowTaskCommentInput input);



        Task<PagedResultDto<SupplyApplyListDto>> GetAll(GetSupplyApplyListInput input);

        /// <summary>
        /// 行政获取申领列表
        /// </summary>
        Task<PagedResultDto<SupplyApplyListDto>> GetMyAll(GetSupplyApplyListInput input);



        Task<PagedResultDto<SupplyApplyListDto>> GetMyAllV2(GetSupplyApplyListInput input);


        Task<PagedResultDto<SupplyApplySubBaseDto>> GetSupplySubsByMainId(GetSupplyApplySubListInput input);

        /// <summary>
        /// 获取个人申请列表
        /// </summary>
        Task<PagedResultDto<ApplyResultDto>> GetMy(PagedAndSortedInputDto input);

        /// <summary>
        /// 个人领取用品  对一个申领下的所有申领明细的领取
        /// </summary>
        /// <param name="input">对申领明细的领取</param>
        /// <returns></returns>
        Task Apply(Guid input);


        /// <summary>
        /// 个人领取用品  对一个申领下一个申领明细的领取
        /// </summary>
        /// <param name="subApplyId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>

        Task ApplyOne(Guid subApplyId, Guid resultId);

        /// <summary>
        /// 获取可被领用的用品（闲置状态的用品）
        /// </summary>
        Task<PagedResultDto<SupplySelectDto>> GetCanApplySupply(PagedAndSortedInputDto input);


        /// <summary>
        /// 行政发放用品
        /// </summary>
        Task Grant(GrantDto input);


        /// <summary>
        /// 判断申领处理结果，是否包含申购
        /// </summary>
        /// <param name="id">申领id</param>
        /// <returns></returns>
        bool HasSupplyApplyPurchase(Guid id);



        /// <summary>
        /// 采购子流程激活事件
        /// </summary>
        /// <param name="instacneId"></param>
        /// <returns></returns>
        string CreateSupplyPurchaseMainForSubFlow(Guid instacneId);



        void CreateSupplyPurchaseQD(Guid instacneId);



        /// <summary>
        /// 行政获取采购列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SupplyPurchaseListDto>> GetAllPurchase(GetSupplyPurchaseListInput input);


        /// <summary>
        /// 采购处理采购待办获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SupplyPurchaseMainDto> GetSupplyPurchase(GetWorkFlowTaskCommentInput input);



        Task UpdateSupplyPurchase(UpdateSupplyPurchaseInput input);


        Task<RegisterSupplyPurchaseOutput> RegisterSupplyPurchase(RegisterSupplyPurchaseInput input);



        /// <summary>
        /// 入库采购的用品 按采购计划 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateRegisterSupplyPurchaseAll(RegisterSupplyPurchasePlanInput input);


        /// <summary>
        ///  入库采购的用品 按采购计划 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateRegisterSupplyPurchaseAll(RegisterSupplyPurchasePlanInput input);


        /// <summary>
        /// 获取某一个采购计划的入库结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<RegisterSupplyPurchaseOutput>> GetSupplyPurchaseResult(GetSupplyPurchaseResultInput input);


        /// <summary>
        /// 采购完成后自定义事件
        /// </summary>
        /// <param name="instanceId"></param>
        void ChangeApplyStatusAfterPurchase(string instanceId);


        Task CreateOrUpdateSupplyPurchase(CreateOrUpdateSupplyPurchasePlanInput input);


        Task DeleteSupplyPurchase(EntityDto<Guid> input);


        /// <summary>
        /// 获取用品申领处理的申购清单
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<SupplyPurchaseQDFromApplyListOutput>> GetSupplyPurchaseQDFromApplyList(SupplyPurchaseQDFromApplyListInput input);



        /// <summary>
        /// 获取一次申领的申购清单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SupplyPurchasePlanDto>> GetSupplyPurchaseQDOne(SupplyPurchaseQDOneFromApplyListInput input);


        /// <summary>
        /// 加入采购计划
        /// </summary>
        /// <returns></returns>
        Task AddSupplyPurchase(AddSupplyPurchasePlanInput input);


        /// <summary>
        /// 批量新增采购计划
        /// </summary>
        /// <returns></returns>
        Task AddSupplyPurchases(List<CreateSupplyPurchasePlanInput> input);



        /// <summary>
        /// 提交采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InitWorkFlowOutput> SubmitSupplyPurchasePlans(SubmitSupplyPurchasePlansInput input);



        /// <summary>
        /// 加入采购计划时，创建采购流程实例
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        InitWorkFlowOutput CreateSupplyPruchasePlanInstance(CreateSupplyPruchasePlanInstanceInput input);



        /// <summary>
        /// 采购是否存在申领
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        bool IsSupplyPruchaseExitApply(string code);




        /// <summary>
        /// 获取采购里的用品申领人员
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [RemoteService(IsEnabled = false)]
        string FindSupplyPruchaseRecipientsUsers(string code);


        /// <summary>
        /// 编辑采购计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdateSupplyPurchasePlan(UpdateSupplyPurchasePlanInput input);



        /// <summary>
        /// 领导审批采购清单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task UpdaterPurchasePlanStatus(List<UpdaterPurchasePlanStatusInput> input);


        /// <summary>
        /// 采购用品领用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<SupplyApplyDto>> GetPlansForRecipients(EntityDto<string> input);


        /// <summary>
        /// 申领是否处理完成
        /// </summary>
        /// <returns></returns>
        bool IsComplateApply(Guid mainId);


        /// <summary>
        /// 采购是否入库完
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        ExecuteWorkFlowOutput IsComplatePutin(string instanceId);


        ExecuteWorkFlowOutput IsComplateAuditByZJL(string instanceId);

    }
}
