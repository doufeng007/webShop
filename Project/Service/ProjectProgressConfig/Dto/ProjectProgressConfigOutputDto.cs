using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(ProjectProgressConfig))]
    public class ProjectProgressConfigOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid? ProjectBaseId { get; set; }

        /// <summary>
        /// FirstAuditKey
        /// </summary>
        public int FirstAuditKey { get; set; }

        /// <summary>
        /// JiliangKey
        /// </summary>
        public int JiliangKey { get; set; }

        /// <summary>
        /// JijiaKey
        /// </summary>
        public int JijiaKey { get; set; }

        /// <summary>
        /// SelfAuditKey
        /// </summary>
        public int SelfAuditKey { get; set; }

        /// <summary>
        /// SecondAuditKey
        /// </summary>
        public int SecondAuditKey { get; set; }

        /// <summary>
        /// LastAuditKey
        /// </summary>
        public int LastAuditKey { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
