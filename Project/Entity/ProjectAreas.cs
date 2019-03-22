using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 片区管理
    /// </summary>
   public class ProjectAreas: CreationAuditedEntity<Guid>
    {
        /// <summary>
        /// 片区名称
        /// </summary>
        [Required(ErrorMessage ="必须输入片区名")]
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        [Required(ErrorMessage ="必须选择负责人")]
        public long? User_Id { get; set; }
        /// <summary>
        /// 上级片区
        /// </summary>
        public Guid? Parent_Id { get; set; }


        
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool? IsDeleted { get; set; } = false;

    }
}
