using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;

namespace B_H5
{
    [AutoMapFrom(typeof(B_InviteUrl))]
    public class B_InviteUrlOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 代理等级id
        /// </summary>
        public Guid AgencyLevelId { get; set; }


        /// <summary>
        /// 代理等级
        /// </summary>
        public string AgencyLevelName { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int ValidityDataType { get; set; }

        /// <summary>
        /// 有效次数
        /// </summary>
        public int AvailableCount { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 邀请代理
        /// </summary>
        public string CreateAgencyName { get; set; }

        /// <summary>
        /// 邀请代理等级id
        /// </summary>
        public Guid CreateAgencyLevelId { get; set; }

        /// <summary>
        /// 邀请代理等级Name
        /// </summary>
        public string CreateAgencyLevelName { get; set; }


        /// <summary>
        /// 联系电话
        /// </summary>
        public string CreateAgencyTel { get; set; }


        /// <summary>
        /// 联系地址
        /// </summary>
        public string CreateAgencyAddress { get; set; }




    }
}
