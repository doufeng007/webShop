using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace B_H5
{
    public class UpdateB_GoodsInput : CreateB_GoodsInput
    {
		public Guid Id { get; set; }        
    }
}