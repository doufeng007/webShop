using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using Train.Enum;

namespace Train
{
    [AutoMapTo(typeof(TrainUserExperience))]
    public class CreateTrainUserExperienceInput 
    {
        #region 表字段

        /// <summary>
        /// 流程编号
        /// </summary>
        public Guid TrainId { get; set; }

        /// <summary>
        /// 心得体会
        /// </summary>
        public string Experience { get; set; }

        public  TrainExperienceType ExperienceType { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

        #endregion
    }
    public class TrainUserExperienceUseInput
    {
        public List<Guid> Guids { get; set; }
    }
}