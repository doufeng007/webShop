using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace B_H5
{
    [AutoMapTo(typeof(B_Categroy))]
    public class CreateB_CategroyInput
    {
        #region 表字段
        /// <summary>
        /// Name
        /// </summary>
        [MaxLength(500, ErrorMessage = "Name长度必须小于500")]
        [Required(ErrorMessage = "必须填写Name")]
        public string Name { get; set; }

        /// <summary>
        /// P_Id
        /// </summary>
        public Guid? P_Id { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "")]
        public decimal Price { get; set; }

        /// <summary>
        /// Unit
        /// </summary>
        [MaxLength(200, ErrorMessage = "Unit长度必须小于200")]
        [Required(ErrorMessage = "必须填写Unit")]
        public string Unit { get; set; }

        /// <summary>
        /// Tag
        /// </summary>
        [MaxLength(200, ErrorMessage = "Tag长度必须小于200")]
        public string Tag { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        [MaxLength(500, ErrorMessage = "Remark长度必须小于500")]
        public string Remark { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "")]
        public int Status { get; set; }

        /// <summary>
        /// 一级商品类别属性  来源于数据字典，  进提货、 直购、试装
        /// </summary>
        public FirestLevelCategroyProperty FirestLevelCategroyPropertyId { get; set; }


        public GetAbpFilesOutput File { get; set; } = new GetAbpFilesOutput();
        #endregion
    }
}