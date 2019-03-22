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

namespace HR
{
	[AutoMapFrom(typeof(EmployeeReceiptV2))]
    public class EmployeeReceiptV2LogDto
    {
        #region 表字段
                /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }



        /// <summary>
        /// 岗位名称
        /// </summary>
        [LogColumn(@"岗位名称", IsLog = true)]
        public string PostName { get; set; }

        /// <summary>
        /// 需求人数
        /// </summary>
        [LogColumn(@"需求人数", IsLog = true)]
        public string Number { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        [LogColumn(@"工作地点", IsLog = true)]
        public string Address { get; set; }


        /// <summary>
        /// 需求岗位名称
        /// </summary>
        [LogColumn(@"需求岗位名称", IsLog = true)]
        public string PostDemandName { get; set; }

        /// <summary>
        /// 需求等级名称
        /// </summary>
        [LogColumn(@"需求等级名称", IsLog = true)]
        public string DemandGradeName { get; set; }

        /// <summary>
        /// 性别要求
        /// </summary>
        [LogColumn(@"性别要求", IsLog = true)]
        public int Sex { get; set; }

        /// <summary>
        /// 年龄要求
        /// </summary>
        [LogColumn(@"年龄要求", IsLog = true)]
        public string Age { get; set; }

        /// <summary>
        /// 学历要求名称
        /// </summary>
        [LogColumn(@"学历要求", IsLog = true)]
        public string EducationName { get; set; }

        /// <summary>
        /// 专业要求
        /// </summary>
        [LogColumn(@"专业要求", IsLog = true)]
        public string ProfessionalRequirements { get; set; }

        /// <summary>
        /// 技能要求
        /// </summary>
        [LogColumn(@"技能要求", IsLog = true)]
        public string SkillRequirement { get; set; }

        /// <summary>
        /// 证书要求
        /// </summary>
        [LogColumn(@"证书要求", IsLog = true)]
        public string CertificateRequirements { get; set; }

        /// <summary>
        /// 其他要求
        /// </summary>
        [LogColumn(@"其他要求", IsLog = true)]
        public string OtherRequirements { get; set; }

        /// <summary>
        /// 工作职责
        /// </summary>
        [LogColumn(@"工作职责", IsLog = true)]
        public string OperatingDuty { get; set; }

        /// <summary>
        /// 薪资建议
        /// </summary>
        [LogColumn(@"薪资建议", IsLog = true)]
        public string SalaryProposal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [LogColumn(@"备注", IsLog = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [LogColumn(@"部门", IsLog = true)]
        public string DepartmentName { get; set; }


        #endregion
    }
}