using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeResume))]
    public class EmployeeResumeListOutputDto 
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
        /// 是否已提取
        /// </summary>
        public bool IsExtract { get; set; }

        public ResumeStatus Status { get; set; }

        public string StatusTitle { get; set; }
    }
}
