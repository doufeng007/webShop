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
    /// 创建档案销毁
    /// </summary>
    [AutoMap(typeof(DocmentDestroy))]
    public class CreateDocmentDestroyInput: CreateWorkFlowInstance
    {
        public Guid DocmentId { get; set; }

        public string Des { get; set; }
    }
    /// <summary>
    /// 更新档案销毁信息
    /// </summary>
    [AutoMap(typeof(DocmentDestroy))]
    public class UpdateeDocmentDestroyInput:CreateWorkFlowInstance
    {
        public Guid Id { get; set; }
        public Guid DocmentId { get; set; }
        [LogColumn("备注", true)]
        public string Des { get; set; }
        public bool IsUpdateForChange { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; }
    }
}
