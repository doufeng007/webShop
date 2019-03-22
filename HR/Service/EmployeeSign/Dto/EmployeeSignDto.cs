using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{

    public class GetEmployeeSignOutput
    {
        public Guid? Id { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public DateTime? GoToWorkTime { get; set; }

        public DateTime? GoOfWork { get; set; }



        public string Remark { get; set; }


        public EmployeeSignType EmployeeSignType { get; set; }

    }



}
