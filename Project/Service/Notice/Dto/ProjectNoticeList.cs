using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class NoticeView
    {

        public Guid? TextId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }
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

    }

    public class NoticeList: FollowOutputDto
    {
        public Guid LogId { get; set; }

        public Guid TextId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public DateTime CreationTime { get; set; }

        public int Type { get; set; }


        public string CreateUserName { get; set; }
        public long CreateUserId { get; set; }

    }

    public class GetNoticeListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        private int _noticeType = 1;

        /// <summary>
        /// 1事务通知  2通知公告 3公司新闻
        /// </summary>
        public int NoticeType
        {
            get
            {
                return _noticeType;
            }
            set
            {
                _noticeType = value;
            }
        }


        public bool IncludeDetele { get; set; }
        public bool IsNew { get; set; } = true;
        public bool IsFollow { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }



    public class NoticeNoRead
    {
        public Guid Id { get; set; }

        public int NoticeType { get; set; }
    }

}