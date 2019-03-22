﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Castle.Components.DictionaryAdapter;
using Abp.File;

namespace Project
{
    [AutoMapTo(typeof(OACompletionSettlement))]
    public class OACompletionSettlementUpdateInput
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

        public string ProjectAdress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContractId { get; set; }

        public string ContractName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal ContractAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UnitA { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public decimal SettlementAmount { get; set; }

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
        public int Status { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }



        public OACompletionSettlementUpdateInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }




}
