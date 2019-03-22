using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{

    public class GetProjectMemberForEditOutput
    {

        public Guid ProjectId { get; set; }


        public Guid SingleProjectId { get; set; }

        public GetProjectBudgetForEditOutput ProjectInfo { get; set; }

        public List<CreateOrUpdateAuditMemberOutput> Members { get; set; }

        public List<CreateOrUpdateFinishOutput> Finishs { get; set; }

        public GetProjectMemberForEditOutput()
        {
            this.ProjectInfo = new GetProjectBudgetForEditOutput();
            this.Finishs = new List<CreateOrUpdateFinishOutput>();
            this.Members = new List<CreateOrUpdateAuditMemberOutput>();
        }
    }
    public class CreateOrUpdateAuditMemberOutput
    {
        public Guid Id { get; set; }

        public int AuditRoleId { get; set; }

        public string AuditRoleName { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public Guid? GroupId { get; set; }

        public string GroupName { get; set; }


        public string WorkDes { get; set; }

        public int? WorkDays { get; set; }

        /// <summary>
        /// 绩效占比 数值*100
        /// </summary>
        public int? Percentes { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Des { get; set; }
        public string FlowId { get; set; }
    }

    public class CreateOrUpdateFinishOutput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int WorkDay { get; set; }
        /// <summary>
        /// 完成度百分比
        /// </summary>
        public decimal Persent { get; set; }
        public Guid MainAllotId { get; set; }

        public List<CreateOrUpdateFinishAllotOutput> FinishMembers { get; set; }


        public ProjectAuditResultInfoOutput GatherResult { get; set; }

        public CreateOrUpdateFinishOutput()
        {
            this.FinishMembers = new List<CreateOrUpdateFinishAllotOutput>();
            this.GatherResult = new ProjectAuditResultInfoOutput();
        }
    }

    public class CreateOrUpdateFinishAllotOutput
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public bool IsMain { get; set; }

        public Guid Id { get; set; }


        public ProjectAuditResultInfoOutput Result { get; set; }



        public CreateOrUpdateFinishAllotOutput()
        {
            this.Result = new ProjectAuditResultInfoOutput();

        }
    }



}
