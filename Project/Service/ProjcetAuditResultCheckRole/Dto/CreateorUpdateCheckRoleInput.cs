using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMapTo(typeof(ProjcetAuditResultCheckRole))]
    public class CreateorUpdateCheckRoleInput
    {
        public Guid? Id { get; set; }

        public Guid CategroyId { get; set; }

        public string Content { get; set; }

        public string Unit { get; set; }

        public decimal DeductionPrice { get; set; }


        public string Summary { get; set; }


    }
}
