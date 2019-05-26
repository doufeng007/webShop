using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_AgencySalesListInput : PagedAndSortedInputDto, IShouldNormalize
    {


        /// <summary>
        /// SalesDate
        /// </summary>
        [Range(1000, 7777, ErrorMessage = "必须输入年数")]
        public int SalesDateYear { get; set; }

        [Range(1, 12, ErrorMessage = "必须输入月数")]
        public int SalesDateMonth { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }


    public class GetB_AgencyCategroySalesListInput : GetB_AgencySalesListInput, IShouldNormalize
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









    public class GetB_AgencySalesInput
    {
        public int Year { get; set; }


        public int Month { get; set; }



        public Guid? CategroyId { get; set; }
    }



}
