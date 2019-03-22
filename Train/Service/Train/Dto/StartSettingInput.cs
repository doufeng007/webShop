using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Train
{
    public class StartSettingInput 
    {
        [Required,Range(0,120)]
        public int Minute { get; set; }
    }
}