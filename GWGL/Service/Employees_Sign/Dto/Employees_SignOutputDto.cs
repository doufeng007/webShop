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
using ZCYX.FRMSCore;
using ZCYX.FRMSCore.Application;

namespace GWGL
{
    public class Employees_SignOutputDto 
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }


        public string UserName { get; set; }
        /// <summary>
        /// SignType
        /// </summary>
        public GW_EmployeesSignTypelEnmu SignType { get; set; }

        public string SignType_Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public GW_EmployeesSignStatusEnmu Status { get; set; }

        public string Status_Title { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
    }


    public class Employees_SignChangeDto
    {
        [LogColumn("主键", IsLog = false)]
        public Guid Id { get; set; }

        /// <summary>
        /// SealType
        /// </summary>
        [LogColumn("签名类型", IsLog = true)]
        public string SignType_Name { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [LogColumn("状态", IsLog = true)]
        public string Status_Title { get; set; }  
        /// <summary>
        /// 员工姓名
        /// </summary>
        [LogColumn("员工姓名", IsLog = true)]
        public string Name { get; set; }

        /// <summary>
        /// 电子资料
        /// </summary>
        [LogColumn("签名")]
        public List<AbpFileChangeDto> Files { get; set; }


    }
}
