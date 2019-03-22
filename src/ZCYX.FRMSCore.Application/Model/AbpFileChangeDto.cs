using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class AbpFileChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        [LogColumn("名称", IsNameField = true)]
        public string FileName { get; set; }
    }
}
