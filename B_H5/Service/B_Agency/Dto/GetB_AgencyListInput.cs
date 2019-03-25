using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5
{
    public class GetB_AgencyListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// AgencyLevel
        /// </summary>
        public Guid AgencyLevel { get; set; }

        /// <summary>
        /// AgenCyCode
        /// </summary>
        public string AgenCyCode { get; set; }

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
        /// Type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// SignData
        /// </summary>
        public DateTime SignData { get; set; }

        /// <summary>
        /// Agreement
        /// </summary>
        public string Agreement { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// UnitId
        /// </summary>
        public string UnitId { get; set; }



        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
