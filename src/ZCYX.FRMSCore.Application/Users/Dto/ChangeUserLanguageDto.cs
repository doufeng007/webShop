using System.ComponentModel.DataAnnotations;

namespace ZCYX.FRMSCore.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}