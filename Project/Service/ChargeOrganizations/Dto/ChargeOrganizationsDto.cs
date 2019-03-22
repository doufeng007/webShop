using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.ComponentModel;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
    public class ChargeOrganizationsDto
    {
        public int? Id { get; set; }

        /// <summary>
        /// 主管部门名字
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DisplayName("Address")]
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName("Email")]
        public string Email { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DisplayName("ContactUser")]
        public string ContactUser { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DisplayName("ContactTel")]
        public string ContactTel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("CreationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 部门座机
        /// </summary>
        [DisplayName("Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        [DisplayName("InputCode1")]
        public string InputCode1 { get; set; }

        /// <summary>
        /// 全拼
        /// </summary>
        [DisplayName("InputCode2")]
        public string InputCode2 { get; set; }

        /// <summary>
        /// 首字母
        /// </summary>
        [DisplayName("FirstLetter")]
        public string FirstLetter { get; set; }
    }

    [AutoMapFrom(typeof(ChargeOrganizations))]
    public class GetChargeOrganizationsForEditOutput : ChargeOrganizationsDto
    {
    }

    [AutoMapFrom(typeof(ChargeOrganizations))]
    public class ChargeOrganizationsList : ChargeOrganizationsDto
    {
    }

    public class GetChargeOrganizationsListInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public bool IncludeDetele { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }
    }
}