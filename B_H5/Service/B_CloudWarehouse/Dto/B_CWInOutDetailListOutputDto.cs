using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.File;

namespace B_H5
{
    public class B_CWInOutDetailListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }


        public B_CWInOrOutEnum InOrOut { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime MakeTime { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }


    }
}
