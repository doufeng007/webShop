using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    public class OrganizationUnitPostsDto
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public virtual int? TenantId { get; set; }
       public long OrganizationUnitId { get; set; }
        public string OrganizationName { get; set; }
        public Guid PostId { get; set; }
        public bool IsMain { get; set; }
        public string PostName { get; set; }
        /// <summary>
        /// 编制岗位
        /// </summary>
        public int PrepareNumber { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 现存人数
        /// </summary>
        public int Number { get; set; }
    }
}
