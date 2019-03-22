using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supply
{
    [AutoMapFrom(typeof(CuringProcurementPlan))]
    public class CuringProcurementPlanOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// MainId
        /// </summary>
        public Guid MainId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        public string Money { get; set; }

        /// <summary>
        /// Des
        /// </summary>
        public string Des { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        public string TypeName { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }

        public string StatusTitle { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }


        public CuringProcurementPlanOutputDto()
        {
            this.FileList = new List<GetAbpFilesOutput>();
        }



    }
}
