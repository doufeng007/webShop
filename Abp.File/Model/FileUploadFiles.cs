using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.File
{
    public class FileUploadFiles
    {
        public string Id { get; set; }
        public string FileName { get; set; }

        public string FileLink { get; set; }


        public long Length { get; set; }
    }
}
