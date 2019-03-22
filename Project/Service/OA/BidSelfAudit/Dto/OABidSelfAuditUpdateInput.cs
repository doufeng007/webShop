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
    [AutoMapTo(typeof(OABidSelfAudit))]
    public class OABidSelfAuditUpdateInput
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
        public string ProjectName { get; set; }

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



        public OABidSelfAuditUpdateInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }




}
