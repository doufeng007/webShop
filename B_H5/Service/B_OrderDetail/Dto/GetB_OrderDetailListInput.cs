using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_OrderDetailListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// BId
        /// </summary>
        public Guid BId { get; set; }

        /// <summary>
        /// BType
        /// </summary>
        public int BType { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// CategroyId
        /// </summary>
        public Guid CategroyId { get; set; }

        /// <summary>
        /// GoodsId
        /// </summary>
        public Guid? GoodsId { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal? Amout { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
