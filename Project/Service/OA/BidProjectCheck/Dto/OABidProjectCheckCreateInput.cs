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
    [AutoMapTo(typeof(OABidProjectCheck))]
    public class OABidProjectCheckCreateInput: CreateWorkFlowInstance
    {

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




        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();



        public OABidProjectCheckCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }



    
  
}
