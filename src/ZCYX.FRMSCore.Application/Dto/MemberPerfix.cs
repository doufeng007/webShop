using Abp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public class MemberPerfix
    {
        /// <summary>
        /// 人员
        /// </summary>
        public const string UserPREFIX = "u_";

        /// <summary>
        /// 工作组
        /// </summary>
        public const string WorkGroupPREFIX = "w_";

        /// <summary>
        /// 部门领导
        /// </summary>
        public const string DepartmentPREFIX = "l_";


        /// <summary>
        /// 部门分管领导
        /// </summary>
        public const string DepartmentFGLeaderPREFIX = "f_";

        /// <summary>
        /// 部门直属成员
        /// </summary>
        public const string DepartmentMemberPREFIX = "d_";


        /// <summary>
        /// 选择部门
        /// </summary>
        public const string DepartmentIdPREFIX = "b_";

        /// <summary>
        /// 选择用户 DepartmentIdPREFIX  UserIdPREFIX 是部门和用户同时选择的时候用
        /// </summary>
        public const string UserIdPREFIX = "y_";

        public const string AllUserPREFIX = "a_";

        /// <summary>
        /// 通过岗位id 查找users
        /// </summary>
        public const string PostPREFIX = "p_";


        public static string RemovePrefix(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return "";
            }
            if (id.StartsWith(UserPREFIX))
            {
                return id.Replace(UserPREFIX, "");
            }
            else if (id.StartsWith(WorkGroupPREFIX))
            {
                return id.Replace(WorkGroupPREFIX, "");
            }
            else if (id.StartsWith(DepartmentPREFIX))
            {
                return id.Replace(DepartmentPREFIX, "");
            }
            else if (id.StartsWith(DepartmentMemberPREFIX))
            {
                return id.Replace(DepartmentMemberPREFIX, "");
            }
            else if (id.StartsWith(DepartmentIdPREFIX))
            {
                return id.Replace(DepartmentIdPREFIX, "");
            }
            else if (id.StartsWith(UserIdPREFIX))
            {
                return id.Replace(UserIdPREFIX, "");
            }
            else if (id.StartsWith(AllUserPREFIX))
            {
                return id.Replace(AllUserPREFIX, "");
            }
            else if (id.StartsWith(PostPREFIX))
            {
                return id.Replace(PostPREFIX, "");
            }
            else if (id.StartsWith(DepartmentFGLeaderPREFIX))
            {
                return id.Replace(DepartmentFGLeaderPREFIX, "");
            }
            return id;
        }


        /// <summary>
        /// 是否是人员的多人字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUserListString(string id)
        {
            if (id.IsNullOrEmpty())
                return false;
            var id_Arrys = id.Split(",");
            foreach (var item in id_Arrys)
            {
                if (!item.StartsWith(UserPREFIX))
                    return false;
            }
            return true;
        }
    }
}
