using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.MultiTenancy;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using Abp.WorkFlow.Entity;
using Project;
using Abp.File;
using HR;
using ZCYX.FRMSCore.Application;
using Supply;
using Supply.Entity;
using Docment;
using XZGL;
using CWGL;
using Train;
using ZCYX.FRMSCore.Entity;
using IMLib;
using EmailServer;
using GWGL;
using MeetingGL;
using TaskGL;

namespace ZCYX.FRMSCore.EntityFrameworkCore
{
    public class FRMSCoreDbContext : AbpZeroDbContext<Tenant, Role, User, FRMSCoreDbContext>
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<Core.BinaryObject> BinaryObject { get; set; }


        public virtual DbSet<AppLibrary> AppLibrary { get; set; }

        public virtual DbSet<WorkFlow> WorkFlow { get; set; }

        public virtual DbSet<WorkFlowModelColumn> WorkFlowModelColumn { get; set; }

        public virtual DbSet<WorkFlowModel> WorkFlowModel { get; set; }

        public virtual DbSet<WorkFlowTemplate> WorkFlowTemplate { get; set; }
        public virtual DbSet<WorkFlowTemplateLog> WorkFlowTemplateLog { get; set; }

        public virtual DbSet<WorkFlowForm> WorkFlowForm { get; set; }

        public virtual DbSet<WorkFlowButtons> WorkFlowButtons { get; set; }

        public virtual DbSet<WorkFlowCustomEvent> WorkFlowCustomEvent { get; set; }




        public virtual DbSet<TestTable> TestTable { get; set; }



        public virtual DbSet<WorkFlowTask> WorkFlowTask { get; set; }
        public virtual DbSet<WorkFlowModelColumnAuth> WorkFlowModelColumnAuth { get; set; }
        public virtual DbSet<WorkFlowOrganizationUnits> WorkFlowOrganizationUnit { get; set; }


        public virtual DbSet<WorkFlowUserOrganizationUnits> WorkFlowUserOrganizationUnits { get; set; }

        public virtual DbSet<PostInfo> PostInfo { get; set; }
        

        public virtual DbSet<OrganizationUnitPosts> OrganizationUnitPosts { get; set; }

        public virtual DbSet<OrganizationUnitPostsBase> OrganizationUnitPostsBase { get; set; }

        public virtual DbSet<EmployeeFinanceSalaryBill> EmployeeFinanceSalaryBill { get; set; }

        public virtual DbSet<UserPosts> UserPosts { get; set; }


        public virtual DbSet<DefaultMemberModel> DefaultMemberModel { get; set; }

        public virtual DbSet<SqlConditionResultModel> SqlConditionResultModel { get; set; }



        public virtual DbSet<AbpDictionary> AbpDictionary { get; set; }


        public virtual DbSet<Code_AppraisalType> Code_AppraisalType { get; set; }

        public virtual DbSet<AappraisalFileType> AappraisalFileType { get; set; }



        public virtual DbSet<ProjectBase> ProjectBase { get; set; }

        public virtual DbSet<ProjectBudget> ProjectBudget { get; set; }

        public virtual DbSet<ProjectBudgetControl> ProjectBudgetControl { get; set; }

        public virtual DbSet<ProjectFile> ProjectFile { get; set; }
        public virtual DbSet<ProjectQuestion> ProjectQuestion { get; set; }
        public virtual DbSet<ProjectSupplement> ProjectSupplement { get; set; }
        public virtual DbSet<ProjectProgressConfig> ProjectProgressConfig { get; set; }
        public virtual DbSet<ProjectProgressComplate> ProjectProgressComplate { get; set; }
        public virtual DbSet<ProjectProgressFault> ProjectProgressFault { get; set; }
        public virtual DbSet<ProjectAuditRole> ProjectAuditRole { get; set; }

        public virtual DbSet<ProjectAuditMember> ProjectAuditMember { get; set; }

        public virtual DbSet<ProjectAuditMemberResult> ProjectAuditMemberResult { get; set; }

        public virtual DbSet<ProjectPersentFinish> ProjectPersentFinish { get; set; }


        public virtual DbSet<ProjectPersentFinishAllot> ProjectPersentFinishAllot { get; set; }

        public virtual DbSet<OrganizationUnitPostsRole> OrganizationUnitPostsRole { get; set; }
        public virtual DbSet<ProjectPersentFinishResult> ProjectPersentFinishResult { get; set; }


        public virtual DbSet<SingleProjectInfo> SingleProjectInfo { get; set; }

        public virtual DbSet<SingleProjectFee> SingleProjectFee { get; set; }
        

        public virtual DbSet<DispatchMessage> DispatchMessage { get; set; }

        public virtual DbSet<ProjectAreas> ProjectAreas { get; set; }


        public virtual DbSet<NoticeLogs> NoticeLogs { get; set; }

        public virtual DbSet<NoticeTexts> NoticeTexts { get; set; }

        public virtual DbSet<ProjectAudit> ProjectAudit { get; set; }

        public virtual DbSet<ProjectInformationEnter> ProjectInformationEnter { get; set; }

        public virtual DbSet<ProjectWorkLog> ProjectWorkLog { get; set; }

        public virtual DbSet<ProjectRealationUser> ProjectRealationUser { get; set; }
        public virtual DbSet<ProjectRegistration> ProjectRegistration { get; set; }

        public virtual DbSet<ProjectWorkTask> ProjectWorkTask { get; set; }


        public virtual DbSet<ProjectListDto> ProjectListDto { get; set; }


        public virtual DbSet<ConstructionOrganizations> ConstructionOrganizations { get; set; }


        public virtual DbSet<UserFollowProject> UserFollowProject { get; set; }

        public virtual DbSet<AbpFile> AbpFile { get; set; }


        public virtual DbSet<AbpFileRelation> AbpFileRelation { get; set; }



        #region 任务
        public virtual DbSet<TaskManagementChange> TaskManagementChange { get; set; }
        public virtual DbSet<TaskManagement> TaskManagement { get; set; }
        public virtual DbSet<TaskManagementRelation> TaskManagementRelation { get; set; }

        public virtual DbSet<TaskManagementRecord> TaskManagementRecord { get; set; }


        #endregion







        public virtual DbSet<ArchivesFile> ArchivesFile { get; set; }


        public virtual DbSet<ArchivesManager> ArchivesManager { get; set; }

        public virtual DbSet<ProjectAuditGroup> ProjectAuditGroup { get; set; }


        public virtual DbSet<ProjectAuditGroupUser> ProjectAuditGroupUser { get; set; }


        public virtual DbSet<ProjectBudgetControlAuditResult> ProjectBudgetControlAuditResult { get; set; }

        public virtual DbSet<CJZFileCompareResult> CJZFileCompareResult { get; set; }

        public virtual DbSet<ProjectAuditStop> ProjectAuditStop { get; set; }

        public virtual DbSet<ProjcetAuditResultCheckRole> ProjcetAuditResultCheckRole { get; set; }

        public virtual DbSet<CJZFileCompareResultStatistics> CJZFileCompareResultStatistics { get; set; }

        public virtual DbSet<ChargeOrganizations> ChargeOrganizations { get; set; }

        public virtual DbSet<BusinessDepartment> BusinessDepartment { get; set; }


        public virtual DbSet<ReplyUnit> ReplyUnit { get; set; }


        public virtual DbSet<ProjectTodoListDtoNew> ProjectTodoListDtoNew { get; set; }


        public virtual DbSet<RealationSystem> RealationSystem { get; set; }


        public virtual DbSet<ContractWithSystem> ContractWithSystem { get; set; }

        public virtual DbSet<NoticeDocument> NoticeDocument { get; set; }

        public virtual DbSet<ProjectFileAdditional> ProjectFileAdditional { get; set; }


        


        public virtual DbSet<WorkFlowVersionNum> WorkFlowVersionNum { get; set; }


        public virtual DbSet<QuickLinkBase> QuickLinkBase { get; set; }


        public virtual DbSet<QuickLinkUser> QuickLinkUser { get; set; }





        #region HR

        public virtual DbSet<EmployeeTrainingSystem> EmployeeTrainingSystem { get; set; }
        public virtual DbSet<EmployeeTrainingSystemUnitPosts> EmployeeTrainingSystemUnitPosts { get; set; }
        public virtual DbSet<EmployeeProposal> EmployeeProposal { get; set; }
        public virtual DbSet<EmployeeReceiptV2> EmployeeReceiptV2 { get; set; }
        public virtual DbSet<CreateAccount> CreateAccount { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EducationExperience> EducationExperience { get; set; }
        public virtual DbSet<WorkExperience> WorkExperience { get; set; }
        public virtual DbSet<EmployeeFamily> EmployeeFamily { get; set; }
        public virtual DbSet<EmployeeDocment> EmployeeDocment { get; set; }
        public virtual DbSet<EmployeeRegular> EmployeeRegular { get; set; }
        public virtual DbSet<EmployeeResign> EmployeeResign { get; set; }

        public virtual DbSet<EmployeeEntrySlip> EmployeeEntrySlip { get; set; }

        public virtual DbSet<EmployeeSkill> EmployeeSkill { get; set; }

        public virtual DbSet<EmployeeSalaryBill> EmployeeSalaryBill { get; set; }
        public virtual DbSet<EmployeeRequire> EmployeeRequire { get; set; }
        public virtual DbSet<EmployeeGoOut> EmployeeGoOut { get; set; }

        public virtual DbSet<CommendResume> CommendResume { get; set; }

        public virtual DbSet<EmployeeBusinessTrip> EmployeeBusinessTrip { get; set; }


        public virtual DbSet<EmployeeBusinessTripMember> EmployeeBusinessTripMember { get; set; }

        public virtual DbSet<EmployeeBusinessTripTask> EmployeeBusinessTripTask { get; set; }

        public virtual DbSet<EmployeeWorkOvertime> EmployeeWorkOvertime { get; set; }


        public virtual DbSet<EmployeeWorkOvertimeMember> EmployeeWorkOvertimeMember { get; set; }

        public virtual DbSet<EmployeeSign> EmployeeSign { get; set; }

        public virtual DbSet<EmployeeAdjustPost> EmployeeAdjustPost { get; set; }

        public virtual DbSet<EmployeeOtherInfo> EmployeeOtherInfo { get; set; }
        public virtual DbSet<EmployeeAskForLeave> EmployeeAskForLeave { get; set; }

        public virtual DbSet<CollaborativeInstitutions> CollaborativeInstitutions { get; set; }

        public virtual DbSet<ExpertDataBase> ExpertDataBase { get; set; }

        public virtual DbSet<LegalAdviser> LegalAdviser { get; set; }


        public virtual DbSet<WorkRecord> WorkRecord { get; set; }
        public virtual DbSet<EmployeeReceipt> EmployeeReceipt { get; set; }


        public virtual DbSet<OrganizationUnitPostPlan> OrganizationUnitPostPlan { get; set; }

        public virtual DbSet<OrganizationUnitPostChangePlan> OrganizationUnitPostChangePlan { get; set; }
        
        public virtual DbSet<EmployeePlan> EmployeePlan { get; set; }

        public virtual DbSet<EmployeeResult> EmployeeResult { get; set; }
        public virtual DbSet<HrSystem> HrSystem { get; set; }
        public virtual DbSet<HrSystemRead> HrSystemRead { get; set; }

        public virtual DbSet<Performance> Performance { get; set; }
        public virtual DbSet<PerformanceAppealDetail> PerformanceAppealDetail { get; set; }
        public virtual DbSet<PerformanceScore> PerformanceScore { get; set; }
        public virtual DbSet<PerformanceScoreType> PerformanceScoreType { get; set; }
        public virtual DbSet<PerformanceAppeal> PerformanceAppeal { get; set; }

        #endregion

        #region 用品管理
        public virtual DbSet<SupplyBase> SupplyBase { get; set; }

        public virtual DbSet<UserSupply> UserSupply { get; set; }

        public virtual DbSet<SupplyScrapMain> SupplyScrapMain { get; set; }

        public virtual DbSet<SupplyScrapSub> SupplyScrapSub { get; set; }


        public virtual DbSet<SupplyApplyMain> SupplyApplyMain { get; set; }
        public virtual DbSet<SupplyApplySub> SupplyApplySub { get; set; }
        public virtual DbSet<SupplyApplySubBak> SupplyApplySubBak { get; set; }

        public virtual DbSet<SupplyApplyResult> SupplyApplyResult { get; set; }

        public virtual DbSet<SupplySupplier> SupplySupplier { get; set; }

        public virtual DbSet<SupplyBackMain> SupplyBackMain { get; set; }
        public virtual DbSet<SupplyBackSub> SupplyBackSub { get; set; }
        public virtual DbSet<SupplyRepair> SupplyRepair { get; set; }

        public virtual DbSet<SupplyPurchaseMain> SupplyPurchaseMain { get; set; }

        public virtual DbSet<SupplyPurchasePlan> SupplyPurchasePlan { get; set; }

        public virtual DbSet<SupplyPurchaseResult> SupplyPurchaseResult { get; set; }

        public virtual DbSet<UserMenuInit> UserMenuInit { get; set; }


        public virtual DbSet<CuringProcurement> CuringProcurement { get; set; }


        public virtual DbSet<CuringProcurementEdit> CuringProcurementEdit { get; set; }


        public virtual DbSet<CuringProcurementPlan> CuringProcurementPlan { get; set; }

            
        #endregion

        #region OA


        public virtual DbSet<OAContract> OAContract { get; set; }

        public virtual DbSet<OAReport> OAReport { get; set; }

        public virtual DbSet<OAMeeting> OAMeeting { get; set; }

        public virtual DbSet<OAContact> OAContact { get; set; }
        public virtual DbSet<OAMeetingUser> OAMeetingUser { get; set; }

        public virtual DbSet<OAPay> OAPay { get; set; }

        public virtual DbSet<OABorrow> OABorrow { get; set; }

        public virtual DbSet<OAProgressFundDeclare> OAProgressFundDeclare { get; set; }

        public virtual DbSet<OAContractCollectionFee> OAContractCollectionFee { get; set; }

        public virtual DbSet<OABirthday> OABirthday { get; set; }
        public virtual DbSet<OACompletionSettlement> OACompletionSettlement { get; set; }

        public virtual DbSet<OATask> OATask { get; set; }


        public virtual DbSet<OATaskUser> OATaskUser { get; set; }

        public virtual DbSet<OAFixedAssets> OAFixedAssets { get; set; }

        public virtual DbSet<OAFixedAssetsPurchase> OAFixedAssetsPurchase { get; set; }

        public virtual DbSet<OAFixedAssetsUseApply> OAFixedAssetsUseApply { get; set; }


        public virtual DbSet<OAFixedAssetsReturn> OAFixedAssetsReturn { get; set; }

        public virtual DbSet<OAFixedAssetsScrap> OAFixedAssetsScrap { get; set; }


        public virtual DbSet<OAFixedAssetsRepair> OAFixedAssetsRepair { get; set; }


        public virtual DbSet<OAEmployee> OAEmployee { get; set; }

        public virtual DbSet<OAPerformance> OAPerformance { get; set; }
        public virtual DbSet<OAPerformanceMain> OAPerformanceMain { get; set; }
        public virtual DbSet<OAWorkout> OAWorkout { get; set; }

        public virtual DbSet<OALeave> OALeave { get; set; }

        public virtual DbSet<OABuessOut> OABuessOut { get; set; }

        public virtual DbSet<OAWorkon> OAWorkon { get; set; }

        public virtual DbSet<OASignin> OASignin { get; set; }

        public virtual DbSet<OACar> OACar { get; set; }
        public virtual DbSet<OAUseCar> OAUseCar { get; set; }
        public virtual DbSet<OAPettyCash> OAPettyCash { get; set; }

        public virtual DbSet<OAFee> OAFee { get; set; }

        public virtual DbSet<OABidProject> OABidProject { get; set; }


        public virtual DbSet<OABidFilePurchase> OABidFilePurchase { get; set; }


        public virtual DbSet<OABidSelfAudit> OABidSelfAudit { get; set; }


        public virtual DbSet<OABidProjectCheck> OABidProjectCheck { get; set; }

        //public virtual IDbSet<>
        public virtual DbSet<OACustomer> OACustomer { get; set; }

        public virtual DbSet<OATenderAudit> OATenderAudit { get; set; }
        public virtual DbSet<OATenderCash> OATenderCash { get; set; }

        public virtual DbSet<OATenderEnemy> OATenderEnemy { get; set; }

        public virtual DbSet<OATenderBuess> OATenderBuess { get; set; }


        public virtual DbSet<AbpPermissionBase> AbpPermissionBase { get; set; }
        public virtual DbSet<AbpMenuBase> AbpMenuBase { get; set; }


        #endregion

        #region 档案管理

        public virtual DbSet<DocmentBorrow> DocmentBorrow { get; set; }
        public virtual DbSet<DocmentBorrowSub> DocmentBorrowSub { get; set; }
        public virtual DbSet<DocmentFlowing> DocmentFlowing { get; set; }

        public virtual DbSet<DocmentList> DocmentList { get; set; }

        public virtual DbSet<DocmentDestroy> DocmentDestroy { get; set; }

        public virtual DbSet<DocmentMove> DocmentMove { get; set; }
        #endregion


        #region 行政管理
        public virtual DbSet<XZGLCarInfo> XZGLCarInfo { get; set; }
        public virtual DbSet<XZGLCarRelation> XZGLCarRelation { get; set; }
        public virtual DbSet<XZGLCarUser> XZGLCarUser { get; set; }
        public virtual DbSet<XZGLCarUserInfo> XZGLCarUserInfo { get; set; }
        public virtual DbSet<XZGLCar> XZGLCar { get; set; }
        public virtual DbSet<XZGLCarBorrow> XZGLCarBorrow { get; set; }
        
        public virtual DbSet<XZGLCompany> XZGLCompany { get; set; }
        public virtual DbSet<XZGLMoney> XZGLMoney { get; set; }
        public virtual DbSet<XZGLProperty> XZGLProperty { get; set; }
        #endregion
        #region 财务管理
        public virtual DbSet<CWGLBorrowMoney> CWGLBorrowMoney { get; set; }
        public virtual DbSet<CWGLReimbursement> CWGLReimbursement { get; set; }
        public virtual DbSet<CWGLPayMoney> CWGLPayMoney { get; set; }
        public virtual DbSet<CWGLCredential> CWGLCredential { get; set; }
        public virtual DbSet<CWGLTravelReimbursement> CWGLTravelReimbursement { get; set; }
        public virtual DbSet<CWGLTravelReimbursementDetail> CWGLTravelReimbursementDetail { get; set; }
        public virtual DbSet<CWGLWagePay> CWGLWagePay { get; set; }
        public virtual DbSet<CWGLoan> CWGLoan { get; set; }
        public virtual DbSet<CWGLAdvanceCharge> CWGLAdvanceCharge { get; set; }
        public virtual DbSet<CWGLAdvanceChargeDetail> CWGLAdvanceChargeDetail { get; set; }
        public virtual DbSet<CWGLPrePayment> CWGLPrePayment { get; set; }
        public virtual DbSet<CWGLPrePaymentDetail> CWGLPrePaymentDetail { get; set; }
        public virtual DbSet<CWGLReceivable> CWGLReceivable { get; set; }
        public virtual DbSet<AccountantCourse> AccountantCourse { get; set; }

        public virtual DbSet<FinancialAccountingCertificate> FinancialAccountingCertificate { get; set; }

        public virtual DbSet<FACertificateDetail> FACertificateDetail { get; set; }

        public virtual DbSet<CWGLRepayment> CWGLRepayment { get; set; }


        public virtual DbSet<CW_PersonalTodo> CW_PersonalTodo { get; set; }


        #endregion
        #region 招聘
        public virtual DbSet<EmployeeProjecExperience> EmployeeProjecExperience { get; set; }
        public virtual DbSet<EmployeeResume> EmployeeResume { get; set; }
        public virtual DbSet<QuestionLibrary> QuestionLibrary { get; set; }
        #endregion
        #region 培训
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Lecturer> Lecturer { get; set; }
        public virtual DbSet<TrainLogistics> TrainLogistics { get; set; }
        public virtual DbSet<Train.Train> Train { get; set; }
        public virtual DbSet<Train.TrainUserExperience> TrainUserExperience { get; set; }
        public virtual DbSet<TrainLeave> TrainLeave { get; set; }
        public virtual DbSet<UserTrainExperience> UserTrainExperience { get; set; }
        public virtual DbSet<CourseSetting> CourseSetting { get; set; }
        public virtual DbSet<TrainSignIn> TrainSignIn { get; set; }
        public virtual DbSet<UserCourseComment> UserCourseComment { get; set; }
        public virtual DbSet<UserCourseRecord> UserCourseRecord { get; set; }
        public virtual DbSet<UserCourseRecordDetail> UserCourseRecordDetail { get; set; }
        public virtual DbSet<UserTrainScoreRecord> UserTrainScoreRecord { get; set; }
        public virtual DbSet<UserCourseRecordNumber> UserCourseRecordNumber { get; set; }
        public virtual DbSet<UserCourseExperience> UserCourseExperience { get; set; }

        #endregion
        #region 公文

        public virtual DbSet<GW_FWZHRole> GW_FWZHRole { get; set; }

        public virtual DbSet<GW_DocumentType> GW_DocumentType { get; set; }


        public virtual DbSet<GW_ComposeTemplate> GW_ComposeTemplate { get; set; }


        public virtual DbSet<GW_GWTemplate> GW_GWTemplate { get; set; }


        public virtual DbSet<Employees_Sign> Employees_Sign { get; set; }


        public virtual DbSet<GW_Seal> GW_Seal { get; set; }


        public virtual DbSet<Seal_Log> Seal_Log { get; set; }

        #endregion

        #region 会议
        public virtual DbSet<XZGLMeetingRoom> XZGLMeetingRoom { get; set; }
        public virtual DbSet<XZGLMeeting> XZGLMeeting { get; set; }


        public virtual DbSet<MeetingFile> MeetingFile { get; set; }

        public virtual DbSet<MeetingIssue> MeetingIssue { get; set; }

        public virtual DbSet<MeetingIssueRelation> MeetingIssueRelation { get; set; }


        public virtual DbSet<MeetingLlogistics> MeetingLlogistics { get; set; }
        public virtual DbSet<MeetingLogisticsRelation> MeetingLogisticsRelation { get; set; }


        public virtual DbSet<MeetingTypeBase> MeetingTypeBase { get; set; }


        public virtual DbSet<MeetingUser> MeetingUser { get; set; }

        public virtual DbSet<MeetingUserBeforeTask> MeetingUserBeforeTask { get; set; }


        public virtual DbSet<MeetingPeriodRule> MeetingPeriodRule { get; set; }


        public virtual DbSet<MeetingRoomUseInfo> MeetingRoomUseInfo { get; set; }

        #endregion



        public virtual DbSet<Follow> Follow { get; set; }
        public virtual DbSet<QrCode> QrCode { get; set; }
        public virtual DbSet<ImMessage> ImMessage { get; set; }
        public virtual DbSet<Daily> Daily { get; set; }
        public virtual DbSet<WorkList> WorkList { get; set; }
        public virtual DbSet<IM_Inquiry> IM_Inquiry { get; set; }
        public virtual DbSet<RoleRelation> RoleRelation { get; set; }
        public virtual DbSet<IM_InquiryResult> IM_InquiryResult { get; set; }
        public virtual DbSet<UserPermissionRelation> UserPermissionRelation { get; set; }
        public virtual DbSet<UserRoleRelation> UserRoleRelation { get; set; }


        public virtual DbSet<EmailLog> EmailLog { get; set; }
        public FRMSCoreDbContext(DbContextOptions<FRMSCoreDbContext> options)
            : base(options)
        {
        }
        
    }
}
