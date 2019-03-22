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

namespace HR
{
    [Serializable]
    [Table("EmployeeReceiptV2")]
    public class EmployeeReceiptV2 : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        #region 表字段
        
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName(@"状态")]
        public int Status { get; set; }

        /// <summary>
        /// DealWithUsers
        /// </summary>
        [DisplayName(@"DealWithUsers")]
        public string DealWithUsers { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [DisplayName(@"部门编号")]
        public long DepartmentId { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [DisplayName(@"岗位名称")]
        [MaxLength(200)]
        [Required]
        public string PostName { get; set; }

        /// <summary>
        /// 需求人数
        /// </summary>
        [DisplayName(@"需求人数")]
        [MaxLength(200)]
        [Required]
        public string Number { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        [DisplayName(@"工作地点")]
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 岗位需求
        /// </summary>
        [DisplayName(@"岗位需求")]
        public Guid PostDemand { get; set; }

        /// <summary>
        /// 需求等级
        /// </summary>
        [DisplayName(@"需求等级")]
        public Guid DemandGrade { get; set; }

        /// <summary>
        /// 需求岗位名称
        /// </summary>
        [DisplayName(@"需求岗位名称")]
        [MaxLength(200)]
        [Required]
        public string PostDemandName { get; set; }

        /// <summary>
        /// 需求等级名称
        /// </summary>
        [DisplayName(@"需求等级名称")]
        [MaxLength(200)]
        [Required]
        public string DemandGradeName { get; set; }

        /// <summary>
        /// 性别要求
        /// </summary>
        [DisplayName(@"性别要求")]
        public int Sex { get; set; }

        /// <summary>
        /// 年龄要求
        /// </summary>
        [DisplayName(@"年龄要求")]
        [MaxLength(200)]
        [Required]
        public string Age { get; set; }

        /// <summary>
        /// 学历要求
        /// </summary>
        [DisplayName(@"学历要求")]
        public Guid Education { get; set; }

        /// <summary>
        /// 学历要求名称
        /// </summary>
        [DisplayName(@"学历要求名称")]
        [MaxLength(200)]
        [Required]
        public string EducationName { get; set; }

        /// <summary>
        /// 专业要求
        /// </summary>
        [DisplayName(@"专业要求")]
        [MaxLength(200)]
        [Required]
        public string ProfessionalRequirements { get; set; }

        /// <summary>
        /// 技能要求
        /// </summary>
        [DisplayName(@"技能要求")]
        [MaxLength(200)]
        [Required]
        public string SkillRequirement { get; set; }

        /// <summary>
        /// 证书要求
        /// </summary>
        [DisplayName(@"证书要求")]
        [MaxLength(200)]
        [Required]
        public string CertificateRequirements { get; set; }

        /// <summary>
        /// 其他要求
        /// </summary>
        [DisplayName(@"其他要求")]
        [MaxLength(200)]
        public string OtherRequirements { get; set; }

        /// <summary>
        /// 工作职责
        /// </summary>
        [DisplayName(@"工作职责")]
        [MaxLength(200)]
        [Required]
        public string OperatingDuty { get; set; }

        /// <summary>
        /// 薪资建议
        /// </summary>
        [DisplayName(@"薪资建议")]
        [MaxLength(200)]
        [Required]
        public string SalaryProposal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName(@"备注")]
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DisplayName(@"部门名称")]
        [MaxLength(200)]
        [Required]
        public string DepartmentName { get; set; }


        #endregion
    }
}