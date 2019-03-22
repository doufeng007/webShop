using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace HR
{
    public class NoticeMessageInput
    {
        public Guid? RoadFlowReceiveId { get; set; }

        public long? ReceiveId { get; set; }

        public Guid? ProjectId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Link { get; set; }

        /// <summary>
        /// tue表示只是提示 不入库
        /// </summary>
        public bool? OnlyTip { get; set; }
    }
}