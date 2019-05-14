using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace B_H5.Service.B_Agency.Dto
{
    public class B_AgencyCountStatisticalDto
    {
        /// <summary>
        /// 总人数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 团队总数
        /// </summary>
        public int TeamCount { get; set; }

        public List<B_AgencyLeavelCountStatisticalDto> Leavels { get; set; } = new List<B_AgencyLeavelCountStatisticalDto>();

    }


    public class B_AgencyLeavelCountStatisticalDto
    {
        public int Leavel { get; set; }

        /// <summary>
        /// 代理数量
        /// </summary>
        public int Count { get; set; }


    }

    public class B_AgencyAddCountStatisDto
    {
        public string Date { get; set; }

        public int Count { get; set; }
    }

    public class B_AgencyAddCountStatisInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }


        public DateTime EndDate { get; set; }

        /// <summary>
        /// 1 是day 2是month
        /// </summary>
        public int DayOrMonth { get; set; }

        /// <summary>
        /// 空是所有代理  团队数和一级代理人数= 1
        /// </summary>
        public int? Leavel { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }



    public class TeamCountStatisDto
    {
        public string Name { get; set; }


        public int AgencyCount { get; set; }

    }

    public class TeamCountStatisInput : PagedAndSortedInputDto, IShouldNormalize
    {

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " Order ";
            }
        }
    }


    public class AgencyAreaStatisDto
    {
        public string Name { get; set; }


        public int AgencyCount { get; set; }

        /// <summary>
        /// 占比
        /// </summary>
        public decimal Proportion { get; set; }

    }

    
}
