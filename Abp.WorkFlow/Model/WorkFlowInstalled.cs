using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore.Authorization.Users;

namespace Abp.WorkFlow
{
    [Serializable]
    [AutoMapFrom(typeof(WorkFlowInstalledBase))]
    public class WorkFlowInstalled : WorkFlowInstalledBase
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionNum { get; set; }
    }


    public class WorkFlowInstalledBase
    {

        /// <summary>
        /// 流程ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 流程分类
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 流程类型：0 常规流程 1 自由流程
        /// </summary>
        public int FlowType { get; set; }

        /// <summary>
        /// 是否允许变更
        /// </summary>
        public bool IsChange { get; set; }
        public bool IsFiles { get; set; }

        /// <summary>
        /// 流程管理者
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 实例管理者
        /// </summary>
        public string InstanceManager { get; set; }



        /// <summary>
        /// 第一步ID
        /// </summary>
        private Guid _firstStepID;
        public Guid FirstStepID
        {
            get
            {
                if (_firstStepID != null)
                    return _firstStepID;
                else
                {
                    var firstId = this.Steps.Select(r => r.ID).Except(this.Lines.Select(r => r.ToID));
                    _firstStepID = firstId.FirstOrDefault();
                    return _firstStepID;
                }
            }
            set
            {
                if (_firstStepID != value)
                    _firstStepID = value;
            }
        }


        public DateTime CreationTime { get; set; }

        public long CreatorUserId { get; set; }

        /// <summary>
        /// 设计时
        /// </summary>
        public string DesignJSON { get; set; }

        /// <summary>
        /// 安装日期
        /// </summary>
        public DateTime InstallTime { get; set; }

        /// <summary>
        /// 安装人
        /// </summary>
        public string InstallUser { get; set; }

        /// <summary>
        /// 运行时JSON
        /// </summary>
        public string RunJSON { get; set; }

        /// <summary>
        /// 状态 1:设计中 2:已安装 3:已卸载 4:已删除
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否删除已完成 0不删除 1要删除
        /// </summary>
        public int RemoveCompleted { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 是否调试模式 0关闭 1开启(有调试窗口) 2开启(无调试窗口)
        /// </summary>
        public int Debug { get; set; }

        /// <summary>
        /// 调试人员
        /// </summary>
        public List<User> DebugUsers { get; set; }

        /// <summary>
        /// 数据库表以及主键信息
        /// </summary>
        public IEnumerable<WorkFlowDataBases> DataBases { get; set; }

        /// <summary>
        /// 数据库表标题字段
        /// </summary>
        public WorkFlowTitleField TitleField { get; set; }

        /// <summary>
        /// 流程步骤
        /// </summary>
        public IEnumerable<WorkFlowStep> Steps { get; set; }

        /// <summary>
        /// 流程连线
        /// </summary>
        public IEnumerable<WorkFlowLine> Lines { get; set; }


        public WorkFlowInstalledBase()
        {
            this.DebugUsers = new List<User>();
            this.DataBases = new List<WorkFlowDataBases>();
            this.TitleField = new WorkFlowTitleField();
            this.Steps = new List<WorkFlowStep>();
            this.Lines = new List<WorkFlowLine>();


        }
    }
}
