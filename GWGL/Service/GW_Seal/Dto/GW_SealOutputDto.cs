using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    public class GW_SealOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// KeepUser
        /// </summary>
        public long KeepUser { get; set; }


        public string KeepUser_Name { get; set; }

        /// <summary>
        /// AuditUser
        /// </summary>
        public string AuditUser { get; set; }


        public string AuditUser_Name { get; set; }

        /// <summary>
        /// SealType
        /// </summary>
        public string SealType_Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status_Title { get; set; }

        /// <summary>
        /// SealType
        /// </summary>
        public GW_SealTypeEnmu SealType { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GW_SealStatusEnmu Status { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();



    }
}
