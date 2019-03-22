using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Docment
{
    public class DocmentSearchInput : WorkFlowPagedAndSortedInputDto
    {
        /// <summary>
        /// 档案属性（电子=0,纸质=1）
        /// </summary>
        public DocmentAttr? Attr { get; set; }
        /// <summary>
        /// 获取外部或内部流转档案
        /// </summary>
        public bool? GetOutorIn { get; set; }
        /// <summary>
        /// 获取可以归档的档案（排除掉收文中的档案，二维码类型为收文的档案）
        /// </summary>
        public bool GetCanDocmentIn { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public Guid? Type { get; set; }
        /// <summary>
        /// 档案状态申请入档=0, 待收 = 1,在档 = -1,已驳回=-2,使用中 = -3, 已移交 = -4,已销毁 = -5,移交中=-6,销毁中=-7,审批中=-8,档案袋=-10
        /// </summary>
        public List< DocmentStatus> Status { get; set; }

    }
    
}
