using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Auditing;

namespace ZCYX.FRMSCore.Application
{
    public class ChangePasswordInput
    {
        [Required]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
    public class NoCurrentChangePasswordInput
    {
        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}