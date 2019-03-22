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
    [AutoMapTo(typeof(OABidProject))]
    public class OABidProjectCreateInput: CreateWorkFlowInstance
    {

        /// <summary>
        /// 
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectSummary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WriteDate { get; set; }

        public string WriteUser { get; set; }

        public string WriteUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNature { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectNatureCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Bidder { get; set; }


        public string BidderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BidPreDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal BidPreFee { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal BidPreContractAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BuildUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContactsTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SignUnit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SupUnit { get; set; }

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



        public OABidProjectCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }



    
  
}
