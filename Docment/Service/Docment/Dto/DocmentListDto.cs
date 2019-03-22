using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace Docment
{
    [AutoMap(typeof(DocmentList))]
    public class DocmentListDto : BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 是否需归还
        /// </summary>
        public bool NeedBack { get; set; }
        /// <summary>
        /// 二维码类型
        /// </summary>
        public QrCodeType QrCodeType { get; set; }
        /// <summary>
        /// 档案二维码
        /// </summary>
        public Guid? QrCodeId { get; set; }
        /// <summary>
        /// 档案所属流程
        /// </summary>
        public Guid? FlowId { get; set; }
        /// <summary>
        /// 档案编号
        /// </summary>

        public string No { get; set; }
        /// <summary>
        /// 档案名称
        /// </summary>
        [LogColumn("档案名称", true)]
        public string Name { get; set; }
        /// <summary>
        /// 档案类别 
        /// </summary>
        [LogColumn("档案类别", true)]
        public Guid Type { get; set; }

        public string Type_Name { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        [LogColumn("存放位置", true)]
        public string Location { get; set; }
        /// <summary>
        /// 档案属性
        /// </summary>
        [LogColumn("档案属性", true)]
        public DocmentAttr Attr { get; set; }

        [LogColumn("档案属性", true)]
        public string Attr_Name { get; set; }

        /// <summary>
        /// 是否老档案（导入的档案都是老档案）
        /// </summary>
        public bool IsOld { get; set; }
        /// <summary>
        /// 是否已借到外部
        /// </summary>
        [LogColumn("外部档案", true)]
        public bool IsOut { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        [LogColumn("责任人", true)]
        public long? UserId { get; set; }
        [LogColumn("责任人", true)]
        public string UserId_Name { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 档案备注
        /// </summary>
        [LogColumn("档案备注", true)]
        public string Des { get; set; }
        /// <summary>
        /// 是否项目归档
        /// </summary>
        public bool IsProject { get; set; }
        /// <summary>
        /// 归档项目id
        /// </summary>
        public Guid? ArchiveId { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
    }
}
