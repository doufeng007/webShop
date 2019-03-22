using Abp.AutoMapper;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 帐号创建信息
    /// </summary>
    [AutoMap(typeof(CreateAccount))]
    public class CreateAccountInput: CreateWorkFlowInstance
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public long Department { get; set; }


        /// <summary>
        /// 岗位
        /// </summary>
        public Guid Post { get; set; }

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
    }
}
