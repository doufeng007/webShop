using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR
{
    [AutoMapTo(typeof(EmployeeResume))]
    public class CreateEmployeeResumeInput
    {
        #region 表字段
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(30)]        
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Required]
        public int Age { get; set; }

        /// <summary>
        /// 期望职位
        /// </summary>
        [Required]
        public string Position { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 薪酬
        /// </summary>
        public decimal? Salary { get; set; }

        /// <summary>
        /// 薪酬(起薪)
        /// </summary>
        public decimal? StartingSalary { get; set; }

        /// <summary>
        /// 薪酬(是否面议)
        /// </summary>
        public bool IsFace { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 岗位经验
        /// </summary>
        [Required]
        public int Experience { get; set; }


        /// <summary>
        /// 教育经历
        /// </summary>
        public List<EducationExperienceDto> EducationExperience { get; set; }
        /// <summary>
        /// 工作经历
        /// </summary>
        public List<WorkExperienceDto> WorkExperience { get; set; }
        
        /// <summary>
        /// 项目经历
        /// </summary>
        public List<EmployeeProjecExperienceDto> ProjecExperience { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }


        public CreateEmployeeResumeInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }

        #endregion
    }
}