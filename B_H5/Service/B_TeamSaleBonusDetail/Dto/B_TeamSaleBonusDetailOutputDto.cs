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

namespace B_H5
{
    [AutoMapFrom(typeof(B_TeamSaleBonusDetail))]
    public class B_TeamSaleBonusDetailOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Pid
        /// </summary>
        public Guid Pid { get; set; }

        /// <summary>
        /// MaxSale
        /// </summary>
        public decimal MaxSale { get; set; }

        /// <summary>
        /// MinSale
        /// </summary>
        public decimal MinSale { get; set; }

        /// <summary>
        /// Scale
        /// </summary>
        public decimal Scale { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
