using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OATenderEnemy))]
    public class OATenderEnemyInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public Guid? ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectType { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 对手情况 json-OATenderEnemyItem
        /// </summary>
        public List<OATenderEnemyItem> EnemyList { get; set; } = new List<OATenderEnemyItem>();
        public int Status { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
    }

    [AutoMap(typeof(OATenderEnemy))]
    public class OATenderEnemyDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectType { get; set; }

        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 对手情况 json-OATenderEnemyItem
        /// </summary>
        public List<OATenderEnemyItem> EnemyList { get; set; } = new List<OATenderEnemyItem>();
        public int Status { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
    }
    [AutoMap(typeof(OATenderEnemy))]
    public class OATenderEnemyListDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectType { get; set; }

        public string AuditUser { get; set; }
        public string AuditUserText { get; set; }
    }
    public class OATenderEnemyItem
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; }

        public decimal? Price { get; set; }

        public decimal? ValiPrice { get; set; }
        /// <summary>
        /// 偏差率
        /// </summary>
        public decimal? Persent { get; set; }

        public int Score { get; set; }

        public string Des { get; set; }
    }
}
