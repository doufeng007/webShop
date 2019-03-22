using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace TaskGL
{
    public class UpdateTaskManagementInput : CreateTaskManagementInput
    {
		public Guid FlowId { get; set; }
       public Guid InStanceId { get; set; }
       public bool IsUpdateForChange { get; set; }     
    }
    public class UpdateTaskManagementByChangeIdInput : UpdateTaskManagementInput
    {
        /// <summary>
        /// 变更对应的编号
        /// </summary>
       public Guid? ChangeId { get; set; }        
    }
}