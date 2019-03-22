using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMapFrom(typeof(ProjcetAuditResultCheckRole))]
    public class ProjcetAuditResultCheckRoleDto
    {
        public Guid Id { get; set; }

        public Guid CategroyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal DeductionPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }
    }
}
