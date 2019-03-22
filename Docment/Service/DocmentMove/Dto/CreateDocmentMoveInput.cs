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
    /// 创建档案移交
    /// </summary>
    [AutoMap(typeof(DocmentMove))]
    public class CreateDocmentMoveInput : CreateWorkFlowInstance
    {
        /// <summary>
        /// 档案编号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        public Guid DocmentId { get; set; }

        public string Des { get; set; }
    }
    [AutoMap(typeof(DocmentMove))]
    public class UpdateeDocmentMoveInput: CreateWorkFlowInstance
    {
        public Guid Id { get; set; }
        public Guid DocmentId { get; set; }
        [LogColumn("备注", true)]
        public string Des { get; set; }
        public bool IsUpdateForChange { get; set; }

        /// <summary>
        /// 销毁证明附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
    }

}
