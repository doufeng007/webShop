﻿using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;

namespace B_H5
{
    public class UpdateB_StoreSignUpInput : CreateB_StoreSignUpInput
    {
		public Guid Id { get; set; }        
    }
}