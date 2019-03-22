using Abp.File;
using Castle.Components.DictionaryAdapter;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class WorkFlowTaskCommentDto
    {
        public Guid StepId { get; set; }

        public string StepName { get; set; }


        public DateTime AddTime { get; set; }


        public string Comment { get; set; }
        public string SignFileId { get; set; }


        public string RecevieUserName { get; set; }


        public string SugguestionTitle { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new EditableList<GetAbpFilesOutput>();

    }


    public class WorkFlowTaskCommentResult
    {
        public List<WorkFlowTaskCommentDto> CommentList { get; set; } = new List<WorkFlowTaskCommentDto>();

        public List<WorkFlowTaskStepFileResult> StepFiles { get; set; } = new List<WorkFlowTaskStepFileResult>();
        public List<WorkFlowTaskStepComentResult> Coments { get; set; } = new List<WorkFlowTaskStepComentResult>();
    }
    public class WorkFlowTaskStepFileResult
    {
        public Guid StepID { get; set; }
        public string StepName { get; set; }
        public List<GetAbpFilesTaskOutput> FileList { get; set; } = new List<GetAbpFilesTaskOutput>();
    }
    public class WorkFlowTaskStepComentResult
    {
        public Guid TaskId { get; set; }
        public string Comment { get; set; }
    }
}
