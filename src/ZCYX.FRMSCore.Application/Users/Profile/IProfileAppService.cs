using System.Threading.Tasks;
using Abp.Application.Services;

namespace ZCYX.FRMSCore.Application
{
    public interface IProfileAppService : IApplicationService
    {
        //Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit();

        //Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input);
        
        Task ChangePassword(ChangePasswordInput input);

        Task UpdateProfilePicture(UpdateProfilePictureInput input);

        Task UpdateProfilePictureBase64(UpdateProfilePictureBase64Input input);

        Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting();
    }
}
