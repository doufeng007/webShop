using Abp.AutoMapper;
using Project;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace GWGL
{
    [AutoMapFrom(typeof(NoticeDocument))]
    public class NoticeDocumentChangeDto
    {
        [LogColumn("标题", IsLog = true)]
        public string Title { get; set; }


        /// <summary>
        /// 发文文号
        /// </summary>

        [LogColumn("发文文号", IsLog = true)]
        public string DispatchCode { get; set; }


        [LogColumn("抄送", IsLog = true)]
        public string ReceiveName { get; set; }

        [LogColumn("内容", IsLog = true)]
        public string Content { get; set; }


        /// <summary>
        /// 发文人名称
        /// </summary>
        [LogColumn("发文人", IsLog = true)]
        public string PubilishUserName { get; set; }


        /// <summary>
        /// 主送人
        /// </summary>
        [LogColumn("主送人", IsLog = true)]
        public string MainReceiveName { get; set; }


        /// <summary>
        /// 发文机关
        /// </summary>
        [LogColumn("发文机关", IsLog = true)]
        public string DispatchUnitName { get; set; }


        [LogColumn("公文类别", IsLog = true)]
        public string DocumentTyepName { get; set; }



        [LogColumn("是否会签", IsLog = true)]
        public bool IsNeedAddWrite { get; set; }


        [LogColumn("会签部门", IsLog = true)]
        public string AddWriteOrgIdName { get; set; }



        [LogColumn("文种", IsLog = true)]
        public string GW_DocumentTypeName { get; set; }

    }
}
