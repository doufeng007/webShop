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
    [AutoMapFrom(typeof(OAContract))]
    public class OAContractDto: WorkFlowTaskCommentResult
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


        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContractType { get; set; }


        public string ContractTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Amount { get; set; }

        public string UnitA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long SigningUser { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SettlementType { get; set; }

        public string PayTypeCode { get; set; }

        public string SettlementTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal PreAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal Deposit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SignData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Conditions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MainConditions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        public string UnitAContract { get; set; }

        public string UnitAContractTel { get; set; }


        public string UnitAContractAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }


        public string SigningUser_Name { get; set; }

        public OAContractDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }

    }


}
