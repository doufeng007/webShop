using System.ComponentModel.DataAnnotations;
using Abp.Organizations;
using System.Collections.Generic;
using System;

namespace ZCYX.FRMSCore.Application
{
    public class UpdateWorkFlowOrganizationUnitInput
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }


        public int Type { get; set; }
        /// <summary>
        /// 部门领导
        /// </summary>
        public string ChargeLeader { get; set; }

        public string Note { get; set; }

        public List<UpdateOrganizationUnitPostInput> Posts { get; set; }

        public UpdateWorkFlowOrganizationUnitInput()
        {
            this.Posts = new List<UpdateOrganizationUnitPostInput>();
        }
    }
}