using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.AutoMapper;
using ZCYX.FRMSCore.Authorization.Roles;

namespace ZCYX.FRMSCore.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleDto : EntityDto<int>
    {
        //[Required]
        //[StringLength(AbpRoleBase.MaxNameLength)]
        //public string Name { get; set; }

        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public string NormalizedName { get; set; }
        
        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        public bool IsStatic { get; set; }
        public bool IsSelect { get; set; }

        public bool IsDefault { get; set; }

        public List<string> Permissions { get; set; }
    }

    [AutoMap(typeof(Role))]
    public class RoleInput : EntityDto<int>
    {
        [Required]
        [StringLength(AbpRoleBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpRoleBase.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public string NormalizedName { get; set; }

        [StringLength(Role.MaxDescriptionLength)]
        public string Description { get; set; }

        //public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public List<string> Permissions { get; set; }
    }
}
