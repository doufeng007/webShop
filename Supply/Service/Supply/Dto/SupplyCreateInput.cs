using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.File;
using Abp.WorkFlow.Service.Dto;

namespace Supply
{
    [AutoMapTo(typeof(SupplyBase))]
    public class SupplyCreateInput
    {
        public string Name { get; set; }
        public string Unit { get; set; }

        public string Version { get; set; }

        [Required(ErrorMessage ="金额字段必填")]
        public decimal Money { get; set; }


        public int Type { get; set; }

        public string UserId { get; set; }

        //public int Status { get; set; }

        public string Code { get; set; }

        public DateTime? ProductDate { get; set; }


        public DateTime? ExpiryDate { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? PutInDate { get; set; }


        public SupplyCreateInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }
}
