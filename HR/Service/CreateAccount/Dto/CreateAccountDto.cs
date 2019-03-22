using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(CreateAccount))]
    public  class CreateAccountDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public long Department { get; set; }

        public string Department_Name { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public Guid Post { get; set; }

        public string Post_Name { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? JoinTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }
    }
}
