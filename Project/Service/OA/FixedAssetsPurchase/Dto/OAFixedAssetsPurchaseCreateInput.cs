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
    [AutoMapTo(typeof(OAFixedAssetsPurchase))]
    public class OAFixedAssetsPurchaseCreateInput: CreateWorkFlowInstance
    {

        public string Code { get; set; }
        public string ApplyUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApplyTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FeeSourceCode { get; set; }


        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string File { get; set; }

        public string Reason { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; }

        public List<OAFixedAssetsCreateInput> FixedAssetss { get; set; }


        public OAFixedAssetsPurchaseCreateInput()
        {
            this.FileList = new List<GetAbpFilesOutput>();
            this.FixedAssetss = new List<OAFixedAssetsCreateInput>();
        }
    }



    
  
}
