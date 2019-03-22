using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore;

namespace Docment
{
    /// <summary>
    /// 创建档案借阅
    /// </summary>
    [AutoMap(typeof(DocmentBorrow))]
    public class CreateDocmentBorrowInput : CreateWorkFlowInstance
    {
        //public Guid DocmentId { get; set; }
        /// <summary>
        /// 多个档案
        /// </summary>
        public List<Guid> DocmentIds { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? BackTime { get; set; }
        public string Des { get; set; }

        /// <summary>
        /// 外部借阅人
        /// </summary>
        public string OutUser { get; set; }
        /// <summary>
        /// 外部借阅人电话
        /// </summary>
        public string OutPhone { get; set; }
        /// <summary>
        /// 外部借阅人单位
        /// </summary>
        public string OutCompany { get; set; }
        /// <summary>
        /// 是否外部借阅申请
        /// </summary>
        public bool? IsOut { get; set; }
    }
    /// <summary>
    /// 更新档案借阅信息
    /// </summary>
    [AutoMap(typeof(DocmentBorrow))]
    public class UpdateeDocmentBorrowInput: CreateWorkFlowInstance
    {
        public Guid Id { get; set; }
        public Guid DocmentId { get; set; }
        [LogColumn("档案", true)]
        public string DocmentName { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        [LogColumn("备注", true)]
        public string Des { get; set; }
        [LogColumn("预计归还时间", true)]
        public DateTime? BackTime { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
}
