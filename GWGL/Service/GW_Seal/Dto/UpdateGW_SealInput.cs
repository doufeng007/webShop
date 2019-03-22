using System;
using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using ZCYX.FRMSCore;

namespace GWGL
{
    public class UpdateGW_SealInput : CreateGW_SealInput
    {
		public Guid Id { get; set; }        
    }

    public class GW_SealChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// BeginTime
        /// </summary>
        [LogColumn("印章名称", IsLog = true)]
        public string Name { get; set; }


        /// <summary>
        /// KeepUser
        /// </summary>
        [LogColumn("保管人", IsLog = true)]
        public string KeepUser_Name { get; set; }

        /// <summary>
        /// AuditUser
        /// </summary>
        [LogColumn("授权人", IsLog = true)]
        public string AuditUser_Name { get; set; }

        /// <summary>
        /// SealType
        /// </summary>
        [LogColumn("印章类型", IsLog = true)]
        public string SealType_Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [LogColumn("状态", IsLog = true)]
        public string Status_Title { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [LogColumn("备注", IsLog = true)]
        public string Remark { get; set; }


        /// <summary>
        /// 电子资料
        /// </summary>
        [LogColumn("印章")]
        public List<AbpFileChangeDto> Files { get; set; }
    }
}