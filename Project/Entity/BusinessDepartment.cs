﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project
{
    [Table("BusinessDepartment")]
    public class BusinessDepartment : IEntity<int>
    {

        public bool IsTransient()
        {
            return false;
        }

        [Column("Id")]
        public int Id { get; set; }

        /// <summary>
        /// 股室名称
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 股室代码
        /// </summary>
        [DisplayName("Code")]
        public string Code { get; set; }

        /// <summary>
        /// 股室办公地址
        /// </summary>
        [DisplayName("Address")]
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName("Email")]
        public string Email { get; set; }

        /// <summary>
        /// 股室联系人
        /// </summary>
        [DisplayName("ContactUser")]
        public string ContactUser { get; set; }

        /// <summary>
        /// 股室联系人电话
        /// </summary>
        [DisplayName("ContactTel")]
        public string ContactTel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("CreationTime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 座机
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


    }
}