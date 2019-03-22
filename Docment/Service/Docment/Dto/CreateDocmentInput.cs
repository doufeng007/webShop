using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docment
{
    /// <summary>
    /// 创建档案
    /// </summary>
    [AutoMap(typeof(DocmentList))]
    public class CreateDocmentInput: CreateWorkFlowInstance
    {


        /// <summary>
        /// 档案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public Guid Type { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 档案属性
        /// </summary>
        public DocmentAttr Attr { get; set; }
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

        /// <summary>
        /// 是否项目归档
        /// </summary>
        public bool IsProject { get; set; }
        /// <summary>
        /// 是否需归还
        /// </summary>
        public bool NeedBack { get; set; }
        /// <summary>
        /// 归档项目id
        /// </summary>
        public Guid? ArchiveId { get; set; }
        public Guid? QrCodeId{ get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public DocmentStatus? Status { get; set; }
    }
    [AutoMap(typeof(DocmentList))]
    public class UpdateDocmentInput : CreateWorkFlowInstance
    { 
        public Guid Id { get; set; }
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
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 档案属性
        /// </summary>
        public DocmentAttr Attr { get; set; }
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

        public bool IsUpdateForChange { get; set; }
    }
    /// <summary>
    /// 其他流程档案归档
    /// </summary>
    public class DocmentInput//: CreateWorkFlowInstance
    {
        public Guid TaskId { get; set; }
        /// <summary>
        /// 档案二维码
        /// </summary>
        public Guid? QrCodeId { get; set; }
        /// <summary>
        /// 档案类别
        /// </summary>
        public DocmentAttr Attr { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
    }
    /// <summary>
    /// 申请归档
    /// </summary>

    public class ApplyDocmentInput {
        public string ApplyDes { get; set; }
        public Guid? ReasonTaskId { get; set; }
        public List<Guid> DocmentIds { get; set; }
    }
}
