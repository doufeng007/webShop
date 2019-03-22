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

namespace CWGL
{
    [AutoMapFrom(typeof(AccountantCourse))]
    public class AccountantCourseOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid? Pid { get; set; }

        /// <summary>
        /// parent_left
        /// </summary>
        public int parent_left { get; set; }

        /// <summary>
        /// parent_right
        /// </summary>
        public int parent_right { get; set; }

        


		
    }
}
