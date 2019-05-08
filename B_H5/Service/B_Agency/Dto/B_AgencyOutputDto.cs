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
    [AutoMapFrom(typeof(B_Agency))]
    public class B_AgencyOutputDto 
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
        /// 名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 代理级别
        /// </summary>
        public string AgencyLevelName { get; set; }

        /// <summary>
        /// 代理编码
        /// </summary>
        public string AgenCyCode { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Provinces { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }


        /// <summary>
        /// 区县
        /// </summary>
        public string County { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime SignData { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();


        /// <summary>
        /// 打款凭证
        /// </summary>
        public List<GetAbpFilesOutput> CredentFiles { get; set; } = new List<GetAbpFilesOutput>();


        /// <summary>
        /// 手持凭证
        /// </summary>
        public List<GetAbpFilesOutput> HandleCredentFiles { get; set; } = new List<GetAbpFilesOutput>();

        public Guid ApplyId { get; set; }


        /// <summary>
        /// 身份证号码
        /// </summary>
        public string PNumber { get; set; }

    }
}
