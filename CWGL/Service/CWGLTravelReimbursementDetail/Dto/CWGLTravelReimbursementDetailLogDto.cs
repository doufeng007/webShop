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
    [AutoMapFrom(typeof(CWGLTravelReimbursementDetail))]
    public class CWGLTravelReimbursementDetailLogDto
    {
        #region 表字段
        /// <summary>
        /// 编号
        /// </summary>
        [LogColumn(@"主键", IsLog = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// 起月
        /// </summary>
        [LogColumn(@"起始日期", IsLog = true)]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 截至日期
        /// </summary>
        [LogColumn(@"起日", IsLog = true)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 起止地点
        /// </summary>
        [LogColumn(@"起止地点", IsLog = true, IsNameField = true)]
        public string Address { get; set; }

        /// <summary>
        /// 交通工具
        /// </summary>
        [LogColumn(@"交通工具", IsLog = true)]
        public string Vehicle { get; set; }

        /// <summary>
        /// 出差天数
        /// </summary>
        [LogColumn(@"出差天数", IsLog = true)]
        public int? Day { get; set; }

        /// <summary>
        /// 交通费
        /// </summary>
        [LogColumn(@"交通费", IsLog = true)]
        public int? Fare { get; set; }

        /// <summary>
        /// 住宿费
        /// </summary>
        [LogColumn(@"住宿费", IsLog = true)]
        public int? Accommodation { get; set; }

        /// <summary>
        /// 其他费
        /// </summary>
        [LogColumn(@"其他费", IsLog = true)]
        public int? Other { get; set; }


        #endregion
    }
}