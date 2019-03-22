using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeTrainingSystemUnitPosts))]
    public class EmployeeTrainingSystemUnitPostsOutputDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 制度编号
        /// </summary>
        public Guid SysId { get; set; }

        /// <summary>
        /// 部门岗位编号
        /// </summary>
        public Guid PortsId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
