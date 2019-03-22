using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace HR
{
    public class UpdateDailyInput : CreateDailyInput
    {
		public Guid Id { get; set; }        
    }
    public class DailyGroupBy {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}