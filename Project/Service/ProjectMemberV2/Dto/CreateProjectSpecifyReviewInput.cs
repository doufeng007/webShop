//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Project
//{
//    public class CreateProjectSpecifyReviewInput
//    {


//        public Guid ProjectBaseId { get; set; }

//        public decimal? AuditAmount { get; set; }


//        public List<CreateOrUpdateProjectAuditMembersInput> Reviews { get; set; }

//        public string Remark { get; set; }


//        public CreateProjectSpecifyReviewInput()
//        {
//            this.Reviews = new List<CreateOrUpdateProjectAuditMembersInput>();
//        }

//    }

//    public class GetProjectForReviewInput
//    {
//        public Guid ProjectBaseId { get; set; }


//        /// <summary>
//        ///  1 一级复核 2 二级复核 3 三级复核
//        /// </summary>
//        public int ReviewSetup { get; set; }
//    }

//    public class GetProjectAuditMembersInput
//    {
//        public Guid ProjectId { get; set; }

//        public string AuditRoleIds { get; set; }
//    }
//}
