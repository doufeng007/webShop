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

namespace CWGL
{
	[AutoMapFrom(typeof(CWGLWagePay))]
    public class CWGLWagePayLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 工资日期
        /// </summary>
        [LogColumn(@"工资日期", IsLog = true)]
        public DateTime WageDate { get; set; }

        /// <summary>
        /// 发放时间
        /// </summary>
        [LogColumn(@"发放时间", IsLog = true)]
        public DateTime DoTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 流程查阅人员
        /// </summary>
        [LogColumn(@"流程查阅人员", IsLog = true)]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [LogColumn(@"Status", IsLog = true)]
        public int Status { get; set; }


        #endregion
    }
}