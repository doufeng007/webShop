using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace Train
{
    public class UpdateTrainInput : CreateTrainInput
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }
    }
    public class UpdateTrainCopyForInput 
    {
        public Guid Id { get; set; }
    }
    public class UpdateTrainDocumentInput 
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
    }
}