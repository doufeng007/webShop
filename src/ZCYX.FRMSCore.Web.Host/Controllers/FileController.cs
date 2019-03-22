using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using ZCYX.FRMSCore.Controllers;

namespace ZCYX.FRMSCore.Web.Host.Controllers
{
    //[Produces("application/json")]
    //[Route("api/[controller]")]
    public class FileController : FRMSCoreControllerBase
    {
        private IHostingEnvironment hostingEnv;

        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };

        public FileController(IHostingEnvironment env)
        {
            this.hostingEnv = env;
        }

        [HttpPost]
        public IActionResult Post()
        {
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);

            //size > 100MB refuse upload !
            if (size > 104857600)
            {
                return Json(Return_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("pictures total size > 100MB , server refused !"));
            }

            List<string> filePathResultList = new List<string>();

            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');

                string filePath = hostingEnv.WebRootPath + $@"\Files\upload\";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string suffix = fileName.ToString().Split(".")[1];

                //if (!pictureFormatArray.Contains(suffix))
                //{
                //    return Json(Return_Helper_DG.Error_Msg_Ecode_Elevel_HttpCode("the picture format not support ! you must upload files that suffix like 'png','jpg','jpeg','bmp','gif','ico'."));
                //}

                fileName = Guid.NewGuid() + "." + suffix;

                string fileFullName = filePath + fileName;

                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                filePathResultList.Add($"/src/upload/{fileName}");
            }

            string message = $"{files.Count} file(s) /{size} bytes uploaded successfully!";

            return Json(Return_Helper_DG.Success_Msg_Data_DCount_HttpCode(message, filePathResultList, filePathResultList.Count));
        }

    }


    public abstract class Return_Helper_DG
    {
        public static object IsSuccess_Msg_Data_HttpCode(bool isSuccess, string msg, dynamic data, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return new { isSuccess = isSuccess, msg = msg, httpCode = httpCode, data = data };
        }
        public static object Success_Msg_Data_DCount_HttpCode(string msg, dynamic data = null, int dataCount = 0, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return new { isSuccess = true, msg = msg, httpCode = httpCode, data = data, dataCount = dataCount };
        }
        public static object Error_Msg_Ecode_Elevel_HttpCode(string msg, int errorCode = 0, int errorLevel = 0, HttpStatusCode httpCode = HttpStatusCode.InternalServerError)
        {
            return new { isSuccess = false, msg = msg, httpCode = httpCode, errorCode = errorCode, errorLevel = errorLevel };
        }
    }
}