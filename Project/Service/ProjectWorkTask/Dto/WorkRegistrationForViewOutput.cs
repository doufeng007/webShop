using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;

namespace Project
{
    [AutoMapFrom(typeof(ProjectRegistration))]
    public class WorkRegistrationForViewOutput
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        public DateTime WorkTime { get; set; }
        public List<GetAbpFilesOutput> Files { get; set; }
    }



    [AutoMapFrom(typeof(ProjectRegistration))]
    public class WorkRegistrationOutput
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string ProjectName { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        public DateTime WorkTime { get; set; }

        public string ReplyContent { get; set; }
        public List<WorkRegistrationModelOutput> List { get; set; }
        public List<GetAbpFilesOutput> Files { get; set; }
    }

    public class WorkRegistrationModelOutput {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
    public class PmInitWorkFlowOutput
    {
        public bool IsPm { get; set; }
        public InitWorkFlowOutput initWorkFlow { get; set; }

    }

    [AutoMapFrom(typeof(ProjectRegistration))]
    public class WorkRegistrationForViewPublishOutput: WorkRegistrationForViewOutput
    {
        /// <summary>
        /// 是否发送
        /// </summary>
        public bool HasPublish { get; set; }
    }
    [AutoMapFrom(typeof(ProjectRegistration))]
    public class WorkRegistrationForViewRelyOutput : WorkRegistrationForViewOutput
    {
        /// <summary>
        /// 是否回复
        /// </summary>
        public bool HasRely { get; set; }
    }


    [AutoMapFrom(typeof(ProjectRegistration))]
    public class WorkRegistrationForFlowOutput: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        public DateTime WorkTime { get; set; }
        public List<GetAbpFilesOutput> Files { get; set; }
    }
}