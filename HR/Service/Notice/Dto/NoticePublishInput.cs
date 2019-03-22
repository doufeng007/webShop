using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR

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
    }


    public class NoticePublishInputForWorkSpaceInput : NoticePublishInput
    {
        public Guid ProjectId { get; set; }
        /// <summary>
        /// user类型,0项目录入人员，1步骤之前处理人，2传递人员，3当前步骤及之前步骤处理人
        /// </summary>
        public int UserType { get; set; }
    }



}