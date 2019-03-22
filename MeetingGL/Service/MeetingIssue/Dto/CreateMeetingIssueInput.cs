using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace MeetingGL
{
    [AutoMapTo(typeof(MeetingIssue))]
    public class CreateMeetingIssueInput 
    {
        #region 表字段
        /// <summary>
        /// 事项
        /// </summary>
        [MaxLength(500,ErrorMessage = "事项长度必须小于500")]
        [Required(ErrorMessage = "必须填写事项")]
        public string Name { get; set; }

        /// <summary>
        /// 汇报部门
        /// </summary>
        public long? OrgId { get; set; }

        /// <summary>
        /// 汇报人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        public Guid? RelationMeetingId { get; set; }


        public IssueType IssueType { get; set; }


        public Guid? SingleProjectId { get; set; }
        public Guid? RelationProposalId { get; set; }


        #endregion
    }
}