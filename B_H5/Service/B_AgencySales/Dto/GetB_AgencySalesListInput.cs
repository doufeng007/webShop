using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
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
        public int SalesDateYear { get; set; }


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
