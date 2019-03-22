using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abp.WorkFlow.Service.Dto
{
    public class TableListDto
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
    }
    /// <summary>
    /// 数据库表中的列集合
    /// </summary>
   
    public class TableColumnDto 
    {
        public string Code { get; set; }
        public Entity.DataType DataType { get; set; }
        public int? MinLength { get; set; }

        public int? MaxLength { get; set; }

        public bool IsRequired { get; set; }
    }
    [AutoMap(typeof(WorkFlowModel))]
    public class WorkFlowModelDto {
        public Guid? Id { get; set; }
        public string Name { get; set; }

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

        public List<WorkFlowModelColumnDto> Columnes { get; set; }
    }
    [AutoMap(typeof(WorkFlowModelColumn))]
    public class WorkFlowModelColumnDto: TableColumnDto
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// 字段标题
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 字段描述
        /// </summary>
        public string Tip { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public FieldType FieldType { get; set; }
        /// <summary>
        /// 验证规则（正则）
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 列宽
        /// </summary>
        public int? Col { get; set; }
        /// <summary>
        /// 错误提示
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 外键表名
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 值域（如果是枚举类型，这些列出所有可选项）
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }

        public DefaultType DefaultType { get; set; }
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 保存或编辑视图
    /// </summary>
    [AutoMap(typeof(WorkFlowTemplate))]
    public class WorkFlowTemplateDto {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public TemplateType TemplateType { get; set; }

        public virtual WorkFlowModel WorkFlowModel { get; set; }
        public Guid WorkFlowModelId { get; set; }

        public string VueTemplate { get; set; }
    }
    /// <summary>
    /// 获取模版的请求参数
    /// </summary>
    public class GetTemplateInput {
        /// <summary>
        /// 关联的模型编码
        /// </summary>
        public string ModelCode { get; set; }
        /// <summary>
        /// 模型类别
        /// </summary>
        public TemplateType TemplateType { get; set; }
    }

    /// <summary>
    /// 已定义的模型列表
    /// </summary>
    [AutoMap(typeof(WorkFlowModel))]
    public class WorkFlowModelListDto {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
