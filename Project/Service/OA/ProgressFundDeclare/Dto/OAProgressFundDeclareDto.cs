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
    [AutoMapFrom(typeof(OAProgressFundDeclare))]
    public class OAProgressFundDeclareDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
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


        public string ProjectId_Name { get; set; }

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

        public string WriteUser_Name { get; set; }

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
        public int Status { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }


        public OAProgressFundDeclareDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }

    }


}
