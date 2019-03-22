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
using System.ComponentModel.DataAnnotations;

namespace HR
{
    [AutoMap(typeof(EmployeeProjecExperience))]
    public class EmployeeProjecExperienceDto 
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 担任职位
        /// </summary>
        [Required]
        public string Position { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 项目内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }



		
    }
}
