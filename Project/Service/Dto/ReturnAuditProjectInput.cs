using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project
{
    /// <summary>
    /// 输入
    /// </summary>
    public class ReturnAuditProjectInput
    {
        /// <summary>
        /// *项目编号
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// *步骤id
        /// </summary>
        [Required]
        public Guid StepId { get; set; }



        /// <summary>
        /// 描述
        /// </summary>
        public string ReturnAuditSmmary { get; set; }
    }
}
