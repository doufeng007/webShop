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
using Abp.File;
using Project;

namespace GWGL
{
    [Table("EmployeeReceipt")]
    public class EmployeeReceipt : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName(@"标题")]
        [MaxLength(200)]
        public string Title { get; set; }

        public long ReceiptNo { get; set; }
        /// <summary>
        /// 来文机关
        /// </summary>
        [DisplayName(@"来文机关")]
        [MaxLength(100)]
        public string DocReceiveDep { get; set; }

        /// <summary>
        /// 请示报告事项
        /// </summary>
        [DisplayName(@"请示报告事项")]
        public string ReportMatters { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 拟办意见
        /// </summary>
        [DisplayName(@"拟办意见")]
        public string Opinion { get; set; }

        /// <summary>
        /// 来文文号
        /// </summary>
        [DisplayName(@"来文文号")]
        [MaxLength(50)]
        public string DocReceiveNo { get; set; }

        /// <summary>
        /// 公文类别
        /// </summary>
        [DisplayName(@"公文类别")]
        public Guid? DocType { get; set; }

        /// <summary>
        /// 公文属性
        /// </summary>
        [DisplayName(@"公文属性")]
        public ReceiptDocProperty DocProperty { get; set; }

        /// <summary>
        /// 密级
        /// </summary>
        [DisplayName(@"密级")]
        public RankProperty? Rank { get; set; }

        /// <summary>
        /// 紧急程度
        /// </summary>
        [DisplayName(@"紧急程度")]
        public EmergencyDegreeProperty? EmergencyDegree { get; set; }


        /// <summary>
        /// 抄送类型
        /// </summary>
        [DisplayName(@"抄送类型")]
        public CopyForTypeProperty? CopyForType { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        [DisplayName(@"任务类型")]
        public TaskTypeProperty? TaskType { get; set; }
        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [DisplayName(@"Status")]
        public int? Status { get; set; }

        /// <summary>
        /// CopyForUsers
        /// </summary>
        [DisplayName(@"CopyForUsers")]
        public string CopyForUsers { get; set; }

        /// <summary>
        /// QrCodeId
        /// </summary>
        [DisplayName(@"QrCodeId")]
        public Guid? QrCodeId { get; set; }

        /// <summary>
        /// IsPrintQrcode
        /// </summary>
        [DisplayName(@"IsPrintQrcode")]
        public bool IsPrintQrcode { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}