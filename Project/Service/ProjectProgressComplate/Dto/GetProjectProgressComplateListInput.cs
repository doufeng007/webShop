using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetProjectProgressComplateListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid ProjectBaseId { get; set; }

        /// <summary>
        /// FirstAuditComplateTime
        /// </summary>
        public DateTime? FirstAuditComplateTime { get; set; }

        /// <summary>
        /// FirstAduitDelayHour
        /// </summary>
        public int? FirstAduitDelayHour { get; set; }

        /// <summary>
        /// JiliangComplateTime
        /// </summary>
        public DateTime? JiliangComplateTime { get; set; }

        /// <summary>
        /// JiliangDelayHour
        /// </summary>
        public int? JiliangDelayHour { get; set; }

        /// <summary>
        /// JijiaComplateTime
        /// </summary>
        public DateTime? JijiaComplateTime { get; set; }

        /// <summary>
        /// JijiaDelayHour
        /// </summary>
        public int? JijiaDelayHour { get; set; }

        /// <summary>
        /// SelfAuditComplateTime
        /// </summary>
        public DateTime? SelfAuditComplateTime { get; set; }

        /// <summary>
        /// SelfAuditDelayHour
        /// </summary>
        public int? SelfAuditDelayHour { get; set; }

        /// <summary>
        /// SecondAuditComplateTime
        /// </summary>
        public DateTime? SecondAuditComplateTime { get; set; }

        /// <summary>
        /// SecondAuditDelayHour
        /// </summary>
        public int? SecondAuditDelayHour { get; set; }

        /// <summary>
        /// LastAuditComplateTime
        /// </summary>
        public DateTime? LastAuditComplateTime { get; set; }

        /// <summary>
        /// LastAuditDelayHour
        /// </summary>
        public int? LastAuditDelayHour { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
