using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Abp.WorkFlow.Service.Dto
{
    [AutoMapFrom(typeof(WorkFlowButtons))]
    public class WorkFlowButtonDto : EntityDto<Guid>
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Ico { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }


    [AutoMapTo(typeof(WorkFlowButtons))]
    public class CreateWorkFlowButtonDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Ico { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

    }



    [AutoMapFrom(typeof(TestTable))]
    public class TestTableDto : EntityDto<Guid>
    {

        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public int Status { get; set; }



    }


    [AutoMapTo(typeof(TestTable))]
    public class CreateTestTableDto : CreateWorkFlowInstance
    {
        public string Name { get; set; }

    }

    public class CreateTestTableOutput: InitWorkFlowOutput
    {
       
    }


    public class CreateWorkFlowInstance
    {
        public Guid FlowId { get; set; }
        /// <summary>
        /// 关联任务编号
        /// </summary>
        public Guid? RelationTaskId { get; set; }
        public string FlowTitle { get; set; }
    }
}
