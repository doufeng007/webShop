namespace ZCYX.FRMSCore
{
    public class AppConsts
    {
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";
        /// <summary>
        /// Gov: 政务版  Enter:企业版
        /// </summary>
        public const string Edition = "Enter";
        /// <summary>
        /// 本地化文件位置
        /// </summary>
        public const string LocalizationSourceName = "Application";
        public class App
        {
            public static class Common
            {
                public const string Administration = "Administration";
                public const string Roles = "Administration.Roles";
                public const string Users = "Administration.Users";
                public const string Models = "Administration.Models";
                public const string AuditLogs = "Administration.AuditLogs";
                public const string OrganizationUnits = "Administration.OrganizationUnits";
                public const string Languages = "Administration.Languages";
                public const string SystemLogs = "Administration.Systemlogs";
                public const string RealationSystem = "Administration.RealationSystem";


            }
            /// <summary>
            /// 宿主
            /// </summary>
            public static class Host
            {
                public const string Tenants = "Tenants";
                public const string Editions = "Editions";
                public const string Maintenance = "Administration.Maintenance";
                public const string Settings = "Administration.Settings.Host";
            }
            /// <summary>
            /// 租户
            /// </summary>
            public static class Tenant
            {
                public const string Dashboard = "Dashboard.Tenant";
                public const string Settings = "Administration.Settings.Tenant";
                public const string Educational = "Educational";
            }

            public static class Security
            {
                public const string PasswordComplexity = "App.Security.PasswordComplexity";
            }
        }
    }
}
