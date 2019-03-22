using Abp.Runtime.Validation;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Supply
{
    public class SupplyPurchaseQDFromApplyListOutput 
    {
        /// <summary>
        /// 申领id
        /// </summary>
        public Guid SupplyAppyId { get; set; }


        /// <summary>
        /// 申领人名称
        /// </summary>
        public string SupplyUserName { get; set; }



        /// <summary>
        /// 申领人岗位名称
        /// </summary>
        public string PostName { get; set; }


        /// <summary>
        /// 申领人部门名称
        /// </summary>
        public string OrgName { get; set; }


        /// <summary>
        /// 申领时间
        /// </summary>
        public DateTime ApplyDateTime { get; set; }


        /// <summary>
        /// 是否紧急
        /// </summary>
        public bool IsImportant { get; set; }


        public string StatusTile { get; set; }


        public int Status { get; set; }
       

    }


  


}
