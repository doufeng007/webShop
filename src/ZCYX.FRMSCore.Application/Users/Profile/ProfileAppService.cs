using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.IO;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Timing;
using ZCYX.FRMSCore.Core;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Security;
using Abp.Reflection.Extensions;
using System;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Application.Users.Profile
{
    [AbpAuthorize]
    public class ProfileAppService : FRMSCoreAppServiceBase, IProfileAppService
    {
        private readonly IAppFolders _appFolders;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        public UserManager UserManager { get; set; }
        public ProfileAppService(

            IBinaryObjectManager binaryObjectManager
            )
        {
            _binaryObjectManager = binaryObjectManager;
        }

        //public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        //{
        //    var user = await GetCurrentUserAsync();
        //    var userProfileEditDto = user.MapTo<CurrentUserProfileEditDto>();

        //    if (Clock.SupportsMultipleTimezone)
        //    {
        //        userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

        //        var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
        //        if (userProfileEditDto.Timezone == defaultTimeZoneId)
        //        {
        //            userProfileEditDto.Timezone = string.Empty;
        //        }
        //    }

        //    return userProfileEditDto;
        //}

        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();
            input.MapTo(user);
            CheckErrors(await UserManager.UpdateAsync(user));

            if (Clock.SupportsMultipleTimezone)
            {
                if (input.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                }
            }
        }

        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            var tempProfilePicturePath = Path.Combine(_appFolders.TempFileDownloadFolder, input.FileName);

            byte[] byteArray;

            using (var fsTempProfilePicture = new FileStream(tempProfilePicturePath, FileMode.Open))
            {
                //using (var bmpImage = new Bitmap(fsTempProfilePicture))
                //{
                //    var width = input.Width == 0 ? bmpImage.Width : input.Width;
                //    var height = input.Height == 0 ? bmpImage.Height : input.Height;
                //    var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                //    using (var stream = new MemoryStream())
                //    {
                //        bmCrop.Save(stream, bmpImage.RawFormat);
                //        stream.Close();
                //        byteArray = stream.ToArray();
                //    }
                //}
                byteArray = new byte[fsTempProfilePicture.Length];
                fsTempProfilePicture.Read(byteArray, 0, byteArray.Length);
                // 设置当前流的位置为流的开始
                fsTempProfilePicture.Seek(0, SeekOrigin.Begin);

            }

            if (byteArray.LongLength > 1024000 * 4) //1000 KB
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "只能选择1mb内的JPG/JPEG图片，请重新选择头像文件.");
            }

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;

            FileHelper.DeleteIfExists(tempProfilePicturePath);
        }


        public async Task UpdateProfilePictureBase64(UpdateProfilePictureBase64Input input)
        {
            //byte[] byteArray = System.Text.Encoding.Default.GetBytes(input.FileContent);
            byte[] byteArray = System.Convert.FromBase64String(input.FileContent);

            if (byteArray.LongLength > 1024000 * 4) //1000 KB
            {
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "只能选择1mb内的JPG/JPEG图片，请重新选择头像文件.");
            }

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());

            if (user.ProfilePictureId.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            user.ProfilePictureId = storedFile.Id;
        }

        public async Task ChangePassword(ChangePasswordInput input)
        {
            //await CheckPasswordComplexity(input.NewPassword);

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());
            //await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword));
            //CheckErrors(await _userManager.ChangePasswordAsync(user, input.User.Password));
        }

        public async Task NoCurrentChangePassword(NoCurrentChangePasswordInput input)
        {
            //await CheckPasswordComplexity(input.NewPassword);

            var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());
            user.IsTwoFactorEnabled = true;//用于标识是否第一次登陆
            //await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
            
            //CheckErrors(await _userManager.ChangePasswordAsync(user, input.User.Password));
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }






        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
         //   var settingValue = await SettingManager.GetSettingValueAsync(AppSettingNames.Security.PasswordComplexity);
           // var setting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(settingValue);

            var coreAssemblyDirectoryPath = typeof(ProfileAppService).GetAssembly().GetDirectoryPathOrNull();
            var _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
           
            var setting = new PasswordComplexitySetting();
            setting.MaxLength= Convert.ToInt32(_appConfiguration["Password:MaxLength"]);
            setting.MinLength= Convert.ToInt32(_appConfiguration["Password:RequiredLength"]);
            setting.UsePunctuations = _appConfiguration["Password:RequireDigit"]== "true";
            setting.UseNumbers = _appConfiguration["Password:RequireNonAlphanumeric"] == "true";
            setting.UseUpperCaseLetters = _appConfiguration["Password:RequireUppercase"] == "true";
            setting.UseLowerCaseLetters = _appConfiguration["Password:RequireLowercase"] == "true";

            return new GetPasswordComplexitySettingOutput
            {
                Setting = setting
            };
        }

        private async Task CheckPasswordComplexity(string password)
        {
            
            //var passwordComplexitySettingValue = await SettingManager.GetSettingValueAsync(AppConsts.App.Security.PasswordComplexity);
            //var passwordComplexitySetting = JsonConvert.DeserializeObject<PasswordComplexitySetting>(passwordComplexitySettingValue);
            //if (passwordComplexitySetting == null)
            //{
               var passwordComplexitySetting = new PasswordComplexitySetting
                {
                    MinLength = 6,
                    MaxLength = 10,
                    UseNumbers = true,
                    UseUpperCaseLetters = false,
                    UseLowerCaseLetters = true,
                    UsePunctuations = false,
                };
            //}
            var passwordComplexityChecker = new PasswordComplexityChecker();
            var passwordValid = passwordComplexityChecker.Check(passwordComplexitySetting, password);
            if (!passwordValid)
            {
                throw new UserFriendlyException(L("PasswordComplexityNotSatisfied"));
            }
        }
    }
}