using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace XZGL
{
    [AutoMapFrom(typeof(XZGLCompany))]
    public class XZGLCompanyListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Guid Type { get; set; }
        public string TypeName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public long? UserId { get; set; }
        public string  UserName { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }
        public bool IsFollow { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
