using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMap(typeof(ProjectAreas))]
    public class ProjectAreaDto : CreationAuditedEntityDto<Guid>
    {

        /// <summary>
        /// 片区名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public long? User_Id { get; set; }


        public string Surname { get; set; }
        /// <summary>
        /// 上级片区
        /// </summary>
        public Guid? Parent_Id { get; set; }


        public string ParentName { get; set; }


        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool? IsDeleted { get; set; } = false;
    }
}
