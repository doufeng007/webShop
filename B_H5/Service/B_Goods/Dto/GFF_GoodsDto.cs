using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5
{
    public class GFF_GoodsDto
    {
        public string GoodsCode { get; set; }


        public string SKU { get; set; }

        public string CnName { get; set; }

        public string EnName { get; set; }


        public string CustomcCode { get; set; }


        public string ProducingArea { get; set; }

        public string Weight { get; set; }

        public string Length { get; set; }

        public string Width { get; set; }

        public string High { get; set; }

        public string Price { get; set; }

        public string Field2 { get; set; }
    }

    public class GFF_GoodsListDto
    {
        public List<GFF_GoodsDto> GFF_Goods { get; set; } = new List<GFF_GoodsDto>();
    }

}
