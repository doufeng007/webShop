using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore.Application
{
    [AutoMapFrom(typeof(WorkFlowOrganizationUnits))]
    public class WorkFlowOrganizationUnitDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public int Sort { get; set; }

        public int Depth { get; set; }

        public int ChildsLength { get; set; }

        public string ChargeLeader { get; set; }

        public string Leader { get; set; }

        public string Note { get; set; }


        public List<OrgPostInfoDto> Posts { get; set; }

        public WorkFlowOrganizationUnitDto()
        {
            Posts = new List<OrgPostInfoDto>();
        }


    }


    public class OrgPostInfoDto
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public string PostName { get; set; }

        public int PrepareNumber { get; set; }

        /// <summary>
        /// 岗位级别，值越小级别越高
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 岗位角色
        /// </summary>
        public List<string> RoleNames { get; set; }
    }
}
