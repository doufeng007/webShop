using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(EmployeeRequire))]
    public class EmployeeRequireDetailDto : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public int? TenantId { get; set; }
        public string CompanyName { get; set; }

        public long Department { get; set; }

        public Guid? Job { get; set; }

        public int? SureNum { get; set; }

        public int? NowNum { get; set; }

        public int? RequireNum { get; set; }

        public string RequireReason { get; set; }

        public DateTime ApplyTime { get; set; }

        public DateTime? JoinTime { get; set; }

        public int Sex { get; set; }

        public string Marray { get; set; }

        public int? MaxAge { get; set; }

        public string Education { get; set; }

        public string Major { get; set; }

        public string English { get; set; }

        public string Experience { get; set; }

        public string Computer { get; set; }

        public string WorkDes { get; set; }

        public string RequireDes { get; set; }

        public string OtherDes { get; set; }

        public string FeatureDes { get; set; }

        public string Salary { get; set; }

        public string Partner { get; set; }
    }
}
