using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace Train
{
    public class UpdateUserTrainScoreRecordInput : CreateUserTrainScoreRecordInput
    {
		public Guid Id { get; set; }        
    }
}