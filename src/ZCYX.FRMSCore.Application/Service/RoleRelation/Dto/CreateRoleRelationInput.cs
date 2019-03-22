using Abp.AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapTo(typeof(RoleRelation))]
    public class CreateRoleRelationInput 
    {
        #region 表字段

        /// <summary>
        /// 关联id
        /// </summary>
        public Guid RelationId { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
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


		
        #endregion
    }
}