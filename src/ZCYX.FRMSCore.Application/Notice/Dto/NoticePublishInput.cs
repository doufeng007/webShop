using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZCYX.FRMSCore.Application

{
    public class NoticePublishInput
    {

        public Guid? Id { get; set; }

        public string Title { get; set; }

        //public DateTime ExpireTime { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 要通知的用户逗号分隔
        /// </summary>
        public string NoticeUserIds { get; set; }

        /// <summary>
        /// 要通知的组
        /// </summary>
        public string NoticeGroupIds { get; set; }
        /// <summary>
        /// 要通知的部门
        /// </summary>
        public string NoticeDepartmentIds { get; set; }

        /// <summary>
        /// 1 事务通知 2 通知公告 3 公司新闻
        /// </summary>
        public int NoticeType { get; set; }

        /// <summary>
        /// 是否所有人接受  针对通知公告
        /// </summary>
        public bool IsAllRecive { get; set; }
        public long? SendUserId { get; set; }
    }


    public class NoticePublishInputForWorkSpaceInput : NoticePublishInput
    {
        public Guid ProjectId { get; set; }

    }



}