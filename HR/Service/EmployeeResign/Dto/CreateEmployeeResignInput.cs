using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZCYX.FRMSCore;

namespace HR
{
    /// <summary>
    /// 创建离职申请
    /// </summary>
    public class CreateEmployeeResignInput: CreateWorkFlowInstance
    {

        /// <summary>
        /// 离职类别
        /// </summary>
        [Required]
        public EmployeeResignType Type { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        [Required]
        [MaxLength(200, ErrorMessage ="不能超过200个字符")]
        public string Reason { get; set; }
    }
    /// <summary>
    /// 更新申请
    /// </summary>
    [AutoMap(typeof(EmployeeResign))]
    public class UpdateEmployeeResignInput : CreateWorkFlowInstance
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 离职类别
        /// </summary>
        public EmployeeResignType Type { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }

        public bool IsUpdateForChange { get; set; }
    }
    /// <summary>
    /// 处理变更记录
    /// </summary>
    [AutoMapFrom(typeof(EmployeeResign))]
    public class UpdateEmployeeResignLogDto 
    {
        /// <summary>
        /// 离职类别
        /// </summary>
        [LogColumn(@"离职类别", IsLog = true)]
        public string Type { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        [LogColumn(@"原因", IsLog = true)]
        public string Reason { get; set; }
    }
}
