using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Castle.Components.DictionaryAdapter;

namespace Supply
{
    [AutoMapTo(typeof(SupplySupplier))]
    public class CreateSupplySupplierInput 
    {
        #region 表字段
        /// <summary>
        /// 供应商类型
        /// </summary>
        [Required(ErrorMessage = "请输入供应商类型。")]
        [MaxLength(1, ErrorMessage = "供应商类型最大为1个字符。")]
        public string Type { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Required(ErrorMessage = "请输入供应商名称。")]
        [MaxLength(100, ErrorMessage = "供应商名称最大为100个字符。")]
        public string Name { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [MaxLength(500)]
        public string MainBusiness { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [MaxLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 法人
        /// </summary>
        [MaxLength(32)]
        public string LegalPerson { get; set; }

        /// <summary>
        /// 法人联系电话
        /// </summary>
        [MaxLength(32)]
        public string LegalPersonTel { get; set; }

        /// <summary>
        /// 销售联系人
        /// </summary>
        public string SalesContact { get; set; }

        /// <summary>
        /// 销售联系人电话
        /// </summary>
        [MaxLength(32)]
        public string SalesContactTel { get; set; }

        /// <summary>
        /// 评价备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }


        /// <summary>
        /// 信箱
        /// </summary>
        [MaxLength(100)]
        public string Email { get; set; }

        public List<GetAbpFilesOutput> FileList { get; set; }


        public CreateSupplySupplierInput()
        {
            this.FileList = new EditableList<GetAbpFilesOutput>();
        }

        #endregion
    }
}