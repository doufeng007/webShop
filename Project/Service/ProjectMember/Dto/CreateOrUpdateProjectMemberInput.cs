using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    
    public class ManagerProjectAuditMembersInput
    {
        public Guid ProjectId { get; set; }

        public List<CreateOrUpdateProjectAuditMembersInput> ProjectAuditMembersInput { get; set; }

        public ManagerProjectAuditMembersInput()
        {
            this.ProjectAuditMembersInput = new List<CreateOrUpdateProjectAuditMembersInput>();
        }

    }

    public class BeginFlowerDto {
       public Guid taskId { get; set; }
        public string instanceId { get; set; }
        public int type { get; set; }
    }
}
