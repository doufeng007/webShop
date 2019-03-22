using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace Supply
{
    public class UserSupplyListDto
    {
        public Guid Id { get; set; }

        public Guid SupplyId { get; set; }

        public string Supply_Name { get; set; }

        public string Supply_Version { get; set; }

        public decimal Supply_Money { get; set; }

        public int SupplyType { get; set; }

        public string SupplyTypeTitle { get; set; }

        public string Supply_Code { get; set; }

        /// <summary>
        /// 用品所属者id
        /// </summary>
        public string Supply_UserId { get; set; }


        /// <summary>
        /// 用品所属者名称
        /// </summary>
        public string Supply_UserId_Name { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        public string Unit { get; set; }
        public DateTime CreationTime { get; set; }

    }
}
