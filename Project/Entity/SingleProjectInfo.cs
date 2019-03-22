using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using ZCYX.FRMSCore;

namespace Project
{
    [Serializable]
    [TableNameAtribute("单项信息")]
    [Table("SingleProjectInfo")]
    /// <summary>
    /// [单表映射]
    /// </summary>
    public class SingleProjectInfo : FullAuditedEntity<Guid>
    {
        #region 表字段
        public SingleProjectInfo DeepClone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as SingleProjectInfo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("ProjectId")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SingleProjectName")]
        public string SingleProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SingleProjectCode")]
        public string SingleProjectCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SingleProjectbudget")]
        public decimal SingleProjectbudget { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("SingleProjectSafaBudget")]
        public decimal SingleProjectSafaBudget { get; set; }


        /// <summary>
        /// 项目性质
        /// </summary>
        [DisplayName("ProjectNature")]
        public string ProjectNature { get; set; }


        /// <summary>
        /// 行业
        /// </summary>
        [DisplayName("Industry")]
        public string Industry { get; set; }


        public string Industry_Name { get; set; }



        /// <summary>
        /// 分派的部门ID（l_orgid）
        /// </summary>
        public string DeparmentId { get; set; }


        public Guid? GroupId { get; set; }


        public string ProjectCode { get; set; }

        public bool? IsReturnAudit { get; set; }

        /// <summary>
        /// 0 未停滞 1停滞申请  2 待解除停滞  3解除停滞
        /// </summary>
        public ProjectStopTypeEnum IsStop { get; set; }


        public int Status { get; set; }


        public string ReturnAuditSmmary { get; set; }


        public ProjectStatus? ProjectStatus { get; set; }

        public DateTime? ReadyEndTime { get; set; }


        public DateTime? ReadyStartTime { get; set; }

        public decimal? AuditAmount { get; set; }


        public bool? HasFinancialReview { get; set; }

        #endregion

    }


}