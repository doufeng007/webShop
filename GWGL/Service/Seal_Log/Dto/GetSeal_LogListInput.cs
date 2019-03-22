using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace GWGL
{
    public class GetSeal_LogListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// Seal_Id
        /// </summary>
        public Guid Seal_Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Copies
        /// </summary>
        public int Copies { get; set; }

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
