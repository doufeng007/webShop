using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace Train
{
    public class UpdateTrainUserExperienceInput 
    {
        /// <summary>
        /// 领导批示
        /// </summary>
        public string Approval { get; set; }
        public Guid Id { get; set; }
    }
}