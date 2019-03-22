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

namespace Project
{
    [AutoMapTo(typeof(OATenderBuess))]
    public class OATenderBuessCreateInput: CreateWorkFlowInstance
    {

        public string Code { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectType { get; set; }
        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal? CashPrice { get; set; }

        public string CashPriceUp { get; set; }

        public string Des { get; set; }

        public string Files { get; set; }

        public int Status { get; set; }

        //public DateTime ApplyDate { get; set; }

        //public string ApplyUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public string Remark { get; set; }




        public List<GetAbpFilesOutput> FileList { get; set; }



        public OATenderBuessCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }



    
  
}
