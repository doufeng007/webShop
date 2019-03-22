using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [AutoMapFrom(typeof(ProjectFileAdditional))]
    public class ProjectFileAdditionalListOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// FileTypeName
        /// </summary>
        public string FileTypeName { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid? ProjectBaseId { get; set; }

        /// <summary>
        /// PaperFileNumber
        /// </summary>
        public int? PaperFileNumber { get; set; }

        /// <summary>
        /// IsPaperFile
        /// </summary>
        public bool IsPaperFile { get; set; }

        /// <summary>
        /// IsNeedReturn
        /// </summary>
        public bool IsNeedReturn { get; set; }

        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }


        public string CreateUserName { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }
    }
}
