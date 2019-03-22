using Abp.WorkFlow.Service.Dto;
using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWGL
{


    public class CreateNoticeDocumentInput : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public int NoticeType { get; set; }


        public long? DispatchUnit { get; set; }


        public DateTime DispatchTime { get; set; }


        public int PrintNum { get; set; }


        public string DispatchCode { get; set; }


        public EmergencyDegreeProperty? Urgency { get; set; }


        public RankProperty? SecretLevel { get; set; }



        public string ReceiveId { get; set; }

        public string ReceiveName { get; set; }



        public string Reason { get; set; }


        public string Content { get; set; }


        public bool IsNeedRes { get; set; }


        public int Status { get; set; }


        public string FileInfo { get; set; }

        public Guid? ProjectId { get; set; }


        public Guid? ProjectRegistrationId { get; set; }


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
        public ReceiptDocProperty? DocumentTyep { get; set; }


        public string DispatchUnitName { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string ReplyContent { get; set; }

        public bool IsUpdateForChange { get; set; }

        public NoticeDocumentBusinessType NoticeDocumentBusinessType { get; set; }


        public bool IsNeedAddWrite { get; set; }


        public int AddType { get; set; }


        public int WriteType { get; set; }

        //public string AddWriteUsers { get; set; }


        public string AddWriteOrgIds { get; set; }

        public Guid GW_DocumentTypeId { get; set; }


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



    public class UpdateSupplierPrintInput
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 送达人
        /// </summary>
        public long DeliveryUser { get; set; }


        public int PrintNum { get; set; }


        public Guid SupplySupplierId { get; set; }


        public string SupplySupplierRemark { get; set; }
    }
}
