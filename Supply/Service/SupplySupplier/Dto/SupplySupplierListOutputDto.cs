using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Supply
{
    [AutoMapFrom(typeof(SupplySupplier))]
    public class SupplySupplierListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 供应商类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }


        /// <summary>
        /// 销售联系人
        /// </summary>
        public string SalesContact { get; set; }

        /// <summary>
        /// 销售联系人电话
        /// </summary>
        public string SalesContactTel { get; set; }

        /// <summary>
        /// 评价备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }
    }
}
