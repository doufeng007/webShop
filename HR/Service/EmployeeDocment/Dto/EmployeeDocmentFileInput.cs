using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    /// <summary>
    /// 上传附件
    /// </summary>
    public class EmployeeDocmentFileInput
    {
        /// <summary>
        /// 文件信息
        /// </summary>
         public List<GetAbpFilesOutput> FileList { get; set; }
        /// <summary>
        /// 文件类型( 面试评审表=2001,
        ///员工合同=2002,
        ///员工毕业证书=2003,
        ///员工面试题=2004,
        ///员工信息登记表=2005,
        ///员工薪水核定表=2006)
        /// </summary>
        public AbpFileBusinessType Type { get; set; }
        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BusinessId { get; set; }
    }
}
