using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Project;
using Abp.WorkFlow;

namespace GWGL
{
    [AutoMapTo(typeof(EmployeeReceipt))]
    public class CreateEmployeeReceiptInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 来文机关
        /// </summary>
        [Required]
        public string DocReceiveDep { get; set; }

        /// <summary>
        /// 请示报告事项
        /// </summary>
        public string ReportMatters { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 来文文号
        /// </summary>
        [Required]
        public string DocReceiveNo { get; set; }

        /// <summary>
        /// 公文类别
        /// </summary>
        public Guid? DocType { get; set; }

        /// <summary>
        /// 公文属性
        /// </summary>
        public ReceiptDocProperty DocProperty { get; set; }


        /// <summary>
        /// 密级
        /// </summary>
        public RankProperty? Rank { get; set; }

        public bool IsPrintQrcode { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public EmergencyDegreeProperty? EmergencyDegree { get; set; }



		public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
    public class EmployeeReceiptOutput : InitWorkFlowOutput
    {
        public Guid QrCodeId { get; set; }
    }
    public class EmployeeReceiptNextInput {
        public Guid FlowId { get; set; }
        public Guid InstanceId { get; set; }
    }
}