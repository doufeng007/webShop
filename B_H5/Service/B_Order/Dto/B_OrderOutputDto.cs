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
    [AutoMapFrom(typeof(B_Order))]
    public class B_OrderOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal Amout { get; set; }

        /// <summary>
        /// Stauts
        /// </summary>
        public int Stauts { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


		
    }
}
