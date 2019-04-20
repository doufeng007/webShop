using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_WithdrawalListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// BankName
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// BankBranchName
        /// </summary>
        public string BankBranchName { get; set; }

        /// <summary>
        /// BankUserName
        /// </summary>
        public string BankUserName { get; set; }

        /// <summary>
        /// BankNumber
        /// </summary>
        public string BankNumber { get; set; }

        /// <summary>
        /// Amout
        /// </summary>
        public decimal Amout { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
