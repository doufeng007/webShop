using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace ZCYX.FRMSCore.Application
{
    public class GetOrganizationUnitTreeOutput
    {

        public string Id { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }
        public long OrgId { get; set; }

        /// <summary>
        ///  0单位 1 部门 2 人员  3 部门领导 4 部门直属成员  5 部门所有人成员
        /// </summary>
        public int NodeType { get; set; }

        public List<GetOrganizationUnitTreeOutput> Children { get; set; }


        public GetOrganizationUnitTreeOutput()
        {
            this.Children = new List<GetOrganizationUnitTreeOutput>();
        }
    }

    public class OrganizationUnitTreeOutput
    {

        public long Id { get; set; }

        public long Value { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        /// <summary>
        ///  0单位 1 部门 2 人员  3 部门领导 4 部门直属成员
        /// </summary>
        public int NodeType { get; set; }

        public List<OrganizationUnitTreeOutput> Children { get; set; }


        public OrganizationUnitTreeOutput()
        {
            this.Children = new List<OrganizationUnitTreeOutput>();
        }
    }



    public class UserUnderOrgProssceStaticOutput : UserUnderOrgOutput
    {
        public int ProcessProjectCount { get; set; }

        public decimal ProcessProjectAmount { get; set; }
    }

    public class UserUnderOrgOutput
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long OrgId { get; set; }

        public string OrgName { get; set; }
    }

    public class UserText {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    /// <summary>
    /// 组织架构控件 用于显示已选择的人员
    /// </summary>
    public class UserOrgShow {
        public string Id { get; set; }
        public string Code { get; set; }
        public string OrgId { get; set; }
        public string Title { get; set; }
    }
    public class UserOrgShowInput
    {
        public int selectTypes { get; set; }

        public string ids { get; set; }
    }
    public class UserUnderOrgProssceStaticInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long? OrgId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name";
            }
        }
    }
}
