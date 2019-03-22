using Abp.AutoMapper;
using Abp.File;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    [AutoMapFrom(typeof(ProjectWorkLog))]
    public class WorkLogList
    {
        public Guid Id { get; set; }

        public Guid TaskId { get; set; }

        public long UserId { get; set; }

        public Guid? ProjectId { get; set; }

        public string Title { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }
        public int? LogType { get; set; }

        public List<GetAbpFilesOutput> Files { get; set; }
        public string FilesJson { get; set; }
        public Guid? StepId { get;set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }
        public string Content { get; set; }

    }

    public class GetWorkLogListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        private Guid _projectId;

        public Guid ProjectId
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

        public bool IncludeDetele { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}