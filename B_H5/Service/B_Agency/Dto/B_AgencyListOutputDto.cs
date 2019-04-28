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
    /// <summary>
    /// 代理列表
    /// </summary>
    [AutoMapFrom(typeof(B_Agency))]
    public class B_AgencyListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 代理级别
        /// </summary>
        public string AgencyLevelName { get; set; }


        /// <summary>
        /// 代理编号
        /// </summary>
        public string AgenCyCode { get; set; }


        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();

    }



    public class B_AgencyManagerListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 代理编号
        /// </summary>
        public string AgenCyCode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 代理级别
        /// </summary>
        public string AgencyLevelName { get; set; }


        public Guid AgencyLevelId { get; set; }


        public string Tel { get; set; }

        public string WxId { get; set; }

        



        public B_AgencyAcountStatusEnum Status { get; set; }



        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }




    }
}
