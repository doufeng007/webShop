using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    [AutoMapFrom(typeof(WorkFlow))]
    public class GetWorkFlowListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? InstallDate { get; set; }
        public DateTime? LastModificationTime { get; set; }


        public int VersionNumber { get; set; }

        /// <summary>
        /// 做版本切换，暂时不使用，之后需要单独接口实现
        /// </summary>
        public List<int> VersionNumbers { get; set; }


        /// <summary>
        /// 状态 1:设计中 2:已安装 3:已卸载 4:已删除
        /// </summary>
        public int Status { get; set; }

    }
}
