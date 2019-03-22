using Abp.Application.Navigation;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Project
{
    public class ProjectNavigationProvider : NavigationProvider
    {
        public const string MenuName = "ProjectMenu";

        private readonly FRMSCoreNavigationProvider _baseProvider;
        public ProjectNavigationProvider(FRMSCoreNavigationProvider baseProvider)
        {
            _baseProvider = baseProvider;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {

            _baseProvider.SetNavigationWithMouldName(context, "Project");

            //var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, L("ProjectMenu"));


            //menu.AddItem(new MenuItemDefinition("NoticeCenter",L("NoticeCenter"),icon: "ivu-icon ivu-icon-chatbox",requiredPermissionName: ProjectPermissionNames.Pages_NoticeCenter_Get,order:10)
            //    .AddItem(new MenuItemDefinition("NoticeCenter.Notice", L("NoticeCenter_Notice"),icon: "ivu-icon ivu-icon-chatbubble-working", url: "/NoticeCenter/Notice"))
            //    .AddItem(new MenuItemDefinition("NoticeCenter.New", L("NoticeCenter_New"), icon: "ivu-icon ivu-icon-chatbox-working", url: "/NoticeCenter/New"))
            //    .AddItem(new MenuItemDefinition("NoticeCenter.Message", L("NoticeCenter_Message"), icon: "ivu-icon ivu-icon-chatbubbles", url: "/NoticeCenter/Message"))
            //    );
            //menu.AddItem(new MenuItemDefinition(
            //     "Project",
            //     L("ProjectMenu"),
            //     icon: "ivu-icon ivu-icon-trophy",
            //     order:20
            //    )
            //        .AddItem(new MenuItemDefinition(ProjectConsts.Bianzhi, L("ProjectBianzhiInit"), icon: "ivu-icon ivu-icon-compose", url: "/task/new", requiredPermissionName: ProjectPermissionNames.Pages_Project_BianZhi))
            //        //.AddItem(new MenuItemDefinition(ProjectConsts.Audit, L("ProjectAuditInit"), icon: "ivu-icon ivu-icon-trophy", url: "/task/new2", requiredPermissionName: ProjectPermissionNames.Pages_Project_Audit))
            //        .AddItem(new MenuItemDefinition(ProjectConsts.Todo, L("ProjectTodo"), icon: "ivu-icon ivu-icon-heart", url: "/task/list", requiredPermissionName: ProjectPermissionNames.Pages_Project_Todo))
            //        .AddItem(new MenuItemDefinition(ProjectConsts.Check, L("ProjectCheck"), icon: "ivu-icon ivu-icon-search", url: "/task/project", requiredPermissionName: ProjectPermissionNames.Pages_Project_List))
            //     )

            //context.Manager.MainMenu.AddItem(new MenuItemDefinition(
            // "DailyOffice",
            // L("DailyOffice"),
            // icon: "ivu-icon ivu-icon-compass",
            // order: 30
            //)
            //    .AddItem(new MenuItemDefinition(ProjectConsts.OATodo, L("DailyOffice_OATodo"), icon: "ivu-icon ivu-icon-at", url: "/Work/oatodo"))
            //    //.AddItem(new MenuItemDefinition(ProjectConsts.Notice, L("DailyOffice_Notice"), icon: "ivu-icon ivu-icon-speakerphone", url: "/Work/DailyOfficeNotice", requiredPermissionName: ProjectPermissionNames.Pages_DailyOffice_Notice))
            //    //.AddItem(new MenuItemDefinition(ProjectConsts.XZNotice, L("DailyOffice_XZNotice"), icon: "ivu-icon ivu-icon-volume-low", url: "/Work/Notices", requiredPermissionName: ProjectPermissionNames.Pages_DailyOffice_XZNotice))
            //    .AddItem(new MenuItemDefinition(ProjectConsts.NoticeDocument, L("DailyOffice_NoticeDocument"), icon: "ivu-icon ivu-icon-ios-photos", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAReport, L("DailyOffice_OAReport"), icon: "ivu-icon ivu-icon-ios-timer-outline", url: "/Gov_OA/OAReport"))
            //            )
            //    .AddItem(new MenuItemDefinition(ProjectConsts.Employee, L("Employee"), icon: "ivu-icon ivu-icon-ios-eye", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAEmployee, L("OAEmployee"), icon: "ivu-icon ivu-icon-ios-bolt", url: "/Gov_OA/OAReport"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAPerformance, L("OAPerformance"), icon: "ivu-icon ivu-icon-ios-color-wand", url: "/Gov_OA/OAPerformance"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAPerformanceMain, L("OAPerformanceMain"), icon: "ivu-icon ivu-icon-ios-color-filter", url: "/Gov_OA/OAPerformanceMain"))
            //            )//人力资源
            //    .AddItem(new MenuItemDefinition(ProjectConsts.OAFixed, L("OAFixed"), icon: "ivu-icon ivu-icon-ios-crop-strong", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFixedAssets, L("OAFixedAssets"), icon: "ivu-icon ivu-icon-ios-barcode", url: "/Gov_OA/OAFixedAssets"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFixedAssetsPurchase, L("OAFixedAssetsPurchase"), icon: "ivu-icon ivu-icon-ios-briefcase", url: "/Gov_OA/OAFixedAssetsPurchase"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFixedAssetsUseApply, L("OAFixedAssetsUseApply"), icon: "ivu-icon ivu-icon-ios-medkit", url: "/Gov_OA/OAFixedAssetsUseApply"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFixedAssetsReturn, L("OAFixedAssetsReturn"), icon: "ivu-icon ivu-icon-ios-medical", url: "/Gov_OA/OAFixedAssetsReturn"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFixedAssetsRepair, L("OAFixedAssetsRepair"), icon: "ivu-icon ivu-icon-ios-infinite", url: "/Gov_OA/OAFixedAssetsRepair"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFixedAssetsScrap, L("OAFixedAssetsScrap"), icon: "ivu-icon ivu-icon-ios-calculator", url: "/Gov_OA/OAFixedAssetsScrap"))
            //            )//资产管理
            //    .AddItem(new MenuItemDefinition(ProjectConsts.OAProject, L("OAProject"), icon: "ivu-icon ivu-icon-ios-locked", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OABidProject, L("OABidProject"), icon: "ivu-icon ivu-icon-ios-locked", url: "/Gov_OA/OABidProject"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OABidFilePurchase, L("OABidFilePurchase"), icon: "ivu-icon ivu-icon-ios-printer", url: "/Gov_OA/OABidFilePurchase"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OABidSelfAudit, L("OABidSelfAudit"), icon: "ivu-icon ivu-icon-ios-game-controller-a", url: "/Gov_OA/OABidSelfAudit"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OABidProjectCheck, L("OABidProjectCheck"), icon: "ivu-icon ivu-icon-ios-body", url: "/Gov_OA/OABidProjectCheck"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OATenderAudit, L("OATenderAudit"), icon: "ivu-icon ivu-icon-ios-musical-notes", url: "/Gov_OA/OATenderAudit"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OATenderEnemy, L("OATenderEnemy"), icon: "ivu-icon ivu-icon-ios-wineglass", url: "/Gov_OA/OATenderEnemy"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OATenderCash, L("OATenderCash"), icon: "ivu-icon ivu-icon-ios-nutrition", url: "/Gov_OA/OATenderCash"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OATenderBuess, L("OATenderBuess"), icon: "ivu-icon ivu-icon-ios-flame", url: "/Gov_OA/OATenderBuess"))
            //            )//投标管理
            //    .AddItem(new MenuItemDefinition(ProjectConsts.OAMoney, L("OAMoney"), icon: "ivu-icon ivu-icon-ios-sunny", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAPay, L("OAPay"), icon: "ivu-icon ivu-icon-android-restaurant", url: "/Gov_OA/OAPay"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OABorrow, L("OABorrow"), icon: "ivu-icon ivu-icon-android-send", url: "/Gov_OA/OABorrow"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAPettyCash, L("OAPettyCash"), icon: "ivu-icon ivu-icon-android-archive", url: "/Gov_OA/OAPettyCash"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAFee, L("OAFee"), icon: "ivu-icon ivu-icon-android-delete", url: "/Gov_OA/OAFee"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAProgressFundDeclare, L("OAProgressFundDeclare"), icon: "ivu-icon ivu-icon-android-happy", url: "/Gov_OA/OAProgressFundDeclare"))
            //            )//财务管理
            //   .AddItem(new MenuItemDefinition(ProjectConsts.OAContractManager, L("OAContractManager"), icon: "ivu-icon ivu-icon-android-contact", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAContract, L("OAContract"), icon: "ivu-icon ivu-icon-android-playstore", url: "/Gov_OA/OAContract"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAContractCollectionFee, L("OAContractCollectionFee"), icon: "ivu-icon ivu-icon-android-hand", url: "/Gov_OA/OAContractCollectionFee"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OACompletionSettlement, L("OACompletionSettlement"), icon: "ivu-icon ivu-icon-android-desktop", url: "/Gov_OA/OACompletionSettlement"))
            //            )//合同管理
            //   .AddItem(new MenuItemDefinition(ProjectConsts.OAWork, L("OAWork"), icon: "ivu-icon ivu-icon-social-twitch", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OABuessOut, L("OABuessOut"), icon: "ivu-icon ivu-icon-link", url: "/Gov_OA/OABuessOut"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAWorkout, L("OAWorkout"), icon: "ivu-icon ivu-icon-pound", url: "/Gov_OA/OAWorkout"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OALeave, L("OALeave"), icon: "ivu-icon ivu-icon-cloud", url: "/Gov_OA/OALeave"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAWorkon, L("OAWorkon"), icon: "ivu-icon ivu-icon-navigate", url: "/Gov_OA/OAWorkon"))
            //            )//考勤管理
            //   .AddItem(new MenuItemDefinition(ProjectConsts.OACustomerManager, L("OACustomerManager"), icon: "ivu-icon ivu-icon-lock-combination", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OACustomer, L("OACustomer"), icon: "ivu-icon ivu-icon-pie-graph", url: "/Gov_OA/OACustomer"))
            //            )//客户管理
            //   .AddItem(new MenuItemDefinition(ProjectConsts.OADepartManager, L("OADepartManager"), icon: "ivu-icon ivu-icon-chatboxes", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OANotice, L("NoticeCenter_Notice"), icon: "ivu-icon ivu-icon-chatbubble-working", url: "/Gov_OA/OANotice", requiredPermissionName: ProjectPermissionNames.Pages_NoticeCenter_Create))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OANew, L("NoticeCenter_New"), icon: "ivu-icon ivu-icon-chatbox-working", url: "/Gov_OA/OANew", requiredPermissionName: ProjectPermissionNames.Pages_NoticeCenter_Create))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAMeeting, L("OAMeeting"), icon: "ivu-icon ivu-icon-beer", url: "/Gov_OA/OAMeeting"))
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OAUseCar, L("OAUseCar"), icon: "ivu-icon ivu-icon-wineglass", url: "/Gov_OA/OAUseCar"))
            //            )//行政管理
            //   .AddItem(new MenuItemDefinition(ProjectConsts.OATaskManager, L("OATaskManager"), icon: "ivu-icon ivu-icon-coffee", requiredPermissionName: ProjectPermissionNames.Pages_OA)
            //            .AddItem(new MenuItemDefinition(ProjectConsts.OATask, L("OATask"), icon: "ivu-icon ivu-icon-icecream", url: "/Gov_OA/OATask"))
            //            )//任务管理

            //)

            //.AddItem(new MenuItemDefinition(
            // "Pages_System",
            // L("Pages_System"),
            // icon: "ivu-icon ivu-icon-toggle",
            // requiredPermissionName: ProjectPermissionNames.Pages_System
            //)
            //    .AddItem(new MenuItemDefinition("SysMenuName_Organize", L("SysMenuName_Organize"), icon: "ivu-icon ivu-icon-speakerphone", url: "/admin/organize", requiredPermissionName: ProjectPermissionNames.Pages_System))
            //    .AddItem(new MenuItemDefinition("SysMenuName_Dic", L("SysMenuName_Dic"), icon: "ivu-icon ivu-icon-volume-low", url: "/admin/dictionary", requiredPermissionName: ProjectPermissionNames.Pages_System))
            //    .AddItem(new MenuItemDefinition("WorkflowFormManager", L("WorkflowFormManager"), icon: "ivu-icon ivu-icon-volume-low", url: "/admin/models", requiredPermissionName: ProjectPermissionNames.Pages_System))
            //    .AddItem(new MenuItemDefinition("WorkflowConfiger", L("WorkflowConfiger"), icon: "ivu-icon ivu-icon-volume-low", url: "/admin/flows", requiredPermissionName: ProjectPermissionNames.Pages_System))
            //)
            ;
            // 评审辅助
            //context.Manager.MainMenu.AddItem(new MenuItemDefinition("ProjectHelper", L("ProjectHelper"), icon: "wrench", order: 40)
            //    .AddItem(new MenuItemDefinition(ProjectConsts.ChargeOrganizations, L("ChargeOrganizations"), icon: "home", url: "/task/chargeorganizations", requiredPermissionName: ProjectPermissionNames.Pages_Project_ChargeOrganizations))
            //    .AddItem(new MenuItemDefinition(ProjectConsts.BusinessDepartment, L("BusinessDepartment"), icon: "briefcase", url: "/task/businessdepartment", requiredPermissionName: ProjectPermissionNames.Pages_Project_BusinessDepartment))
            //        .AddItem(new MenuItemDefinition(ProjectConsts.BuildUnit, L("AssistantMenuName_ConstructionOrganizations"), icon: "ivu-icon ivu-icon-hammer", url: "/task/buildunit", requiredPermissionName: ProjectPermissionNames.Pages_Project_BuildUnit))
            //        .AddItem(new MenuItemDefinition(ProjectConsts.ReplyUnit, L("ReplyUnit"), icon: "android-playstore", url: "/task/replyunit", requiredPermissionName: ProjectPermissionNames.Pages_Project_BusinessDepartment))
            //        .AddItem(new MenuItemDefinition(ProjectConsts.Area, L("ProjectArea"), icon: "location", url: "/task/areaManagement", requiredPermissionName: ProjectPermissionNames.Pages_Project_Area))
            //         .AddItem(new MenuItemDefinition(ProjectConsts.AuditGroup, L("ProjectAuditGroup"), icon: "person-stalker", url: "/task/auditGroup", requiredPermissionName: ProjectPermissionNames.Pages_Project_AuditGroup))
            //         .AddItem(new MenuItemDefinition(ProjectConsts.AuditFile, L("ProjectAuditFile"), icon: "ivu-icon ivu-icon-chatbox-working", url: "/task/auditFile", requiredPermissionName: ProjectPermissionNames.Pages_Project_AuditGroup))

            //    );
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectConsts.LocalizationSourceName);
        }
    }
}
