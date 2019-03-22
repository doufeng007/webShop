using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace GWGL
{
    public class UpdateEmployeeReceiptInput : EmployeeReceipt
    {
        public Guid FlowId { get; set; }
        public bool IsUpdateForChange { get; set; }

        public List<GetAbpFilesOutput> OldFileList { get; set; } = new List<GetAbpFilesOutput>();
    }
    public class EmployeeReceiptAddWriteInput {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
    }
}