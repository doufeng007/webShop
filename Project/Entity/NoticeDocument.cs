using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Project
{
    [Table("NoticeDocument")]
    [Serializable]
    public class NoticeDocument : FullAuditedEntity<Guid>
    {
        public string Title { get; set; }

        public int NoticeType { get; set; }


        public long? DispatchUnit { get; set; }


        /// <summary>
        /// 发文时间
        /// </summary>
        public DateTime DispatchTime { get; set; }


        public int PrintNum { get; set; }


        public string DispatchCode { get; set; }


        public EmergencyDegreeProperty? Urgency { get; set; }


        public RankProperty? SecretLevel { get; set; }



        public string ReceiveId { get; set; }


        /// <summary>
        /// 抄送人
        /// </summary>
        public string ReceiveName { get; set; }



        public string Reason { get; set; }


        public string Content { get; set; }


        public bool IsNeedRes { get; set; }


        public int Status { get; set; }


        public string FileInfo { get; set; }

        public Guid? ProjectId { get; set; }

        public string DealWithUsers { get; set; }


        public Guid? ProjectRegistrationId { get; set; }


        /// <summary>
        /// 发文机关
        /// </summary>
        public string DispatchUnitName { get; set; }


        /// <summary>
        /// 发文人
        /// </summary>
        public long? PubilishUserId { get; set; }

        /// <summary>
        /// 发文人名称
        /// </summary>
        public string PubilishUserName { get; set; }

        /// <summary>
        /// 主送人
        /// </summary>
        public string MainReceiveName { get; set; }


        /// <summary>
        /// 公文类别
        /// </summary>
        public int? DocumentTyep { get; set; }


        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }


        public string CopyForUsers { get; set; }


        /// <summary>
        /// 公文业务类型
        /// </summary>
        public int NoticeDocumentBusinessType { get; set; }


        public bool IsNeedAddWrite { get; set; }


        public int AddType { get; set; }


        public int WriteType { get; set; }

        public string AddWriteUsers { get; set; }


        public string AddWriteOrgIds { get; set; }


        public Guid? GW_DocumentTypeId { get; set; }

        public string GW_DocumentTypeName { get; set; }

        /// <summary>
        /// 校对人
        /// </summary>
        public long? CheckUser { get; set; }

        /// <summary>
        /// 送达人
        /// </summary>
        public long? DeliveryUser { get; set; }


        public Guid? SupplySupplierId { get; set; }


        public string SupplySupplierRemark { get; set; }




        #region  批文签发内容
        public string Additional { get; set; }

        /// <summary>
        /// 评审负责人
        /// </summary>
        public string ProjectLeader { get; set; }

        /// <summary>
        /// 评审人员
        /// </summary>
        public string ProjectReviewer { get; set; }


        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// 建设单位
        /// </summary>
        public string SendUnitName { get; set; }

        public decimal? AuditAmount { get; set; }


        public string ProjectUndertakeCode { get; set; }


        #endregion



    }
}
