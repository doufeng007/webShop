using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(RoleRelation))]
    public class RoleRelationListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 关联id
        /// </summary>
        public Guid RelationId { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        public RelationType Type { get; set; }

        /// <summary>
        /// 关联用户
        /// </summary>
        public long RelationUserId { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 转让角色
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
