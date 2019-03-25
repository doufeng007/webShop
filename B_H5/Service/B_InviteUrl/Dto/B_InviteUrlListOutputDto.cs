using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_InviteUrl))]
    public class B_InviteUrlListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// AgencyLevel
        /// </summary>
        public Guid AgencyLevel { get; set; }

        /// <summary>
        /// ValidityDataType
        /// </summary>
        public int ValidityDataType { get; set; }

        /// <summary>
        /// AvailableCount
        /// </summary>
        public int AvailableCount { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
