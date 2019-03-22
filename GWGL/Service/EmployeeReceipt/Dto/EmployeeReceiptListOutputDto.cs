using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using Project;

namespace GWGL
{
    [AutoMapFrom(typeof(EmployeeReceipt))]
    public class EmployeeReceiptListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 收文编号
        /// </summary>
        public long ReceiptNo { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 来文机关
        /// </summary>
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
        public string DocReceiveNo { get; set; }

        /// <summary>
        /// 公文类别
        /// </summary>
        public virtual Guid? DocType { get; set; }
        public string DocTypeName { get; set; }

        /// <summary>
        /// 公文属性
        /// </summary>
        public virtual ReceiptDocProperty? DocProperty { get; set; }
        public string DocPropertyName { get; set; }


        /// <summary>
        /// 密级
        /// </summary>
        public virtual RankProperty? Rank { get; set; }
        public string RankPropertyName { get; set; }
        /// <summary>
        /// 紧急程度
        /// </summary>
        public virtual EmergencyDegreeProperty? EmergencyDegree { get; set; }
        public string EmergencyDegreePropertyName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        public string DealWithUsers { get; set; }

        /// <summary>
        /// CopyForUsers
        /// </summary>
        public string CopyForUsers { get; set; }
        public string Opinion { get; set; }
        public long? CreateUserId { get; set; }
        public bool IsMe { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }
}
