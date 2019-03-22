using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace Project
{
    public class CreateOrUpdateProjectAuditGroupInput
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<CreateOrUpdateProjectAuditGroupUserInput> Users { get; set; }

        public CreateOrUpdateProjectAuditGroupInput()
        {
            Users = new List<CreateOrUpdateProjectAuditGroupUserInput>();
        }
    }

    [AutoMapFrom(typeof(ProjectAuditGroupUser))]
    public class CreateOrUpdateProjectAuditGroupUserInput
    {
        public Guid? Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public int UserRole { get; set; }

        public Guid GroupId { get; set; }
    }

}
