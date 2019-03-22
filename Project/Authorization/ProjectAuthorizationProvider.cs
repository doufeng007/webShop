using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using ZCYX.FRMSCore.Authorization;

namespace Project
{
    public class ProjectAuthorizationProvider : AuthorizationProvider
    {
        private readonly ApplicationAuthorizationProvider _baseprovider;
        public ProjectAuthorizationProvider(ApplicationAuthorizationProvider baseprovider)
        {
            _baseprovider = baseprovider;
        }
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            
            _baseprovider.SetPermissionsWithMouldName(context, "Project");


            //var project = context.CreatePermission(ProjectPermissionNames.Pages_Project, L("ProjectMenu"));
            //project.CreateChildPermission(ProjectPermissionNames.Pages_Project_BianZhi, L("ProjectBianzhiInit"));
            //project.CreateChildPermission(ProjectPermissionNames.Pages_Project_Audit, L("ProjectAuditInit"));
            //project.CreateChildPermission(ProjectPermissionNames.Pages_Project_Todo, L("ProjectTodo"));
            //project.CreateChildPermission(ProjectPermissionNames.Pages_Project_List, L("ProjectList"));
           
            //var projectHelper = context.CreatePermission(ProjectPermissionNames.Pages_ProjectHelper, L("ProjectHelper"));
            //projectHelper.CreateChildPermission(ProjectPermissionNames.Pages_Project_BuildUnit, L("AssistantMenuName_ConstructionOrganizations"));
            //projectHelper.CreateChildPermission(ProjectPermissionNames.Pages_Project_Area, L("ProjectArea"));
            //projectHelper.CreateChildPermission(ProjectPermissionNames.Pages_Project_AuditGroup, L("ProjectAuditGroup"));
            //projectHelper.CreateChildPermission(ProjectPermissionNames.Pages_Project_ChargeOrganizations, L("ChargeOrganizations"));
            //projectHelper.CreateChildPermission(ProjectPermissionNames.Pages_Project_ReplyUnit, L("ReplyUnit"));


           // projectHelper.CreateChildPermission(ProjectPermissionNames.Pages_Project_BusinessDepartment, L("BusinessDepartment"));


            //var pages_DailyOffice = context.CreatePermission(ProjectPermissionNames.Pages_DailyOffice, L("DailyOffice"));
            //pages_DailyOffice.CreateChildPermission(ProjectPermissionNames.Pages_DailyOffice_Notice, L("DailyOffice_Notice"));
            //pages_DailyOffice.CreateChildPermission(ProjectPermissionNames.Pages_DailyOffice_XZNotice, L("DailyOffice_XZNotice"));
            //pages_DailyOffice.CreateChildPermission(ProjectPermissionNames.Pages_OA, L("Pages_OA"));
            //pages_DailyOffice.CreateChildPermission(ProjectPermissionNames.Pages_NoticeCenter_Get, L("Pages_NoticeCenter_Get"));
            //pages_DailyOffice.CreateChildPermission(ProjectPermissionNames.Pages_NoticeCenter_Create, L("Pages_NoticeCenter_Create"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectConsts.LocalizationSourceName);
        }
    }
}
