using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(ProjectAuditGroup))]
    public class GetProjectAuditGroupForEditOutput
    {

        public Guid? Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public List<CreateOrUpdateProjectAuditGroupUserInput> Users { get; set; }

        public GetProjectAuditGroupForEditOutput()
        {
            Users = new List<CreateOrUpdateProjectAuditGroupUserInput>();
        }
    }


}
