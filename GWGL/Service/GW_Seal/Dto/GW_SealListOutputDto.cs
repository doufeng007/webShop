using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Authorization.Users;

namespace GWGL
{
    public class GW_SealListOutputDto
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
        public string KeepUser_Name { get; set; }

        /// <summary>
        /// AuditUser
        /// </summary>
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
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? ActiveDate { get; set; }


        public DateTime CreationTime { get; set; }


    }
}
