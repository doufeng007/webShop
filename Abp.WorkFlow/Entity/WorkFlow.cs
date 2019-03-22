using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.WorkFlow
{
    [Table("WorkFlow")]
    [AutoMapTo(typeof(WorkFlow))]
    public class WorkFlow : FullAuditedEntity<Guid>
    {

        #region 表字段



        /// <summary>
        /// 流程名称
        /// </summary>
        [DisplayName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 流程分类
        /// </summary>
        [DisplayName("Type")]
        public Guid Type { get; set; }

        /// <summary>
        /// 管理人员
        /// </summary>
        [DisplayName("Manager")]
        public long Manager { get; set; }

        /// <summary>
        /// 实例管理人员
        /// </summary>
        [DisplayName("InstanceManager")]
        public long InstanceManager { get; set; }

        /// <summary>
        /// 设计时
        /// </summary>
        [DisplayName("DesignJSON")]
        public string DesignJSON { get; set; }

        /// <summary>
        /// 安装日期
        /// </summary>
        [DisplayName("InstallDate")]
        public DateTime? InstallDate { get; set; }

        /// <summary>
        /// 安装人员
        /// </summary>
        [DisplayName("InstallUserID")]
        public long InstallUserID { get; set; }

        /// <summary>
        /// 运行时
        /// </summary>
        [DisplayName("RunJSON")]
        public string RunJSON { get; set; }

        /// <summary>
        /// 状态 1:设计中 2:已安装 3:已卸载 4:已删除
        /// </summary>
        [DisplayName("Status")]
        public int Status { get; set; }


        /// <summary>
        /// 是否允许变更
        /// </summary>
        [DisplayName("Status")]
        public bool IsChange { get; set; }

        /// <summary>
        /// 是否需要资料包
        /// </summary>
        [DisplayName("IsFiles")]
        public bool IsFiles { get; set; }

        public int VersionNum { get; set; }

        #endregion


    }
}