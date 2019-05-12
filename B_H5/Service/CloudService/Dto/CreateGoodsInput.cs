using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5.Service.CloudService.Dto
{
    public class CreateGoodsInput
    {
        public GFF_Goods GFF_Goods { get; set; } = new GFF_Goods();
    }


    public class GFF_GoodsItem
    {
        public string GoodsCode { get; set; } = "1";
        public string SKU { get; set; } = "1";
        public string CnName { get; set; } = "1";
        public string EnName { get; set; } = "1";
        public string CustomcCode { get; set; } = "1";
        public string ProducingArea { get; set; } = "1";
        public string Weight { get; set; } = "1";
        public string Length { get; set; } = "1";
        public string Width { get; set; } = "1";
        public string High { get; set; } = "1";
        public string Price { get; set; } = "1";
        public string Field2 { get; set; } = "1";
    }

    public class GFF_Goods
    {
        public List<GFF_GoodsItem> item { get; set; } = new List<GFF_GoodsItem>();
    }
}
