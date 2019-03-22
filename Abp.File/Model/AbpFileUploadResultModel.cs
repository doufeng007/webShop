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


    }


    public class TempAbpFileUploadResultModel : AbpFileUploadResultModel
    {
        public string FileRelationPath { get; set; }
    }
}
