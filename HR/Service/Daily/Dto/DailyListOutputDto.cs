using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace HR
{
    [AutoMapFrom(typeof(Daily))]
    public class DailyListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Personnel
        /// </summary>
        public string Personnel { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// OverState
        /// </summary>
        public string OverState { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        public string Note { get; set; }


    }
}
