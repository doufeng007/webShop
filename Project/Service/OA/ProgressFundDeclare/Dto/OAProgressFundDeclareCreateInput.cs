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
    [AutoMapTo(typeof(OAProgressFundDeclare))]
    public class OAProgressFundDeclareCreateInput: CreateWorkFlowInstance
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ProjectId { get; set; }


        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContractId { get; set; }

        public string ContractName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UnitA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SettlementType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SettlementTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal ReplyAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Debit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Fine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long WriteUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WriteData { get; set; }

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
        //public int Status { get; set; }




        public List<GetAbpFilesOutput> FileList { get; set; }



        public OAProgressFundDeclareCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }



    
  
}
