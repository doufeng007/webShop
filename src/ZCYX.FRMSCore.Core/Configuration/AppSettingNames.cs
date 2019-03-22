namespace ZCYX.FRMSCore.Configuration
{
    public static class AppSettingNames
    {
        public const string UiTheme = "App.UiTheme";

        public static class General
        {
            public const string WebSiteRootAddress = "App.General.WebSiteRootAddress";
        }

        public static class TenantManagement
        {
            public const string AllowSelfRegistration = "App.TenantManagement.AllowSelfRegistration";
            public const string IsNewRegisteredTenantActiveByDefault = "App.TenantManagement.IsNewRegisteredTenantActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.TenantManagement.UseCaptchaOnRegistration";
            public const string DefaultEdition = "App.TenantManagement.DefaultEdition";
        }

        public static class UserManagement
        {
            public const string AllowSelfRegistration = "App.UserManagement.AllowSelfRegistration";
            public const string IsNewRegisteredUserActiveByDefault = "App.UserManagement.IsNewRegisteredUserActiveByDefault";
            public const string UseCaptchaOnRegistration = "App.UserManagement.UseCaptchaOnRegistration";
            public const string XietongdanweiOrgId = "XietongdanweiOrgId";
        }

        public static class Security
        {
            public const string PasswordComplexity = "App.Security.PasswordComplexity";
        }
    }
}
