using Abp.AutoMapper;
using Abp.File;
using Abp.WorkFlow.Service.Dto;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using HR;

namespace Train
{
    [AutoMapTo(typeof(Lecturer))]
    public class CreateLecturerInput 
    {
        #region 表字段
        /// <summary>
        /// 讲师姓名
        /// </summary>
        [MaxLength(20,ErrorMessage = "讲师姓名长度必须小于20")]
        [Required(ErrorMessage = "必须填写讲师姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 课时费
        /// </summary>
        [Range(0, double.MaxValue,ErrorMessage="")]
        public decimal TeachSubsidy { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [MaxLength(11,ErrorMessage = "电话长度必须小于11")]
        [Required(ErrorMessage = "必须填写电话")]
        public string Tel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(50,ErrorMessage = "邮箱长度必须小于50")]
        [Required(ErrorMessage = "必须填写邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [MaxLength(24,ErrorMessage = "银行卡号长度必须小于24")]
        [Required(ErrorMessage = "必须填写银行卡号")]
        public string BankId { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [MaxLength(50,ErrorMessage = "银行长度必须小于50")]
        [Required(ErrorMessage = "必须填写银行")]
        public string Bank { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [MaxLength(50,ErrorMessage = "开户行长度必须小于50")]
        [Required(ErrorMessage = "必须填写开户行")]
        public string OpenBank { get; set; }

        /// <summary>
        /// 讲师简介
        /// </summary>
        [MaxLength(500,ErrorMessage = "讲师简介长度必须小于500")]
        public string Introduction { get; set; }


        public List<GetAbpFilesOutput> FileList { get; set; } = new List<GetAbpFilesOutput>();

        #endregion
    }
}