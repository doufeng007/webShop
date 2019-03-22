using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.Enum;

namespace HR
{
    [AutoMapFrom(typeof(EmployeeTrainingSystem))]
    public class EmployeeTrainingSystemListOutputDto
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 制度标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 制度类型
        /// </summary>
        public TrainingSystemType Type { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


    }
}
