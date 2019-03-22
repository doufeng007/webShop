using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using ZCYX.FRMSCore;
using Abp.AutoMapper;

namespace HR
{
	[AutoMapFrom(typeof(EmployeeAdjustPost))]
    public class EmployeeAdjustPostLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 原部门
        /// </summary>
        [LogColumn(@"原部门", IsLog = true)]
        public long OriginalDepId { get; set; }

        /// <summary>
        /// 原岗位
        /// </summary>
        [LogColumn(@"原岗位", IsLog = true)]
        public Guid OriginalPostId { get; set; }

        /// <summary>
        /// 情况说明
        /// </summary>
        [LogColumn(@"情况说明", IsLog = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 调入部门
        /// </summary>
        [LogColumn(@"调入部门", IsLog = true)]
        public string AdjustDepName { get; set; }

        /// <summary>
        /// 申请职位
        /// </summary>
        [LogColumn(@"申请职位", IsLog = true)]
        public string AdjustPostName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [LogColumn(@"DealWithUsers", IsLog = true)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [LogColumn(@"Status", IsLog = true)]
        public int? Status { get; set; }

        /// <summary>
        /// WorkflowAdjsutDepId
        /// </summary>
        [LogColumn(@"WorkflowAdjsutDepId", IsLog = true)]
        public string WorkflowAdjsutDepId { get; set; }


        #endregion
    }
}