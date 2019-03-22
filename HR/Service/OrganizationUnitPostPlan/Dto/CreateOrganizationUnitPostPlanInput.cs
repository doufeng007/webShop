using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;

namespace HR
{
    [AutoMapTo(typeof(OrganizationUnitPostPlan))]
    public class CreateOrganizationUnitPostPlanInput : CreateWorkFlowInstance
    {
        #region 表字段
        /// <summary>
        /// OrganizationUnitId
        /// </summary>
        public long OrganizationUnitId { get; set; }

        /// <summary>
        /// PostId
        /// </summary>
        public Guid? PostId { get; set; }


        public string PostName { get; set; }

        /// <summary>
        /// PrepareNumber
        /// </summary>
        public int PrepareNumber { get; set; }


        /// <summary>
        /// 1新增编制---- 2编辑编制
        /// </summary>
        public int ActionType { get; set; }


        #endregion
    }



    public class UpdateOrganizationUnitPostPlanInput : CreateOrganizationUnitPostPlanInput
    {
        public Guid Id { get; set; }

        public bool IsUpdateForChange { get; set; }
    }


    public class GetOrgPostPlanInput : GetWorkFlowTaskCommentInput
    {
        public int ActionType { get; set; }
    }
}