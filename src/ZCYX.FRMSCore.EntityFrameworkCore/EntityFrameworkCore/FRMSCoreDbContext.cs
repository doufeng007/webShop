using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ZCYX.FRMSCore.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.MultiTenancy;
using Abp.WorkFlow;
using Abp.WorkFlowDictionary;
using Abp.WorkFlow.Entity;
using Abp.File;
using HR;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Entity;
using IMLib;
using EmailServer;
using B_H5;

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



        public virtual DbSet<OrganizationUnitPostsRole> OrganizationUnitPostsRole { get; set; }


        public virtual DbSet<NoticeLogs> NoticeLogs { get; set; }

        public virtual DbSet<NoticeTexts> NoticeTexts { get; set; }

        public virtual DbSet<ProjectAudit> ProjectAudit { get; set; }


        public virtual DbSet<AbpFile> AbpFile { get; set; }


        public virtual DbSet<AbpFileRelation> AbpFileRelation { get; set; }



        #region 任务
        public virtual DbSet<TaskManagementRelation> TaskManagementRelation { get; set; }



        #endregion








        public virtual DbSet<RealationSystem> RealationSystem { get; set; }


        public virtual DbSet<ContractWithSystem> ContractWithSystem { get; set; }






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


        public virtual DbSet<OrganizationUnitPostPlan> OrganizationUnitPostPlan { get; set; }

        public virtual DbSet<OrganizationUnitPostChangePlan> OrganizationUnitPostChangePlan { get; set; }

        public virtual DbSet<EmployeePlan> EmployeePlan { get; set; }

        public virtual DbSet<EmployeeResult> EmployeeResult { get; set; }

        #endregion

        #region 用品管理

        public virtual DbSet<UserMenuInit> UserMenuInit { get; set; }




        #endregion


        public virtual DbSet<OAWorkout> OAWorkout { get; set; }

        public virtual DbSet<OAWorkon> OAWorkon { get; set; }



        public virtual DbSet<AbpPermissionBase> AbpPermissionBase { get; set; }
        public virtual DbSet<AbpMenuBase> AbpMenuBase { get; set; }


        public virtual DbSet<EmployeeProjecExperience> EmployeeProjecExperience { get; set; }
        public virtual DbSet<EmployeeResume> EmployeeResume { get; set; }
        public virtual DbSet<QuestionLibrary> QuestionLibrary { get; set; }
    


        public virtual DbSet<Employees_Sign> Employees_Sign { get; set; }







        public virtual DbSet<Follow> Follow { get; set; }
        public virtual DbSet<QrCode> QrCode { get; set; }
        public virtual DbSet<ImMessage> ImMessage { get; set; }
        public virtual DbSet<WorkList> WorkList { get; set; }
        public virtual DbSet<IM_Inquiry> IM_Inquiry { get; set; }
        public virtual DbSet<RoleRelation> RoleRelation { get; set; }
        public virtual DbSet<IM_InquiryResult> IM_InquiryResult { get; set; }
        public virtual DbSet<UserPermissionRelation> UserPermissionRelation { get; set; }
        public virtual DbSet<UserRoleRelation> UserRoleRelation { get; set; }


        public virtual DbSet<EmailLog> EmailLog { get; set; }

        #region  B_H5

        public virtual DbSet<B_Agency> B_Agency { get; set; }
        public virtual DbSet<B_InviteUrl> B_InviteUrl { get; set; }

        public virtual DbSet<B_Message> B_Message { get; set; }
        public virtual DbSet<B_MyAddress> B_MyAddress { get; set; }
        public virtual DbSet<B_Notice> B_Notice { get; set; }
        public virtual DbSet<B_Question> B_Question { get; set; }
        public virtual DbSet<B_StoreSignUp> B_StoreSignUp { get; set; }
        public virtual DbSet<B_TrialProduct> B_TrialProduct { get; set; }

        


        #endregion


        public FRMSCoreDbContext(DbContextOptions<FRMSCoreDbContext> options)
            : base(options)
        {
        }

    }
}
