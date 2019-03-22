using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    [AutoMapTo(typeof(Employees_Sign))]
    public class CreateEmployees_SignInput 
    {
        #region 表字段
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// SignType
        /// </summary>
        public GW_EmployeesSignTypelEnmu SignType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GW_EmployeesSignStatusEnmu Status { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();



        #endregion
    }
}