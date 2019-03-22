using Abp.File;
using Abp.WorkFlow;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp
{
    public class GetAbpFilesTaskOutput : GetAbpFilesOutput
    {
        public string TaskId { get; set; }
    }
}
