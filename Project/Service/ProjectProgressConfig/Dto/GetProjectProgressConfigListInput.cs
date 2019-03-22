using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetProjectProgressConfigListInput : PagedAndSortedInputDto, IShouldNormalize
    {
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



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
