using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Train.Enum
{
    public enum CourseFileType
    {
        [Description("pdf文件")] Pdf = 0,
        [Description("word文件")] Doc = 1,
        [Description("视频/音频")] Link = 2
    }
}
