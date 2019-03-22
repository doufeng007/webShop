using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;

namespace Project
{
    [AutoMapTo(typeof(ProjectFileAdditional))]
    public class CreateProjectFileAdditionalInput
    {
        #region 表字段
        /// <summary>
        /// FileTypeName
        /// </summary>
        public string FileTypeName { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid ProjectBaseId { get; set; }

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


        public List<GetAbpFilesOutput> FileList { get; set; }


        #endregion
    }

    public class UpdateProjectFileAdditionalInput
    {
        public Guid Id { get; set; }

        /// <summary>
        /// FileTypeName
        /// </summary>
        public string FileTypeName { get; set; }

        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid ProjectBaseId { get; set; }

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


        public List<GetAbpFilesOutput> FileList { get; set; }
    }
}