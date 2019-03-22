using Abp.AutoMapper;
using Abp.File;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace EmailServer
{
    [AutoMapTo(typeof(EmailLog))]
    public class CreateEmailLogInput 
    {
        #region 表字段

        /// <summary>
        /// To
        /// </summary>
        public List<long> To { get; set; } = new List<long>();
        public List<string> ToEmail { get; set; } = new List<string>();

        /// <summary>
        /// CC
        /// </summary>
        public List<long> CC { get; set; } = new List<long>();
        public List<string> CCEmail { get; set; } = new List<string>();
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body
        /// </summary>
        public string Body { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

        #endregion
    }
}