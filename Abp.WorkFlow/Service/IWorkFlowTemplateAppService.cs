using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow.Entity;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application.Dto;

namespace Abp.WorkFlow.Service
{
   public interface IWorkFlowTemplateAppService: IApplicationService
    {
        /// <summary>
        /// 获取数据库中的所有表
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetTables();
        /// <summary>
        /// 根据表名获取表的所有列
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetTableColumnes(string tableName);
        /// <summary>
        /// 保存表结构
        /// </summary>
        /// <param name="model">表结构定义</param>
        Guid CreateOrUpdateModel(WorkFlowModelDto model);
        /// <summary>
        /// 将自动生成的模版保存到数据库
        /// </summary>
        /// <param name="template"></param>
        void SaveTemplate(WorkFlowTemplateDto template);
        void SaveTemplates(List< WorkFlowTemplateDto> templates);
        /// <summary>
        /// 根据模型名和视图类型获取该视图的模版
        /// </summary>
        /// <param name="input"></param>
        WorkFlowTemplateDto GetTemplate(GetTemplateInput input);
        WorkFlowTemplateDto GetTemplateByModelId(Guid modelId, TemplateType type = TemplateType.编辑模版);
        /// <summary>
        /// 获取已定义的模型列表
        /// </summary>
        PagedResultDto<WorkFlowModelListDto> GetModels(PagedAndSortedInputDto input);
        /// <summary>
        /// 根据模型ID获取模型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WorkFlowModelDto GetModel(Guid id);
        /// <summary>
        /// 根据模型ID获取模型定义及所有关联模型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<WorkFlowModelDto> GetModelRelations(Guid id);
        /// <summary>
        /// 测试保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        dynamic TestSave(dynamic model);
        /// <summary>
        /// 获取指定模型表的select数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IEnumerable<dynamic> GetModelSelect(GetModelSelectInput input);

        /// <summary>
        /// 根据工作流id获取工作流第一步模型对应的模版
        /// </summary>
        /// <param name="flowId">工作流id</param>
        /// <param name="type">模版类型 默认0编辑模版 1详情视图</param>
        /// <returns></returns>
        WorkFlowTemplateDto GetTemplateByFlowId(Guid flowId, TemplateType type = TemplateType.编辑模版);
        /// <summary>
        /// 锁定编辑
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        bool LockEdit(Guid templateId);
        /// <summary>
        /// 强制解锁
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        bool UnLock(Guid templateId);
    }
}
