using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetWorkFlowTaskCommentInput
    {
        [Required]
        public Guid FlowId { get; set; }

        [Required]
        public string InstanceId { get; set; }
    }
    public class GetWorkFlowTaskCommentUserInput
    {
        [Required]
        public Guid FlowId { get; set; }

        [Required]
        public string InstanceId { get; set; }

        [Required]
        public long UserId { get; set; }
    }
}
