using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class GetProjectAuditStopForEidtOutput
    {

        public Guid? Id { get; set; }
        public Guid ProjectBaseId { get; set; }


        public string ProjectName { get;set;}

        public string SingleProjectName { get; set; }

        public string ProjectCode { get; set; }
        

        public string SingleProjectCode { get; set; }

        public decimal? SendTotalBudget { get; set; }

        public string Remark { get; set; }

        public string RelieveRemark { get; set; }


        public int DelayDay { get; set; }
        public string Status_Name { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNew { get; set; }
    }
}
