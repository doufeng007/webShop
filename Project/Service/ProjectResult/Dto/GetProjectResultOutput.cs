using Abp.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{

    public class GetProjectAuditResultBaseOutput
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public GetProjectBudgetForEditOutput ProjectInfo { get; set; }

        public Guid ProjectBaseId { get; set; }


        #region 当前登录人员的评审结果

        /// <summary>
        /// 对应ProjectAuditRole
        /// </summary>
        public int AuditRoleId { get; set; }
        public ProjectAuditResultInfoOutput Result { get; set; }

        public GetProjectAuditResultBaseOutput()
        {
            this.ProjectInfo = new GetProjectBudgetForEditOutput();
            this.Result = new ProjectAuditResultInfoOutput();
        }


        #endregion
    }

    /// <summary>
    /// 工程评审结果
    /// </summary>
    public class GetProjectAuditResultOutput : GetProjectAuditResultBaseOutput
    {


        #region  分派给 当前登录的工程评审人员  的事务


        public List<ProjectFinishItem> FinishItems { get; set; }



        #endregion



        public GetProjectAuditResultOutput()
        {
            this.FinishItems = new List<ProjectFinishItem>();

        }

    }


    /// <summary>
    /// 分派给工程评审人员的事务
    /// </summary>
    public class ProjectFinishItem
    {
        public Guid FinishId { get; set; }

        public string FinishName { get; set; }

        public int WorkDay { get; set; }

        public Guid AllotId { get; set; }

        public ProjectAuditResultInfoOutput Result { get; set; }
    }

    public class GetProjectHuiZongResultOutput
    {

        public List<CreateOrUpdateFinishOutput> Finishs { get; set; }

        public GetProjectBudgetForEditOutput ProjectInfo { get; set; }

        public Guid ProjectBaseId { get; set; }
    }


    /// <summary>
    /// 项目负责人评审结果
    /// </summary>
    public class GetProjectLeaderResultOutput : GetProjectAuditResultBaseOutput
    {

        public List<CreateOrUpdateFinishOutput> Finishs { get; set; }

        public GetProjectLeaderResultOutput()
        { this.Finishs = new List<CreateOrUpdateFinishOutput>(); }

    }


    /// <summary>
    /// 财务终审评审结果
    /// </summary>
    public class GetProjectCWEResultOutput : GetProjectAuditResultBaseOutput
    {
        /// <summary>
        /// 财务初审结果
        /// </summary>
        public ProjectAuditResultInfoOutput CWFResult { get; set; }

        public GetProjectCWEResultOutput()
        { this.CWFResult = new ProjectAuditResultInfoOutput(); }

    }

    /// <summary>
    /// 获取联系人汇总初审结果界面
    /// </summary>
    public class GetProjectFirstAuditCollectResultOutput : GetProjectAuditResultBaseOutput
    {

        public List<CreateOrUpdateFinishOutput> Finishs { get; set; }
        public GetProjectFirstAuditCollectResultOutput()
        {
            this.Finishs = new List<CreateOrUpdateFinishOutput>();
        }

    }

    /// <summary>
    /// 总工复核结果
    /// </summary>
    public class GetProjectReviewResultOutput : GetProjectAuditResultBaseOutput
    {

        public ProjectAuditResultInfoOutput LeaderResult { get; set; }


        public List<CreateOrUpdateFinishOutput> Finishs { get; set; }

        public ProjectAuditResultInfoOutput ReviewResult { get; set; }

        public ProjectAuditResultInfoOutput ReviewResult2 { get; set; }


        public ProjectAuditResultInfoOutput ReviewResult3 { get; set; }
        /// <summary>
        /// 财务初审
        /// </summary>
        public ProjectAuditResultInfoOutput FinanceResult1 { get; set; }
        /// <summary>
        /// 财务终审
        /// </summary>
        public ProjectAuditResultInfoOutput FinanceResult2 { get; set; }

        public GetProjectReviewResultOutput()
        {
            this.LeaderResult = new ProjectAuditResultInfoOutput();
            this.Finishs = new List<CreateOrUpdateFinishOutput>();
            this.ReviewResult = new ProjectAuditResultInfoOutput();
            this.ReviewResult2 = new ProjectAuditResultInfoOutput();
            this.ReviewResult3 = new ProjectAuditResultInfoOutput();
            this.FinanceResult1 = new ProjectAuditResultInfoOutput();
            this.FinanceResult2 = new ProjectAuditResultInfoOutput();
        }

    }

    public class ProjectAuditTotalResultOutput
    {
        public ProjectAuditResultInfoOutput LeaderResult { get; set; }

        public ProjectAuditResultInfoOutput ReviewResult { get; set; }

        public ProjectAuditResultInfoOutput ReviewResult2 { get; set; }


        public ProjectAuditResultInfoOutput ReviewResult3 { get; set; }

        /// <summary>
        /// 财务初审
        /// </summary>
        public ProjectAuditResultInfoOutput FinanceResult1 { get; set; }
        /// <summary>
        /// 财务终审
        /// </summary>
        public ProjectAuditResultInfoOutput FinanceResult2 { get; set; }
        public List<CreateOrUpdateFinishOutput> Finishs { get; set; }

        public ProjectAuditTotalResultOutput()
        {
            this.LeaderResult = new ProjectAuditResultInfoOutput();
            this.ReviewResult = new ProjectAuditResultInfoOutput();
            this.ReviewResult2 = new ProjectAuditResultInfoOutput();
            this.ReviewResult3 = new ProjectAuditResultInfoOutput();
            this.Finishs = new List<CreateOrUpdateFinishOutput>();
        }


    }




    public class ProjectAuditResultInfoOutput
    {
        public List<GetAbpFilesOutput> Files { get; set; }

        public GetAbpFilesOutput CjzFile { get; set; }

        public string Remark { get; set; }

        public decimal? AuditAmount { get; set; }

        public string UserName { get; set; }

        public long UserId { get; set; }

        public int AuditRoleId { get; set; }
        /// <summary>
        /// 工程师自己确定的完成比例
        /// </summary>
        public decimal? SurePersent { get; set; }

        /// <summary>
        /// 项目经理确认的完成比例
        /// </summary>
        public decimal? ManagerSurePersent { get; set; }

        public string AuditRoleName { get; set; }

        public Guid? Id { get; set; }
    }


}
