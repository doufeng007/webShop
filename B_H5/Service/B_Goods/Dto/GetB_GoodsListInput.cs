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
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid? CategroyId { get; set; }

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
