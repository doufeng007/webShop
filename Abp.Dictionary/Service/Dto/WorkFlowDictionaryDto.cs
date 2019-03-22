using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlowDictionary
{
    [AutoMap(typeof(AbpDictionary))]
    public class WorkFlowDictionaryDto: EntityDto<Guid>
    {

        /// <summary>
        /// 上级ID
        /// </summary>
        public Guid? ParentID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 唯一代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 其它信息
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }
}
