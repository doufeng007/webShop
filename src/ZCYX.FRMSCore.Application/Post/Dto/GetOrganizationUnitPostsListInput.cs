using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public class GetOrganizationUnitPostsListInput : PagedResultRequestDto
    {
        /// <summary>
        /// 查询一个机构
        /// </summary>
        public long OrgId { get; set; }
        /// <summary>
        /// 查询多个机构
        /// </summary>
        public List<long> OrgIds { get; set; }
    }
}
