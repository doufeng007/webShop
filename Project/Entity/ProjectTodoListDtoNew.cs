using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public class ProjectTodoListDtoNew
    {
        public string ID { get; set; }
        public string FlowID { get; set; }

        public string StepID { get; set; }

        public string GroupID { get; set; }

        public string StepName { get; set; }

        public string Title { get; set; }

        public string InstanceID { get; set; }

        public string Note { get; set; }


        public string ReceiveID { get; set; }

        public string ReceiveName { get; set; }

        public DateTime ReceiveTime { get; set; }

        public string SenderName { get; set; }


        public string SenderID { get; set; }


        public DateTime SenderTime { get; set; }


        public int STATUS { get; set; }


        public string FlowName { get; set; }


        public int TaskType { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public Guid GovId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string GovName { get; set; }



        public ProjectTodoListDtoNew()
        {
        }




    }
}
