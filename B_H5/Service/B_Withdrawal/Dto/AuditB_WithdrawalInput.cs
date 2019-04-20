using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5
{
    public class AuditB_WithdrawalInput
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

    public class RemitInput
    {
        public Guid Id { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public bool IsSucce { get; set; }


    }
}
