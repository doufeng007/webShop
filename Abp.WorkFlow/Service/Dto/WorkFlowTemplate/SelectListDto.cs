using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow.Service.Dto
{
    /// <summary>
    /// 外键关联多对一下拉表
    /// </summary>
   public class SelectListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class GetModelSelectInput : PagedResultRequestDto, IShouldNormalize
    {
        public string Key { get; set; }
        /// <summary>
        /// 模型主键
        /// </summary>
        public Guid ModelId { get; set; }
        public void Normalize()
        {
            
        }
    }
}
