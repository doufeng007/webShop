using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace HR
{
    public class UpdateEmployeeResumeInput : EmployeeResume
    {
        //public Guid FlowId { get; set; }
        //public bool IsUpdateForChange { get; set; }

        /// <summary>
        /// 教育经历
        /// </summary>
        public List<EducationExperienceDto> EducationExperience { get; set; }
        /// <summary>
        /// 工作经历
        /// </summary>
        public List<WorkExperienceDto> WorkExperience { get; set; }

        /// <summary>
        ///项目经历
        /// </summary>
        public List<EmployeeProjecExperienceDto> ProjecExperience { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }


        public UpdateEmployeeResumeInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }
    }
}