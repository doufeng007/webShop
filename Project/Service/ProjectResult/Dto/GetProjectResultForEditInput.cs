using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    /// <summary>
    /// 上传评审结果的输入
    /// </summary>
    public class GetProjectResultForEditInput
    {


        public Guid ProjectBaseId { get; set; }


        /// <summary>
        /// 工程评审人员 获取 项目信息+自己的评审结果  项目负责人 获取 项目信息+每个工程评审人员的评审结果+评审事项的汇总结果+自己的汇总结果  
        ///总工复核 获取 项目项目+每个工程评审人员的评审结果+评审事项的汇总结果+项目负责人的汇总结果+自己的复核结果
        /// </summary>
        public int AuditRoleId { get; set; }


        ///// <summary>
        /////  0 表示提交评审结果  1 表示评审结果已经提交完成 查看评审结果
        ///// </summary>
        //public int ActionType { get; set; }

    }


}
