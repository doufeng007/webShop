using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class GetProjectFileAdditionalListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// FileTypeName
        /// </summary>
        public string FileTypeName { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid? ProjectBaseId { get; set; }

        /// <summary>
        /// PaperFileNumber
        /// </summary>
        public int? PaperFileNumber { get; set; }

        /// <summary>
        /// IsPaperFile
        /// </summary>
        public bool IsPaperFile { get; set; }

        /// <summary>
        /// IsNeedReturn
        /// </summary>
        public bool IsNeedReturn { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
