using System.ComponentModel.DataAnnotations;
using Abp.Organizations;
using System.Collections.Generic;
using System;

namespace ZCYX.FRMSCore.Application
{
    public class CreateWorkFlowOrganizationUnitInput
    {
        public long? ParentId { get; set; }

        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        /// <summary>
        ///  0单位 1 部门 2 人员  3 部门领导 4 部门直属成员
        /// </summary>
        public int Type { get; set; }

        public string ChargeLeader { get; set; }

        public string Note { get; set; }


        public List<CreateOrganizationUnitPostInput> Posts { get; set; }

        public CreateWorkFlowOrganizationUnitInput()
        {
            Posts = new List<CreateOrganizationUnitPostInput>();
        }
    }


    /// <summary>
    /// 岗位信息
    /// </summary>
    public class CreateOrganizationUnitPostInput
    {

        public Guid? PostId { get; set; }

        [Required]
        public string PostName { get; set; }

        public int PrepareNumber { get; set; } = 0;
        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public Level? Level { get; set; }
        /// <summary>
        /// 岗位角色
        /// </summary>
        public List<string> RoleNames { get; set; }
    }
    public enum Level {
        分管领导=0,
        部门领导=1,
        员工=2,
        实习生=3
    }
    public class UpdateOrganizationUnitPostInput : CreateOrganizationUnitPostInput
    {
        public Guid? Id { get; set; }

    }

    public class UpdateOrganizationName {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }
}