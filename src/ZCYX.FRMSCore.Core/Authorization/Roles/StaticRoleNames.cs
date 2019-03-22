namespace ZCYX.FRMSCore.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
            public const string Employee = "Employee";
            public const string DepartmentLeader = "DepartmentLeader";
            public const string HR = "HR";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string Contact = "Contact";
        }
    }
}
