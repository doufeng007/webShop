using Abp.Application.Services.Dto;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    public class DocmentDto: FullAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 档案编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 档案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public Guid Type { get; set; }
        /// <summary>
        /// 是否需归还
        /// </summary>
        public bool NeedBack { get; set; }
        public Guid? FlowId { get; set; }
        public string Type_Name { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 档案属性
        /// </summary>
        public DocmentAttr Attr { get; set; }

        public string Attr_Name { get; set; }
        /// <summary>
        /// 档案备注
        /// </summary>
        public string Des { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public long? UserId { get; set; }

        public string UserId_Name { get; set; }

        /// <summary>
        /// 是否项目归档
        /// </summary>
        public bool IsProject { get; set; }
        /// <summary>
        /// 归档id
        /// </summary>
        public Guid? ArchiveId { get; set; }

        /// <summary>
        /// 档案二维码
        /// </summary>
        public Guid? QrCodeId { get; set; }

        /// <summary>
        /// 归档申请备注
        /// </summary>
        public string ApplyDes { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
