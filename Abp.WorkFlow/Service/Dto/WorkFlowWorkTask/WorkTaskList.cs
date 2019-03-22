using Abp.AutoMapper;
using Abp.File;
using Abp.Runtime.Validation;
using System;
using ZCYX.FRMSCore.Application.Dto;

namespace Abp.WorkFlow
{
    public class WorkTaskList
    {
        public Guid Id { get; set; }

        public int TaskType { get; set; }

        public long UserId { get; set; }

        public Guid? ProjectId { get; set; }

        public string Title { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? StepId { get; set; }

        public string StepName { get; set; }

        public Guid? InstanceId { get; set; }

        /// <summary>
        /// 用户账号  发送者姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 处理者姓名
        /// </summary>
        public string ReceiveName { get; set; }



        public string CompletedTime { get; set; }


        public string StatusTitle { get; set; }


        public string Note { get; set; }

        public string TaskTypeName
        {
            get
            {
                var arr = new string[] { "引擎流程", "工作底稿", "数据修改", "工作联系", "意见征询", "发文记录", "退回审核", "", "" };

                return arr[TaskType];
            }
        }

        public string Comment { get; set; }


        public GetAbpFilesOutput CommentFile { get; set; } = new GetAbpFilesOutput();
    }

    public class GetWorkTaskListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        private string _projectId;

        public string ProjectId
        {
            get
            {
                return _projectId;
            }
            set
            {
                _projectId = value;
            }
        }

        //public bool IncludeDetele { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime desc";
            }
        }
    }
}