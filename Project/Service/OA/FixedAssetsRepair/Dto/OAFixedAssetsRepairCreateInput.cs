﻿using System;
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
    [AutoMapTo(typeof(OAFixedAssetsRepair))]
    public class OAFixedAssetsRepairCreateInput: CreateWorkFlowInstance
    {

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
        public string FAName { get; set; }

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

        public decimal PreCost { get; set; }

        public decimal Cost { get; set; }

        public DateTime? RequestComplateDate { get; set; }

        public DateTime? ComplateDate { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; }


        public OAFixedAssetsRepairCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }
    }



    
  
}
