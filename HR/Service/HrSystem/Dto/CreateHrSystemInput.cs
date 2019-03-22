using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using HR.Enum;

namespace HR
{
    [AutoMapTo(typeof(HrSystem))]
    public class CreateHrSystemInput 
    {
        #region 表字段
        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "必须填写标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public HrSystemType TypeId { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        public string UserIds { get; set; }

        /// <summary>
        /// 是否全公司
        /// </summary>
        public bool IsAll { get; set; }

        /// <summary>
        /// 部门权限
        /// </summary>
        public string OrgIds { get; set; }

        /// <summary>
        /// 人员权限
        /// </summary>
        public string UserNames { get; set; }

        /// <summary>
        /// 部门权限
        /// </summary>
        public string OrgNames { get; set; }


		
        #endregion
    }
}