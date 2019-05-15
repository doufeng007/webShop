using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_TeamSaleBonus))]
    public class B_TeamSaleBonusListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// EffectTime
        /// </summary>
        public DateTime EffectTime { get; set; }

        /// <summary>
        /// FailureTime
        /// </summary>
        public DateTime? FailureTime { get; set; }

        /// <summary>
        /// Status
        /// </summary>


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public List<B_TeamSaleBonusDetailListOutputDto> Details { get; set; } = new List<B_TeamSaleBonusDetailListOutputDto>();


        public string CreatorUserName { get; set; }


    }
}
