using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment
{
    public class DocmentBorrowSearchInput: WorkFlowPagedAndSortedInputDto
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public long? OrgId { get; set; }
        public DocmentAttr? Attr { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public Guid? Type { get; set; }
    }

    public enum BorrowType {
        我的借阅=0,
        领导审核=1,
        档案员出借=2,
        待归还档案=3
    }
}
