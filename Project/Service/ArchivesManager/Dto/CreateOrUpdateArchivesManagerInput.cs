using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore;

namespace Project
{
    [AutoMap(typeof(ArchivesManager))]
    public class CreateOrUpdateArchivesManagerInput: CreateWorkFlowInstance
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? Id { get; set; }
        [LogColumn("档案类型", true)]
        public int ArchivesType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("案卷号", true)]
        public string ArchivesNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("卷号", true)]
        public string VolumeNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("档案名称", true)]
        public string ArchivesName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("存放位置", true)]
        public string Location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("保密级别", true)]
        public int? SecrecyLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ArchivesNumber1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("档案份数", true)]
        public int? PageNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [LogColumn("说明", true)]
        public string Summary { get; set; }


        public List<CreateOrUpdateArchivesFileInput> Files { get; set; }

        public bool IsUpdateForChange { get; set; }
        public CreateOrUpdateArchivesManagerInput()
        {
            this.Files = new List<CreateOrUpdateArchivesFileInput>();
        }
    }
}
