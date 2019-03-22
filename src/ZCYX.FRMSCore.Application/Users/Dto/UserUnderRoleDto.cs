using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace ZCYX.FRMSCore.Users.Dto
{

    public class GetAbpUsersByRoleIdInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int RoleId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class UserUnderRoleDto
    {
        public long UserId { get; set; }

        public string UserName { get; set; }


        public string OrgName { get; set; }
    }


    public class UpdateUsersUnderRoleInput
    {
        public int RoleId { get; set; }


        public List<long> UserIds { get; set; } = new List<long>();
    }

    public class UpdateUsersUnderRoleOneWayInput : UpdateUsersUnderRoleInput
    {
        /// <summary>
        /// 1 新增 2删除
        /// </summary>
        public int ActionType { get; set; }
    }


    public class IMUserInfoDto
    {
        public string UserName { get; set; }


        public string NickName { get; set; }

    }


}
