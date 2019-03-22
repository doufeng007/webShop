using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(ProjectRealationUser))]
    public class ProjectRealationUserDto : EntityDto<Guid>
    {
        public Guid InstanceID { get; set; }

        public Guid FlowID { get; set; }

        public Guid StepID { get; set; }

        public long UserID { get; set; }
    }

    public class ProjectRelationUserCreate {
        public Guid FlowID { get; set; }

        public Guid StepID { get; set; }

        public Guid GroupID { get; set; }

        public Guid TaskID { get; set; }

        public string InstanceID { get; set; }

        public List<WorkFlowTask> NextTasks { get; set; } = new List<WorkFlowTask>();
    }
}
