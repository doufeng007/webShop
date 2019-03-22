using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace XZGL
{
    [AutoMapTo(typeof(XZGLCompany))]
    public class CreateXZGLCompanyInput 
    {
        #region 表字段


        /// <summary>
        /// 类型
        /// </summary>
        public Guid Type { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(300,ErrorMessage = "名称长度必须小于300")]
        [Required(ErrorMessage = "必须填写名称")]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? EndTime { get; set; }



        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();
        #endregion
    }
}