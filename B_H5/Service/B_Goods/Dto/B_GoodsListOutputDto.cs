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
    [AutoMapFrom(typeof(B_Goods))]
    public class B_GoodsListOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string ModeType { get; set; }


        /// <summary>
        /// 规格
        /// </summary>
        public string Spe { get; set; }

        
        /// <summary>
        /// 单位
        /// </summary>

        public string UnitName { get; set; }




        public GoodStatusEnum Status { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品小类
        /// </summary>
        public string CategroyIdName { get; set; }

        /// <summary>
        /// 商品大类
        /// </summary>
        public string CategroyIdPName { get; set; }



        /// <summary>
        /// 商品大类
        /// </summary>
        public Guid CategroyIdP { get; set; }


        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid? CategroyId { get; set; }







        /// <summary>
        /// 原价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>
        public decimal Pirce1 { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();
    }

    public class B_GoodsInventoryListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string ModeType { get; set; }


        /// <summary>
        /// 规格
        /// </summary>
        public string Spe { get; set; }


        /// <summary>
        /// 单位
        /// </summary>

        public string UnitName { get; set; }




        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品小类
        /// </summary>
        public string CategroyIdName { get; set; }

        /// <summary>
        /// 商品大类
        /// </summary>
        public string CategroyIdPName { get; set; }



        /// <summary>
        /// 商品大类
        /// </summary>
        public Guid CategroyIdP { get; set; }


        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid? CategroyId { get; set; }






        /// <summary>
        /// 当前库存
        /// </summary>
        public int Inventory { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
