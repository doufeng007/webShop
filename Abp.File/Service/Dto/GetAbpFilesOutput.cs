using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.File
{
    public class GetAbpFilesOutput
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public long FileSize { get; set; }


        public int Sort { get; set; }

    }

    public class GetMultiAbpFilesOutput
    {
        public string BusinessId { get; set; }

        public List<GetAbpFilesOutput> Files { get; set; } = new List<GetAbpFilesOutput>();
    }
}
