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
    [AutoMapFrom(typeof(OAFixedAssetsScrap))]
    public class OAFixedAssetsScrapDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid FAId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FAId_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }


        public string Reason { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }



        public string ApplyUserId_Name { get; set; }

        public OAFixedAssetsScrapDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }

    }


}
