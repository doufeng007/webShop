using Abp.AspNetCore.Mvc.Controllers;
using Abp.Domain.Repositories;
using Abp.Reflection.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SearchAll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Model;

namespace Abp.File
{
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AbpFileController : AbpController
    {
        private IHostingEnvironment hostingEnv;
      //  private ISearchAllAppService _searchAllAppService;
        private readonly IRepository<AbpFile, Guid> _abpFilerepository;

        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };

        public AbpFileController(IHostingEnvironment env, IRepository<AbpFile, Guid> abpFilerepository)
        {
            this.hostingEnv = env;
            _abpFilerepository = abpFilerepository;
            //_searchAllAppService = searchAllAppService;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Post(bool isWordToPdf = false)
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            var coreAssemblyDirectoryPath = typeof(AbpFileController).GetAssembly().GetDirectoryPathOrNull();
            var _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            var bodySize = Convert.ToInt32(_appConfiguration["App:MaxRequestBodySize"]);
            //size > 100MB refuse upload !
            if (size > (bodySize * 1024 * 1024))
            {
                //throw new UserFriendlyException((int)ErrorCode.DataAccessErr, $"上传文件大于{bodySize}MB ,请联系管理员。");
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode($"上传文件大于{bodySize}MB ,请联系管理员。"));
            }

            var filePathResultList = new List<AbpFileUploadResultModel>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                var old_fileName = fileName;
                var dt = DateTime.Now;
                string filePath = hostingEnv.WebRootPath + $@"/Files/upload/" + dt.Year + "/" + dt.Month + "/";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string suffix = fileName.ToString().Split(".")[1];

                //if (!pictureFormatArray.Contains(suffix))
                //{
                //    return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'."));
                //}

                fileName = Guid.NewGuid() + "." + suffix;

                string fileFullName = filePath + fileName;

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var ret = new AbpFileUploadResultModel()
                {
                    Id = Guid.NewGuid(),
                    FileName = old_fileName,
                    Size = file.Length,
                    FileExtend = suffix,
                };
                filePathResultList.Add(ret);
                var entity = new AbpFile()
                {
                    Id = ret.Id,
                    FileName = ret.FileName,
                    FileExtend = ret.FileExtend,
                    FileSize = ret.Size,
                    FilePath = fileFullName
                };
                _abpFilerepository.Insert(entity);

                if (isWordToPdf)
                {
                    var ext = suffix.ToLower();
                    if (ext == "doc" || ext == "docx")
                    {
                        var savePath = fileFullName.Replace("." + suffix, ".pdf");
                        try
                        {
                            ThreePartDll.ThreePartDllHelper.CreatePDF(fileFullName, savePath);
                            var turnFilePath = savePath;
                            var turnFileName = old_fileName.Replace("." + suffix, ".pdf");
                            System.IO.FileInfo objFI = new System.IO.FileInfo(savePath);
                            entity = new AbpFile()
                            {
                                Id = Guid.NewGuid(),
                                FileName = turnFileName,
                                FileExtend = "pdf",
                                FileSize = objFI.Length,
                                TurnFileId = ret.Id,
                                TurnType = TurnType.转PDF,
                                FilePath = turnFilePath
                            };
                            _abpFilerepository.Insert(entity);
                           
                        }
                        catch (Exception ex)
                        {
                            return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode($"转档失败"));
                        }
                    }
                }


                /* //文件转为文本存入elasticsearch
                 var pageoffice = new Search();
                 pageoffice.Id = entity.Id;
                 pageoffice.Content = entity.FilePath;
                 pageoffice.Title = entity.FileName;
                 _searchAllAppService.CreateOffice(pageoffice);*/

            }

            string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";

            return Json(AbpFileReturn_Helper_DG.Success_Msg_Data_DCount_HttpCode(message, filePathResultList, filePathResultList.Count));
        }

        /// <summary>
        /// 批注
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        [HttpPost]
        public void PostFlie(string path, string id)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(id, out fileId))
                return;
            var fileModel = _abpFilerepository.Get(fileId);
            FileInfo tmpFile = new FileInfo(fileModel.FilePath);
            if (!tmpFile.Exists && fileModel.FilePath != path)
            {
                var message = "文件不存在";
                return;
            }
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                var tmp = path + "tmp";
                using (FileStream fs = System.IO.File.Create(tmp))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                System.IO.File.Copy(tmp, path, true);
                System.IO.File.Delete(tmp);
            }
        }


        [HttpPost]
        //[Route("accountantCoursePost")]
        public IActionResult AccountantCoursePost()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 100MB refuse upload !
            if (size > 104857600)
            {
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("file total size > 500MB , server refused !"));
            }

            var filePathResultList = new List<TempAbpFileUploadResultModel>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                var old_fileName = fileName;

                string filePath = hostingEnv.WebRootPath + $@"/Files/upload/temp/";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string suffix = fileName.ToString().Split(".")[1];

                //if (!pictureFormatArray.Contains(suffix))
                //{
                //    return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'."));
                //}

                fileName = Guid.NewGuid() + "." + suffix;

                string fileFullName = filePath + fileName;

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                var ret = new TempAbpFileUploadResultModel()
                {
                    Id = Guid.NewGuid(),
                    FileName = old_fileName,
                    Size = file.Length,
                    FileExtend = suffix,
                    FileRelationPath = fileName,
                };
                filePathResultList.Add(ret);
            }

            string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";

            return Json(AbpFileReturn_Helper_DG.Success_Msg_Data_DCount_HttpCode(message, filePathResultList, filePathResultList.Count));
        }
        [HttpGet]
        public IActionResult GetTest(string title,string content)
        {
            var a = new string[] { "title", "date" };
            var b = new object[] { title, DateTime.Now.ToString("yyyy年MM月dd日") };
            string filePath = hostingEnv.WebRootPath + $@"\Files\test.docx";
            var file = ThreePartDll.ThreePartDllHelper.Get(a, b, filePath,content);
            return base.File(file, "application/msword", "Template.docx");
        }


        [HttpGet]
        public IActionResult Show(string id,bool isTurn=false)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(id, out fileId))
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("fileId not found , server refused !"));
            var fileModel = _abpFilerepository.Get(fileId);
            if (isTurn)
            {
                fileModel= _abpFilerepository.FirstOrDefault(x=>x.TurnType==TurnType.转PDF &&x.TurnFileId == fileId);
            }
            var filepath = fileModel.FilePath;
            var fileName = fileModel.FileName;
          
            FileInfo tmpFile = new FileInfo(filepath);
            if (!tmpFile.Exists)
            {
                var message = "文件不存在";
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode(message));
            }
            var stream = System.IO.File.OpenRead(tmpFile.FullName);
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(fileName));
        }


        [HttpGet]
        public IActionResult ShowFile(string filename)
        {
            string filePath = hostingEnv.WebRootPath + $@"/Files/upload/"+ filename + ".xlsx";
            FileInfo tmpFile = new FileInfo(filePath);
            if (!tmpFile.Exists)
            {
                var message = "文件不存在";
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode(message));
            }
            var stream = System.IO.File.OpenRead(tmpFile.FullName);
            return File(stream, "application/vnd.android.package-archive", Path.GetFileName(filename+ "工作日报.xlsx"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTemplate(Guid modelId,int type) {
            var temproot = hostingEnv.ContentRootPath + @"\wwwroot\templates\";//模板文件夹
            var t = "";
            switch (type)
            {
                case 1:
                    t = "detail";
                    break;
                case 0:
                    t = "edit";
                    break;
            }
            var temfilename = $"{modelId}-{t}.vue";//模板文件名
            if (System.IO.File.Exists(temproot + $@"\{temfilename}"))
            {
                var stream = System.IO.File.OpenRead(temproot + $@"\{temfilename}");
                return File(stream, "application/vnd.android.package-archive", Path.GetFileName(temfilename));
            }
            else {
              throw new  UserFriendlyException((int)ErrorCode.DataAccessErr, "当前模版文件不存在，请先保存后再试。");
            }
        }
        [HttpGet]
        public IActionResult GetInfo(string id)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(id, out fileId))
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("fileId not found , server refused !"));
            var fileModel = _abpFilerepository.Get(fileId);
            FileInfo tmpFile = new FileInfo(fileModel.FilePath);
            if (!tmpFile.Exists)
            {
                var message = "文件不存在";
                return Json(AbpFileReturn_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode(message));
            }
            return Json(fileModel);
        }


        [HttpGet]
        public string Exists(string id)
        {
            var fileId = Guid.Empty;
            if (!Guid.TryParse(id, out fileId))
                return null;
            try
            {
                var fileModel = _abpFilerepository.Get(fileId);
                FileInfo tmpFile = new FileInfo(fileModel.FilePath);
                if (!tmpFile.Exists)
                    return null;
                return tmpFile.Extension;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }


}