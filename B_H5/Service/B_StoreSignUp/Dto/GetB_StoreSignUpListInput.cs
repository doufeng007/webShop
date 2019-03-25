using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_StoreSignUpListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Provinces
        /// </summary>
        public string Provinces { get; set; }

        /// <summary>
        /// County
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// BankNumber
        /// </summary>
        public string BankNumber { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// BankName
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// OpenDate
        /// </summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// StorArea
        /// </summary>
        public string StorArea { get; set; }

        /// <summary>
        /// Goods
        /// </summary>
        public string Goods { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
