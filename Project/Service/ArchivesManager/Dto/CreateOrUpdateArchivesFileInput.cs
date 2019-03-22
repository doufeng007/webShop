using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;

namespace Project
{
    [AutoMap(typeof(ArchivesFile))]
    public class CreateOrUpdateArchivesFileInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }

        public Guid ArchivesId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPaper { get; set; }


        public List<FileUploadFiles> Files { get; set; }


        public int? PaperNumber { get; set; }


        public CreateOrUpdateArchivesFileInput()
        {
            this.Files = new EditableList<FileUploadFiles>();
        }
    }
}
