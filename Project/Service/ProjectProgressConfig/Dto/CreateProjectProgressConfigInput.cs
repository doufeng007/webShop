using Abp.AutoMapper;
using System;

namespace Project
{
    [AutoMapTo(typeof(ProjectProgressConfig))]
    public class CreateProjectProgressConfigInput
    {
        #region 表字段
        /// <summary>
        /// ProjectBaseId
        /// </summary>
        public Guid? ProjectBaseId { get; set; }

        /// <summary>
        /// FirstAuditKey
        /// </summary>
        public int FirstAuditKey { get; set; }

        /// <summary>
        /// JiliangKey
        /// </summary>
        public int JiliangKey { get; set; }

        /// <summary>
        /// JijiaKey
        /// </summary>
        public int JijiaKey { get; set; }

        /// <summary>
        /// SelfAuditKey
        /// </summary>
        public int SelfAuditKey { get; set; }

        /// <summary>
        /// SecondAuditKey
        /// </summary>
        public int SecondAuditKey { get; set; }

        /// <summary>
        /// LastAuditKey
        /// </summary>
        public int LastAuditKey { get; set; }


        #endregion
    }
}