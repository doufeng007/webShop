using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{

    [AutoMap(typeof(ProjectBudgetControl))]
    public class ProjectBudgetControlOutput
    {
        #region 表字段
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Pro_Id { get; set; }


        public Guid SingleProjectId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ApprovalMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SendMoney { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ValidationMoney { get; set; }

        #endregion
    }
}
