using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_CategroyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// P_Id
        /// </summary>
        public Guid? P_Id { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }


    public class GetB_CategroyListByCategroyIdInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 类别id
        /// </summary>
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
