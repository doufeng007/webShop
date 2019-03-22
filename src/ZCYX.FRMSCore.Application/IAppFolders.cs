using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }

        string SampleProfileImagesFolder { get; }

        string WebLogsFolder { get; set; }

        string ImportFileFolder { get; set; }

        string ImporttocTemplateFolder { get; set; }
    }
}
