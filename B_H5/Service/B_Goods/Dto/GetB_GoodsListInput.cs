using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_GoodsListInput : PagedAndSortedInputDto, IShouldNormalize
    {
       

        /// <summary>
        /// 商品大类
        /// </summary>
        public Guid? CategroyIdP { get; set; }

        /// <summary>
        /// 商品小类
        /// </summary>
        public Guid? CategroyId { get; set; }

       


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }

    public class GetB_GoodsInventoryListInput : PagedAndSortedInputDto, IShouldNormalize
    {


        /// <summary>
        /// 商品大类
        /// </summary>
        public Guid? CategroyIdP { get; set; }

        /// <summary>
        /// 商品小类
        /// </summary>
        public Guid? CategroyId { get; set; }




        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }


    public class GetB_GoodsListByCategroyIdInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public Guid CategroyId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
