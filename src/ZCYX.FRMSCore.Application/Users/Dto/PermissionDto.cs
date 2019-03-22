using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Users.Dto
{
    public class SpecialPermissionDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 是否授权(true:增加的权限 false:移除的权限)
        /// </summary>
        public bool IsGranted { get; set; }
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string DisplayName { get; set; }

        public long? UserId { get; set; }
    }
}
