using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Roles;
using ZCYX.FRMSCore.Authorization.Users;

namespace ZCYX.FRMSCore.Authorization.Roles
{
    public class Role : AbpRole<User>
    {

        public bool IsSelect { get; set; }
        public const int MaxDescriptionLength = 5000;

        public Role()
        {
        }

        public Role(int? tenantId, string displayName)
            : base(tenantId, displayName)
        {
        }

        public Role(int? tenantId, string name, string displayName)
            : base(tenantId, name, displayName)
        {
        }

        [StringLength(MaxDescriptionLength)]
        public string Description {get; set;}
    }
}
