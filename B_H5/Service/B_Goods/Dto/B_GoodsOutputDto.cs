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
    [AutoMapFrom(typeof(B_Goods))]
    public class B_GoodsOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid? CategroyId { get; set; }

        /// <summary>
        /// 商品大类
        /// </summary>
        public Guid CategroyIdP { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Pirce1
        /// </summary>
        public decimal Pirce1 { get; set; }

        /// <summary>
        /// Price2
        /// </summary>
        public decimal Price2 { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        public string Code { get; set; }


        public string ModeType { get; set; }



        public string Spe { get; set; }

        public Guid UnitId { get; set; }


        public string UnitName { get; set; }


        public GoodStatusEnum Status { get; set; }


        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();



    }
}
