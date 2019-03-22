using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace HR
{
    [AutoMapTo(typeof(EmployeeReceiptV2))]
    public class CreateEmployeeReceiptV2Input : CreateWorkFlowInstance
    {
        #region 表字段

        /// <summary>
        /// 部门编号
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [MaxLength(200,ErrorMessage = "岗位名称长度必须小于200")]
        [Required(ErrorMessage = "必须填写岗位名称")]
        public string PostName { get; set; }

        /// <summary>
        /// 需求人数
        /// </summary>
        [MaxLength(200,ErrorMessage = "需求人数长度必须小于200")]
        [Required(ErrorMessage = "必须填写需求人数")]
        public string Number { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        [MaxLength(200,ErrorMessage = "工作地点长度必须小于200")]
        [Required(ErrorMessage = "必须填写工作地点")]
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
        [MaxLength(200,ErrorMessage = "需求岗位名称长度必须小于200")]
        [Required(ErrorMessage = "必须填写需求岗位名称")]
        public string PostDemandName { get; set; }

        /// <summary>
        /// 需求等级名称
        /// </summary>
        [MaxLength(200,ErrorMessage = "需求等级名称长度必须小于200")]
        [Required(ErrorMessage = "必须填写需求等级名称")]
        public string DemandGradeName { get; set; }

        /// <summary>
        /// 性别要求
        /// </summary>
        [Range(0, int.MaxValue,ErrorMessage="")]
        public int Sex { get; set; }

        /// <summary>
        /// 年龄要求
        /// </summary>
        [MaxLength(200,ErrorMessage = "年龄要求长度必须小于200")]
        [Required(ErrorMessage = "必须填写年龄要求")]
        public string Age { get; set; }

        /// <summary>
        /// 学历要求
        /// </summary>
        public Guid Education { get; set; }

        /// <summary>
        /// 学历要求名称
        /// </summary>
        [MaxLength(200,ErrorMessage = "学历要求名称长度必须小于200")]
        [Required(ErrorMessage = "必须填写学历要求名称")]
        public string EducationName { get; set; }

        /// <summary>
        /// 专业要求
        /// </summary>
        [MaxLength(200,ErrorMessage = "专业要求长度必须小于200")]
        [Required(ErrorMessage = "必须填写专业要求")]
        public string ProfessionalRequirements { get; set; }

        /// <summary>
        /// 技能要求
        /// </summary>
        [MaxLength(200,ErrorMessage = "技能要求长度必须小于200")]
        [Required(ErrorMessage = "必须填写技能要求")]
        public string SkillRequirement { get; set; }

        /// <summary>
        /// 证书要求
        /// </summary>
        [MaxLength(200,ErrorMessage = "证书要求长度必须小于200")]
        [Required(ErrorMessage = "必须填写证书要求")]
        public string CertificateRequirements { get; set; }

        /// <summary>
        /// 其他要求
        /// </summary>
        [MaxLength(200,ErrorMessage = "其他要求长度必须小于200")]
        public string OtherRequirements { get; set; }

        /// <summary>
        /// 工作职责
        /// </summary>
        [MaxLength(200,ErrorMessage = "工作职责长度必须小于200")]
        [Required(ErrorMessage = "必须填写工作职责")]
        public string OperatingDuty { get; set; }

        /// <summary>
        /// 薪资建议
        /// </summary>
        [MaxLength(200,ErrorMessage = "薪资建议长度必须小于200")]
        [Required(ErrorMessage = "必须填写薪资建议")]
        public string SalaryProposal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200,ErrorMessage = "备注长度必须小于200")]
        public string Remark { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [MaxLength(200,ErrorMessage = "部门名称长度必须小于200")]
        [Required(ErrorMessage = "必须填写部门名称")]
        public string DepartmentName { get; set; }


        #endregion
    }
}