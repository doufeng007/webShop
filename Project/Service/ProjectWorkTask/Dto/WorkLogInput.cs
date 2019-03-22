using Abp.AutoMapper;
using Abp.File;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project
{
    public class WorkLogInput
    {

        public string Title { get; set; }

        public Guid? StepId { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }

        public Guid? ProjectId { get; set; }

        public string Content { get; set; }

        public DateTime WorkTime { get; set; }
        
        public int? LogType { get; set; }

        public List<GetAbpFilesOutput> Files { get; set; }
    }
}