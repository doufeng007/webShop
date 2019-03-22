using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Abp.AspNetCore.Mvc.Authorization;
using ZCYX.FRMSCore.Controllers;
using ZCYX.FRMSCore.Authorization.Users;
using ZCYX.FRMSCore.Application;
using Microsoft.AspNetCore.Mvc;
using ZCYX.FRMSCore.Core;
using static System.Net.Mime.MediaTypeNames;
using ZCYX.FRMSCore.IO;
using Microsoft.AspNetCore.Hosting;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Web.Core
{
    [AbpMvcAuthorize]
    [Route("api/[controller]/[action]")]
    public class ProfileController : FRMSCoreControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IAppFolders _appFolders;
        //private readonly IFriendshipManager _friendshipManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProfileController(
            UserManager userManager,
            IBinaryObjectManager binaryObjectManager,
            IAppFolders appFolders
            //IFriendshipManager friendshipManager
            , IHostingEnvironment hostingEnvironment
            )
        {
            _userManager = userManager;
            _binaryObjectManager = binaryObjectManager;
            _appFolders = appFolders;
            //_friendshipManager = friendshipManager;
            _hostingEnvironment = hostingEnvironment;
        }

        [DisableAuditing]
        [HttpGet]
        public async Task<FileResult> GetProfilePicture()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user.ProfilePictureId == null)
            {
                return GetDefaultProfilePicture();
            }

            return await GetProfilePictureById(user.ProfilePictureId.Value);
        }

        [DisableAuditing]
        [HttpGet]
        public async Task<string> GetProfilePictureBase64()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            if (user.ProfilePictureId == null)
            {
                return GetDefaultProfileBase64Picture();
            }

            return await GetProfilePictureBsae64ById(user.ProfilePictureId.Value);
        }



        [HttpPost]
        public JsonResult UploadProfilePicture()
        {
            try
            {
                //Check input
                var files = Request.Form.Files;
                if (files.Count <= 0 || files[0] == null)
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "头像修改错误.");
                }

                var file = files[0];

                if (file.Length > 1048576) //1MB.
                {
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "只能选择1mb内的JPG/JPEG图片，请重新选择头像文件.");
                }

                //Check file type & format 交给前端去处理
                //var fileImage = System.Drawing.  Image.FromStream(file.InputStream);
                //var acceptedFormats = new List<ImageFormat>
                //{
                //    ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif
                //};

                //if (!acceptedFormats.Contains(fileImage.RawFormat))
                //{
                //    throw new ApplicationException("Uploaded file is not an accepted image file !");
                //}

                //Delete old temp profile pictures
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "userProfileImage_" + AbpSession.GetUserId());

                //Save new picture
                var fileInfo = new FileInfo(file.FileName);
                var tempFileName = "userProfileImage_" + AbpSession.GetUserId() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
                //file.sa(tempFilePath);

                using (FileStream fs = System.IO.File.Create(tempFilePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return Json(new AjaxResponse(new { fileName = tempFileName, }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //[DisableAuditing]
        //[HttpGet]
        //public async Task<FileResult> GetProfilePictureById(string id = "")
        //{
        //    if (id.IsNullOrEmpty())
        //    {
        //        return GetDefaultProfilePicture();
        //    }

        //    return await GetProfilePictureById(Guid.Parse(id));
        //}

        //[DisableAuditing]
        //[UnitOfWork]
        //public virtual async Task<FileResult> GetFriendProfilePictureById(long userId, int? tenantId, string id = "")
        //{
        //    if (id.IsNullOrEmpty() ||
        //        _friendshipManager.GetFriendshipOrNull(AbpSession.ToUserIdentifier(), new UserIdentifier(tenantId, userId)) == null)
        //    {
        //        return GetDefaultProfilePicture();
        //    }

        //    using (CurrentUnitOfWork.SetTenantId(tenantId))
        //    {
        //        return await GetProfilePictureById(Guid.Parse(id));
        //    }
        //}

        //[UnitOfWork]
        //[HttpPost]
        //public virtual async Task<JsonResult> ChangeProfilePicture()
        //{
        //    try
        //    {
        //        //Check input
        //        var files = Request.Form.Files;
        //        if (files.Count <= 0 || files[0] == null)
        //        {
        //            throw new UserFriendlyException("头像修改错误.");
        //        }

        //        var file = files[0];

        //        if (file.Length > 30720) //30KB.
        //        {
        //            throw new UserFriendlyException("只能选择1mb内的JPG/JPEG图片，请重新选择头像文件.");
        //        }

        //        //Get user
        //        var user = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());

        //        //Delete old picture
        //        if (user.ProfilePictureId.HasValue)
        //        {
        //            await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
        //        }

        //        //Save new picture

        //        var storedFile = new BinaryObject(AbpSession.TenantId, file.OpenReadStream().GetAllBytes());
        //        await _binaryObjectManager.SaveAsync(storedFile);

        //        //Update new picture on the user
        //        user.ProfilePictureId = storedFile.Id;

        //        //Return success
        //        return Json(new AjaxResponse());
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        //Return error message
        //        return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
        //    }
        //}





        private FileResult GetDefaultProfilePicture()
        {
            var path = $"{ _hostingEnvironment.WebRootPath}/Common/Images/default-profile-picture.png";
            return File(path, Net.MimeTypes.MimeTypeNames.ImagePng);
        }

        private string GetDefaultProfileBase64Picture()
        {
            var path = $"{ _hostingEnvironment.WebRootPath}/Common/Images/default-profile-picture.png";
            var file = File(path, Net.MimeTypes.MimeTypeNames.ImagePng);
            var retStr = "";
            using (var filestream = new FileStream(path, FileMode.Open))
            {
                byte[] bt = new byte[filestream.Length];
                //调用read读取方法  
                filestream.Read(bt, 0, bt.Length);
                retStr = Convert.ToBase64String(bt);
            }

            return retStr;

        }

        private async Task<FileResult> GetProfilePictureById(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return GetDefaultProfilePicture();
            }

            return File(file.Bytes, Net.MimeTypes.MimeTypeNames.ImageJpeg);
        }

        private async Task<string> GetProfilePictureBsae64ById(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return GetDefaultProfileBase64Picture();
            }
            else
            {
                return Convert.ToBase64String(file.Bytes);
            }

        }

    }
}