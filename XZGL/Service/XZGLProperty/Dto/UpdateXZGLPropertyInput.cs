﻿using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace XZGL
{
    public class UpdateXZGLPropertyInput : CreateXZGLPropertyInput
    {
		public Guid Id { get; set; }        
    }
}