using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    [AutoMapFrom(typeof(SupplyBase))]
    public class SupplyListDto 
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Unit { get; set; }
        public string Version { get; set; }


        public decimal Money { get; set; }


        public int Type { get; set; }
        public string  Type_Name { get; set; }


        public int Status { get; set; }

        public string Code { get; set; }

        public string StatusTitle { get; set; }

        public string CreationUserName { get; set; }
        public string UserId_Name { get; set; }

        public string UserId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? ProductDate { get; set; }


        public DateTime? ExpiryDate { get; set; }
        public DateTime LastModificationTime { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime? PutInDate { get; set; }
    }
}
