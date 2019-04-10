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
    [AutoMapFrom(typeof(B_OrderDetail))]
    public class B_OrderDetailListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// BId
        /// </summary>
        public Guid BId { get; set; }

        /// <summary>
        /// BType
        /// </summary>
        public int BType { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// GoodsId
        /// </summary>
        public Guid? GoodsId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal? Amout { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
