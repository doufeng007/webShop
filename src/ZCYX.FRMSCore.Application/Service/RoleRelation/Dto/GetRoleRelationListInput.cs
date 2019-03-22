using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace ZCYX.FRMSCore.Application
{
    public class GetRoleRelationListInput : PagedAndSortedInputDto, IShouldNormalize
    {

        /// <summary>
        /// 关联id
        /// </summary>
        public Guid RelationId { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        public RelationType Type { get; set; }

        /// <summary>
        /// 关联用户
        /// </summary>
        public long RelationUserId { get; set; }

        /// <summary>
        /// 当前用户
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 转让角色
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
