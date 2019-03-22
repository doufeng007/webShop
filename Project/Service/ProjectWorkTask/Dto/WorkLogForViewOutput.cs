using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;

namespace Project
{
    [AutoMapFrom(typeof(ProjectWorkLog))]
    public class WorkLogForViewOutput
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }



    public class WriteLogViewModelOut
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectCode { get; set; }

        public Guid StepId { get; set; }

        public string StepName { get; set; }

        public string Title { get; set; }


        public string Content { get; set; }

        public int? LogType { get; set; }

        public List<GetAbpFilesOutput> Files { get; set; }
    }
}