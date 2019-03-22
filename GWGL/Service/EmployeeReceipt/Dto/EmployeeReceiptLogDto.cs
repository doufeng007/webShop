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

namespace GWGL
{
	[AutoMapFrom(typeof(EmployeeReceipt))]
    public class EmployeeReceiptLogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 收文编号
        /// </summary>
        [LogColumn(@"收文编号", IsLog = true)]
        public long ReceiptNo { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [LogColumn(@"标题", IsLog = true)]
        public string Title { get; set; }

        /// <summary>
        /// 来文机关
        /// </summary>
        [LogColumn(@"来文机关", IsLog = true)]
        public string DocReceiveDep { get; set; }

        /// <summary>
        /// 请示报告事项
        /// </summary>
        [LogColumn(@"请示报告事项", IsLog = true)]
        public string ReportMatters { get; set; }

        /// <summary>
        /// 拟办意见
        /// </summary>
        [LogColumn(@"拟办意见", IsLog = true)]
        public string Opinion { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [LogColumn(@"备注", IsLog = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 来文文号
        /// </summary>
        [LogColumn(@"来文文号", IsLog = true)]
        public string DocReceiveNo { get; set; }

        /// <summary>
        /// 公文类别
        /// </summary>
        [LogColumn(@"公文类别", IsLog = true)]
        public string DocType { get; set; }

        /// <summary>
        /// 公文属性
        /// </summary>
        [LogColumn(@"公文属性", IsLog = true)]
        public string DocProperty { get; set; }

        /// <summary>
        /// 密级
        /// </summary>
        [LogColumn(@"密级", IsLog = true)]
        public string Rank { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        [LogColumn(@"紧急程度", IsLog = true)]
        public string EmergencyDegree { get; set; }




        [LogColumn("附件", IsLog = true)]
        public List<AbpFileChangeDto> Files { get; set; } = new List<AbpFileChangeDto>();
        #endregion
    }
}