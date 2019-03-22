using Abp.AutoMapper;
using Abp.File;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace HR
{
    /// <summary>
    /// 员工档案管理
    /// </summary>
    [AutoMap(typeof(Employee))]
    public   class EmployeeDocmentListDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 员工信息
        /// </summary>
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>     
        public string Name { get; set; }
        /// <summary>
        /// 性别0：男  1：女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 员工部门
        /// </summary>
        public List<SimpleOrganizationDto> Organization { get; set; }
        /// <summary>
        /// 员工岗位
        /// </summary>
        public List<UserPostDto> Posts { get; set; }
        /// <summary>
        /// 是否临时工
        /// </summary>
        public bool? IsTemp { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 员工登记表
        /// </summary>
        public List<GetAbpFilesOutput> InfoFileList { get; set; }
        /// <summary>
        /// 毕业证书附件
        /// </summary>
        public List<GetAbpFilesOutput> EduFileList { get; set; }
        /// <summary>
        /// 面试题附件
        /// </summary>
        public List<GetAbpFilesOutput> FaceFileList { get; set; }
        /// <summary>
        /// 面试评审表
        /// </summary>
        public List<GetAbpFilesOutput> AuditFileList { get; set; }
        /// <summary>
        /// 薪水审核表
        /// </summary>
        public List<GetAbpFilesOutput> SalaryFileList { get; set; }
        /// <summary>
        /// 合同附件
        /// </summary>
        public List<GetAbpFilesOutput> ContactFileList { get; set; }

        public long? UserId { get; set; }
    }
}
