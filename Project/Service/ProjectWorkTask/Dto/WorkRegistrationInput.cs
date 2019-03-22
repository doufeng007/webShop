using Abp.AutoMapper;
using Abp.File;
using Abp.Runtime.Validation;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project
{
    public class WorkRegistrationInput : CreateWorkFlowInstance
    {

        public string Title { get; set; }

        public string Code { get; set; }

        public Guid? StepId { get; set; }

        /// <summary>
        /// 1工作联系  2意见征询
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }

        public Guid? ProjectId { get; set; }

        public string Content { get; set; }

        public DateTime WorkTime { get; set; }

        public List<Guid> RegistrationIds { get; set; }
        public bool IsPersonOnCharge { get; set; }


        public List<GetAbpFilesOutput> Files { get; set; } = new List<GetAbpFilesOutput>();
    }

    public class UpdateWorkRegistrationInput: WorkRegistrationInput
    {
        public Guid Id { get; set; }
    }
}