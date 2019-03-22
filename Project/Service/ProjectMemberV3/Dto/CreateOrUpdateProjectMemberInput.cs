using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Service.ProjectMemberV3.Dto
{
    public class CreateOrUpdateProjectMemberInput
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

        public List<CreateOrUpdateAuditMemberInput> Members { get; set; }

        public List<CreateOrUpdateFinishInput> Finishs { get; set; }


        public CreateOrUpdateProjectMemberInput()
        {
            this.Members = new List<CreateOrUpdateAuditMemberInput>();
            this.Finishs = new List<CreateOrUpdateFinishInput>();
        }
    }

    public class CreateOrUpdateAuditMemberInput
    {
        public Guid? Id { get; set; }

        public int AuditRoleId { get; set; }

        public long UserId { get; set; }
    }

    public class CreateOrUpdateFinishInput
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public int WorkDay { get; set; }
        /// <summary>
        /// 完成度百分比
        /// </summary>
        public decimal Persent { get; set; }
        public List<CreateOrUpdateFinishAllotInput> FinishMembers { get; set; }

        public CreateOrUpdateFinishInput()
        {
            this.FinishMembers = new List<CreateOrUpdateFinishAllotInput>();
        }
    }

    public class CreateOrUpdateFinishAllotInput
    {
        public long UserId { get; set; }

        public bool IsMain { get; set; }
    }
}
