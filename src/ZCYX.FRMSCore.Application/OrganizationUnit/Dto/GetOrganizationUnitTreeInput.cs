using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public class GetOrganizationUnitTreeInput
    {
        public long ParentId { get; set; }

        /// <summary>
        ///  0 人员  1 部门 2 用户  3 部门+用户
        /// </summary>
        public int SelectType { get; set; }


        public int TreeType { get; set; } = 1;

    }

    public class GetOrganizationUnitTreeNewInput
    {
        public string ParentId { get; set; }

        /// <summary>
        ///  0 人员  1 部门 2 用户  3 部门+用户
        /// </summary>
        public int SelectType { get; set; }


        public int TreeType { get; set; } = 1;

        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Permissions { get; set; } = new List<string>();

    }

    [Flags]
    public enum GetOrganizationUnitTreeType
    {
        选择人员 = 1,
        选择部门领导人 = 2,
        选择部门直属成员 = 4,
        选择部门所有成员 = 8,
    }
}
