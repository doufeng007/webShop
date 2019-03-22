using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public class OrganizationUnitPostsListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// OrganizationUnitId
        /// </summary>
        public long OrganizationUnitId { get; set; }


        public string OrganizationUnitName { get; set; }

        /// <summary>
        /// PostId
        /// </summary>
        public Guid PostId { get; set; }


        public string PostName { get; set; }

        /// <summary>
        /// PrepareNumber
        /// </summary>
        public int PrepareNumber { get; set; }


        public int NowNumber { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
