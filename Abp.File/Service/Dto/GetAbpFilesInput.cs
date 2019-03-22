using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abp.File
{
    public class GetAbpFilesInput
    {
        [Required(ErrorMessage = "文件的BusinessId不能为空。")]
        
        public string BusinessId { get; set; }

        public int BusinessType { get; set; }

    }

    public class CreateFileRelationsInput : GetAbpFilesInput
    {
        public List<AbpFileListInput> Files { get; set; }

        public CreateFileRelationsInput()
        {
            this.Files = new List<AbpFileListInput>();
        }
    }

    public class AbpFileListInput
    {
        public Guid Id { get; set; }

        public int Sort { get; set; }
    }
}
