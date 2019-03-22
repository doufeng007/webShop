using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using Castle.Components.DictionaryAdapter;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeResume))]
    public class EmployeeResumeOutputDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 期望职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
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
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 岗位经验
        /// </summary>
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


        public EmployeeResumeOutputDto()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }

    }
}
