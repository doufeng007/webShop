using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 是否入职
    /// </summary>
    public class EmployeeJoinInput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 是否入职
        /// </summary>
        public bool IsJoin { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string JoinDes { get; set; }
    }
}
