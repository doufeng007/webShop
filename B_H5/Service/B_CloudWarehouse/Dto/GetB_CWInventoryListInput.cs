using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
   
    public class GetB_CWInventoryListInput : PagedAndSortedInputDto, IShouldNormalize
    {


        public FirestLevelCategroyProperty CategroyPropertyId { get; set; }
        /// <summary>
        /// IsActive
        /// </summary>
        public bool? IsActive { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
