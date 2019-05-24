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
    [AutoMapFrom(typeof(B_MyAddress))]
    public class B_MyAddressOutputDto 
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
        /// Provinces
        /// </summary>
        public string Provinces { get; set; }

        /// <summary>
        /// County
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Addres
        /// </summary>
        public string Addres { get; set; }

        /// <summary>
        /// Consignee
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// Tel
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public string Name { get; set; }



    }
}
