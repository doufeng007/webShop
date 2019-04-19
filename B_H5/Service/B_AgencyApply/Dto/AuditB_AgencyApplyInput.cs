using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace B_H5
{
    public class AuditB_AgencyApplyInput
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 审核不通过原因
        /// </summary>
        public string Reason { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 是否通过
        /// </summary>
        public bool IsPass { get; set; }
    }



    public class AuditB_PrePayInput : AuditB_AgencyApplyInput
    { }
}