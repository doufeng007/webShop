using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeReceiptV2))]
    public class EmployeeReceiptV2ListOutputDto : BusinessWorkFlowListOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 部门编号
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        public string PostName { get; set; }

        /// <summary>
        /// 需求人数
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 岗位需求
        /// </summary>
        public Guid PostDemand { get; set; }

        /// <summary>
        /// 需求等级
        /// </summary>
        public Guid DemandGrade { get; set; }

        /// <summary>
        /// 需求岗位名称
        /// </summary>
        public string PostDemandName { get; set; }

        /// <summary>
        /// 需求等级名称
        /// </summary>
        public string DemandGradeName { get; set; }

        /// <summary>
        /// 性别要求
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 年龄要求
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 学历要求
        /// </summary>
        public Guid Education { get; set; }

        /// <summary>
        /// 学历要求名称
        /// </summary>
        public string EducationName { get; set; }

        /// <summary>
        /// 专业要求
        /// </summary>
        public string ProfessionalRequirements { get; set; }

        /// <summary>
        /// 技能要求
        /// </summary>
        public string SkillRequirement { get; set; }

        /// <summary>
        /// 证书要求
        /// </summary>
        public string CertificateRequirements { get; set; }

        /// <summary>
        /// 其他要求
        /// </summary>
        public string OtherRequirements { get; set; }

        /// <summary>
        /// 工作职责
        /// </summary>
        public string OperatingDuty { get; set; }

        /// <summary>
        /// 薪资建议
        /// </summary>
        public string SalaryProposal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }


    }
}
