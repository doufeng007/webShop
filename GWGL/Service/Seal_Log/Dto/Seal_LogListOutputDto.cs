using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    [AutoMapFrom(typeof(Seal_Log))]
    public class Seal_LogListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Seal_Id
        /// </summary>
        public Guid Seal_Id { get; set; }

        public string Seal_Name { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }


        public string User_Name { get; set; }

        public string User_OrgName { get; set; }

        public GW_SealTypeEnmu SealType { get; set; }

        public string SealType_Name { get; set; }


        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Copies
        /// </summary>
        public int Copies { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
