using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.File
{
    public class AbpFileUploadResultModel
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public long Size { get; set; }

        public string FileExtend { get; set; }

        /// <summary>
        /// 是否成功生成缩略图
        /// </summary>
        public bool HasThumFile { get; set; }


        public Guid ThumFileId { get; set; }


    }


    public class TempAbpFileUploadResultModel : AbpFileUploadResultModel
    {
        public string FileRelationPath { get; set; }
    }
}
