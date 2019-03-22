using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 新增或编辑工程评审人员
    /// </summary>
    public class CreateProjectAuditMemberV2Input
    {
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 是否需要财务评审
        /// </summary>
        public bool HasFinancialReview { get; set; }


        /// <summary>
        /// 财务初审部门编号
        /// </summary>
        public long OrgFinancial1 { get; set; } = 0;


        /// <summary>
        /// 财务终审部门编号
        /// </summary>
        public long OrgFinancial2 { get; set; } = 0;

        public List<ProjectAuditMembersV2> Members { get; set; }

        public CreateProjectAuditMemberV2Input()
        {
            this.Members = new List<ProjectAuditMembersV2>();
        }

    }

    public class ProjectAuditMembersV2
    {
        public Guid? Id { get; set; }

        public long UserId { get; set; } = 0;

        public int AuditRoleId { get; set; }


        public string FlowId { get; set; }

        /// <summary>
        /// 分派工作描述
        /// </summary>
        public string WorkDes { get; set; }

       /// <summary>
       /// 分派工日
       /// </summary>
        public int WorkDays { get; set; }
        /// <summary>
        /// 绩效占比 数值*100
        /// </summary>
        public int? Percentes { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Des { get; set; }
    }



    public class CreateOrUpdateAuditGroupAndFinancialInput
    {
        public Guid ProjectBaseId { get; set; }

        /// <summary>
        /// 评审组编号
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 财务初审部门编号
        /// </summary>
        public long OrgFinancial1 { get; set; } = 0;


        /// <summary>
        /// 财务终审部门编号
        /// </summary>
        public long OrgFinancial2 { get; set; } = 0;


        /// <summary>
        /// 是否进行财务评审
        /// </summary>
        public bool HasFinancialReview { get; set; }
    }


    public class AuditGroupAndFinancialOutPut : CreateOrUpdateAuditGroupAndFinancialInput
    {

    }



}
