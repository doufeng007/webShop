using Abp.WorkFlow.Entity;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class GetWorkFlowUrlParameterOutput
    {

        public Guid FlowId { get; set; }

        public Guid StepId { get; set; }
        public string StepName { get; set; }
        /// <summary>
        /// 流程分类
        /// </summary>
        public string FlowerType { get; set; }

        //public bool IsChange { get; set; }

        public bool ChangeButtonShow { get; set; }


        public bool ChangeLogShow { get; set; }
        public bool IsFiles { get; set; }


        //public bool StepIsCanChange { get; set; }

        public bool IsFirstStepID { get; set; } = false;

        public Guid ModelId { get; set; }

        public TemplateType TemplateType { get; set; }

        /// <summary>
        /// 意见显示 0不显示 1显示
        /// </summary>
        public int OpinionDisplay { get; set; }


        /// <summary>
        /// 是否可以删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 审签类型 0无签批意见栏 1有签批意见(无须签章) 2有签批意见(须签章)
        /// </summary>
        public int SignatureType { get; set; }

        /// <summary>
        /// 当前步骤意见显示的标题
        /// </summary>
        public string SugguestionTitle { get; set; }

        /// <summary>
        /// 流程按钮
        /// </summary>
        public List<WorkFlowButton> Buttons { get; set; }

        /// <summary>
        /// 字段状态
        /// </summary>
        public List<FieldStatus> FieldStatus { get; set; }


        /// <summary>
        /// 模型数据
        /// </summary>
        public WorkFlowModelDto ModelData { get; set; }


        public int TaskType { get; set; }
        /// <summary>
        /// 待办状态
        /// </summary>
        public int Status { get; set; }

    }
}
