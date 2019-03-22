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
    [AutoMapFrom(typeof(OABidProjectCheck))]
    public class OABidProjectCheckDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectId_Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[DisplayName("Amount")]
        //public string Amount { get; set; }


        public string Participant { get; set; }

        public string ParticipantTxt { get; set; }

        public string Summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }

        public string ApplyUser_Name { get; set; }

        public string Participant_Name { get; set; }



        public OABidProjectCheckDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }

    }


}
