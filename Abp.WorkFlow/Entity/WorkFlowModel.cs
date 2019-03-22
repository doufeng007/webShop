using Abp.AutoMapper;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow.Entity
{
    /// <summary>
    /// 模型定义
    /// </summary>
    [AutoMapTo(typeof(WorkFlowModel))]
    public class WorkFlowModel : Entity<Guid>
    {
        public string Name { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 标题字段
        /// </summary>
        public string TitleCode { get; set; }
        /// <summary>
        /// 数据保存接口
        /// </summary>
        public string CreateApi { get; set; }

        public string UpdateApi { get; set; }

        public string ListApi { get; set; }
        public string GetApi { get; set; }

        public string DelApi { get; set; }

        //public virtual ICollection<WorkFlowModelColumn> Columnes { get; set; }
    }

    [AutoMapTo(typeof(WorkFlowModelColumn))]
    public class WorkFlowModelColumn : Entity<Guid>
    {
        public Guid WorkFlowModelId { get; set; }
        /// <summary>
        /// 字段标题
        /// </summary>
        public string Name { get; set; }

        public string Code { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Tip { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldType FieldType { get; set; }

        public DataType DataType { get; set; }
        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }
        /// <summary>
        /// 验证规则（正则）
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 外键表名
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 外键表模型
        /// </summary>
        public Guid? RelationModelId { get; set; }
        /// <summary>
        /// 必填
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// 值域（如果是枚举类型，这些列出所有可选项）
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }
        /// <summary>
        /// 列宽
        /// </summary>
        public int? Col { get; set; }

        public DefaultType? DefaultType { get; set; }

        //public int? TenantId { get; set; }
        /// <summary>
        /// 字段排序
        /// </summary>
        public int? Sort { get; set; }
    }

    public enum DataType
    {
        nvarchar = 0,
        varchar = 1,
        inter = 2,
        bigint = 3,
        bit = 4,
        datetime = 5,
        uniqueidentifier = 6,
        money=7,
        text = 10
    }
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum FieldType
    {
        主键 = -1,
        单行文本 = 0,
        多行文本 = 1,
        数值类型 = 2,
        时间类型 = 3,
        日期类型 = 4,
        组织架构 = 5,
        数据字典 = 6,
        单选下拉 = 7,
        多选下拉 = 8,
        附件 = 9,
        坐标 = 10,
        开关控件=11,
        复选框=12,

        一对一 = 31,
        一对多 = 32,
        多对一 = 33,
        多对多 = 34,


    }
    public enum DefaultType
    {
        自定义 = 0,
        当前创建时间 = 1,
        当前登陆用户 = 2,
        当前登陆租户 = 3
    }
}
