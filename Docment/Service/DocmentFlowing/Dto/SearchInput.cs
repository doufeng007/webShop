using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment
{
    /// <summary>
    /// 查看统计列表
    /// </summary>
    public class SearchInput: PagedAndSortedInputDto
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }

        public ListType ListType { get; set; }

        public long Id { get; set; }
    }
    public enum Type {
        部门=0,
        用户=1
    }

    public enum ListType {
        资料总数=1,
        需归还档案=2,
        在外流转=3,
        发起归档=4
    }
}
