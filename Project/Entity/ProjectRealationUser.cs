using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project
{
    /// <summary>
    /// 项目干系人
    /// </summary>
    [Table("ProjectRealationUser")]
    public class ProjectRealationUser : Entity<Guid>
    {

        public Guid InstanceID { get; set; }

        public Guid FlowID { get; set; }

        public Guid StepID { get; set; }

        public long UserID { get; set; }

        public Guid ProjectId { get; set; }
    }
}
