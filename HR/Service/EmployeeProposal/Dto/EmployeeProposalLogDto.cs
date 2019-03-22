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
	[AutoMapFrom(typeof(EmployeeProposal))]
    public class EmployeeProposalLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [LogColumn(@"类型", IsLog = true)]
        public string TypeName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [LogColumn(@"标题", IsLog = true)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [LogColumn(@"内容", IsLog = true)]
        public string Content { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        [LogColumn(@"处理人", IsLog = true)]
        public string ParticipateUser { get; set; }

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
        /// CopyForUsers
        /// </summary>
        [LogColumn(@"CopyForUsers", IsLog = true)]
        public string CopyForUsers { get; set; }

        /// <summary>
        /// 回复
        /// </summary>
        [LogColumn(@"回复", IsLog = true)]
        public string Comment { get; set; }


        #endregion
    }
}