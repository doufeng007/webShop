using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.WorkFlow
{
    public class WorkFlowTodoListDto
    {
        public Guid FlowID { get; set; }


        public string FlowName { get; set; }

        public Guid ID { get; set; }


        public string InstanceID { get; set; }

        public string Note { get; set; }

        public string Query { get; set; }

        public DateTime ReceiveTime { get; set; }

        public string SenderName { get; set; }

        public string ReceiveName { get; set; }


        public string StatusTitle { get; set; }


        public Guid StepID { get; set; }


        public string StepName { get; set; }

        public Guid GroupID { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public Guid GovId { get; set; }

        /// <summary>
        /// 模型id
        /// </summary>
        public Guid? WorkFlowModelId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string GovName { get; set; }

        public string Title { get; set; }

        public string OpenModel { get; set; }


        public string Width { get; set; }

        public string Height { get; set; }

        public int StopStatus { get; set; }

        public int Status { get; set; }

        public string AppraisalTypeName { get; set; }

        public string SendUnitName { get; set; }

        public DateTime SendTime { get; set; }


        /// <summary>
        /// 是否能收回任务
        /// </summary>
        public bool CanCancle { get; set; }


        /// <summary>
        /// 下一步接受者
        /// </summary>
        public string NextReciveName { get; set; }


        //public string PorjcetName { get; set; }


        //public string SingleProjectCode { get; set; }

        public WorkFlowFormView ViewInfo { get; set; }
        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsFollow { get; set; }

        public WorkFlowTodoListDto()
        {
            this.ViewInfo = new WorkFlowFormView();
        }




    }




    public class WorkFlowFormView
    {
        public string Custorm_Address { get; set; }


        public string Custorm_AddressView { get; set; }


        public string Custorm_AddressViewScriptUrl { get; set; }

        public string Custorm_AddressViewModalClass { get; set; }

        public string Custorm_AddressViewWidth { get; set; }

    }
}
