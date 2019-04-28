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
    [AutoMapFrom(typeof(B_InventoryAddRecord))]
    public class B_InventoryAddRecordListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Goodsid
        /// </summary>
        public Guid Goodsid { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public B_InventoryAddConfigEnum Status { get; set; }

        /// <summary>
        /// ConfirmUserId
        /// </summary>
        public long? ConfirmUserId { get; set; }


        public string ConfirmUserName { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
