using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{

    public class EmployeeGoOutDto
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public DateTime GoOutTime { get; set; }

        public int GoOutHour { get; set; }

        public DateTime BackTime { get; set; }


        public string OutTele { get; set; }

        public string Reason { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }

        

    }

    public class GetEmployeeGoDtoInput : GetWorkFlowTaskCommentInput
    {
        public Guid Id { get; set; }
    }

    public class GetEmployeeGoDtoOutput : WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public DateTime GoOutTime { get; set; }

        public int GoOutHour { get; set; }

        public DateTime BackTime { get; set; }


        public string OutTele { get; set; }

        public string Reason { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }

        public string Remark { get; set; }
    }


    public class EmployeeGoOutListDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime GoOutTime { get; set; }

        public int GoOutHour { get; set; }

        public DateTime BackTime { get; set; }

        public int Status { get; set; }

        public string StatusTitle { get; set; }

        public long? CreatorUserId { get; set; }

    }


}
