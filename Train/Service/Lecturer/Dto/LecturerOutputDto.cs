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
using HR;

namespace Train
{
    [AutoMapFrom(typeof(Lecturer))]
    public class LecturerOutputDto 
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 讲师姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课时费
        /// </summary>
        public decimal TeachSubsidy { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankId { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public string Bank { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string OpenBank { get; set; }

        /// <summary>
        /// 讲师简介
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

    }
}
