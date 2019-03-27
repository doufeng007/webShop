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
    public class B_CWInventoryListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 可提前数量
        /// </summary>
        public int CanExtractCount { get; set; }

        /// <summary>
        /// 取货数量
        /// </summary>
        public int TakeLessCount { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();


    }
}
