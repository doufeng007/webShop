using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR
{
    [AutoMapTo(typeof(QuickLinkBase))]
    public class CreateQuickLinkBaseInput 
    {
        #region 表字段
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage ="名称不能为空")]
        [MaxLength(500,ErrorMessage ="名称不能超过250字")]
        public string Name { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [Required(ErrorMessage = "链接不能为空")]
        [MaxLength(500, ErrorMessage = "链接不能超过500个字符")]
        public string Link { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public int Sort { get; set; }



        #endregion
    }
}