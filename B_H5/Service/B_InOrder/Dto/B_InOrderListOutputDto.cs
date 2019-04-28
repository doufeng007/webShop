using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.File;

namespace B_H5
{
    public class B_InOrderListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string OrderNo { get; set; }


        public DateTime CreationTime { get; set; }

        public InOrderStatusEnum Status { get; set; }


        /// <summary>
        /// 代理名称
        /// </summary>
        public string UserName { get; set; }


        public long UserId { get; set; }



        /// <summary>
        /// 进货数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal Amout { get; set; }

        /// <summary>
        /// 货款
        /// </summary>
        public decimal GoodsPayment { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; }


        public Guid CategroyId { get; set; }

        public string CategroyTitle { get; set; }


        /// <summary>
        /// 进货商品类别缩略图
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();

    }
}
