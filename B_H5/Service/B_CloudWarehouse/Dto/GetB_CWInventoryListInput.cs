using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{

    public class GetB_CWInventoryListInput : PagedAndSortedInputDto, IShouldNormalize
    {


        public FirestLevelCategroyProperty CategroyPropertyId { get; set; }

        [Required(ErrorMessage = "必须填写类别")]
        public Guid CategroyId { get; set; }

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
