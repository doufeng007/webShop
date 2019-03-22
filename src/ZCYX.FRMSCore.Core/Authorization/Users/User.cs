using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace ZCYX.FRMSCore.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";
        [StringLength(10, ErrorMessage = "最大长度为10")]
        public string WorkNumber { get; set; }

        public bool? Sex { get; set; }
        [StringLength(18, ErrorMessage = "最大长度为18")]
        public string IdCard { get; set; }

        public DateTime? EnterTime { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public virtual Guid? ProfilePictureId { get; set; }

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
