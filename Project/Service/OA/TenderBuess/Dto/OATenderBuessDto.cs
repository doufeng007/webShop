using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project

{
    [AutoMapFrom(typeof(OATenderBuess))]
    public class OATenderBuessDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectId_Name { get; set; }

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

        ///// <summary>
        ///// 
        ///// </summary>
        //public string Remark { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }

        //public string ApplyUserName { get; set; }


        public OATenderBuessDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }

    }


}
