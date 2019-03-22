using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project
{
    /// <summary>
    /// 分派部门id
    /// </summary>
    public  class UpdateSingleDepartmentInput
    {
        /// <summary>
        /// 项目id
        /// </summary>
        [Required(ErrorMessage = "单项id必填")]
        public Guid SingleProjectId { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        [Required(ErrorMessage ="请选择要分派的部门")]
        public long OrgId { get; set; }
    }


    public class UpdateProjectDepartmentInput
    {
        public Guid ProjectId { get; set; }

        public List<UpdateSingleDepartmentInput> Single { get; set; } = new List<UpdateSingleDepartmentInput>();
    }
}
