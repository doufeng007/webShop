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
using HR.Enum;

namespace HR
{
    [AutoMapFrom(typeof(HrSystem))]
    public class HrSystemOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public HrSystemType TypeId { get; set; }
        public string TypeName { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        public string UserIds { get; set; }

        /// <summary>
        /// 是否全公司
        /// </summary>
        public bool? IsAll { get; set; }

        /// <summary>
        /// 部门权限
        /// </summary>
        public string OrgIds { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        public string UserNames { get; set; }

        /// <summary>
        /// 部门权限
        /// </summary>
        public string OrgNames { get; set; }


		
    }
    public class HrSystemTypeOutput {
        public HrSystemType Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
