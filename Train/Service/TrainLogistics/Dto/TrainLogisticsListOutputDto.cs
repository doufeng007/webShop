using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;

namespace Train
{
    [AutoMapFrom(typeof(TrainLogistics))]
    public class TrainLogisticsListOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 培训编号
        /// </summary>
        public Guid 培训编号 { get; set; }

        /// <summary>
        /// 类型名
        /// </summary>
        public string 类型名 { get; set; }

        /// <summary>
        /// 类型值
        /// </summary>
        public string 类型值 { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
