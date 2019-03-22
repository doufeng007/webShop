//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Castle.Components.DictionaryAdapter;

//namespace Project
//{
//    public class UpdateProjectControlAuditResultInput
//    {
//        public Guid ProjectId { get; set; }
//        public int UserAuditRoleId { get; set; }

//        public long UserId { get; set; }

//        public List<UpdateProjectControlAuditResultInputItems> ControlRsults { get; set; }

//        public UpdateProjectControlAuditResultInput()
//        {
//            ControlRsults = new EditableList<UpdateProjectControlAuditResultInputItems>();
//        }
//    }


//    public class UpdateProjectControlAuditResultInputItems
//    {
//        public Guid? ControlResultId { get; set; }
//        public Guid ControlId { get; set; }

//        public string ValidationMoney { get; set; }
//    }

//    public class UpdateProjectControlAuditResultOutput : CreateOrUpdateProjectBudgetControlInput
//    {
//        public Guid? ControlResultId { get; set; }

//        public String ValidationMoneyResult { get; set; }
//    }


//    public class GetProjectControlAuditResultInput
//    {
//        public Guid ProjectId { get; set; }

//        public bool IsSelf { get; set; }


//        public int UserAuditRoleId { get; set; }
//    }

//    public class ProjectControlAuditResultOutput
//    {

//        public Guid ControlId { get; set; }

//        public Guid? ControlResultId { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Code { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string CodeName { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string ApprovalMoney { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string SendMoney { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string ValidationMoney { get; set; }
//    }
//}





