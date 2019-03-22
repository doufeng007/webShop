using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;

namespace CWGL
{
    [AutoMapTo(typeof(CWGLWagePay))]
    public class CreateCWGLWagePayInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// 工资日期
        /// </summary>
        public DateTime WageDate { get; set; }

        

		//public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}