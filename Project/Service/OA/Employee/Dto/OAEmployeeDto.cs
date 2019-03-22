using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAEmployee))]
    public class OAEmployeeInputDto : CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeftDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HomeTown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMarrayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BankCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarningPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarningPhone { get; set; }

        public DateTime? Birthday { get; set; }

        public string Name { get; set; }

        public int? Status { get; set; }

        public string AuditUser { get; set; }

    }
    [AutoMap(typeof(OAEmployee))]
    public class OAEmployeeDto: WorkFlowTaskCommentResult
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }
        public string UserId_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeftDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HomeTown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMarrayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BankCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarningPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarningPhone { get; set; }

        public DateTime? Birthday { get; set; }

        public string Name { get; set; }


        public string AuditUser { get; set; }

    }
    [AutoMap(typeof(OAEmployee))]
    public class OAEmployeeListDto : BusinessWorkFlowListOutput
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long UserId { get; set; }
        public string UserId_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WorkNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Work { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeftDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HomeTown { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMarrayed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BankCard { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarningPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WarningPhone { get; set; }

        public DateTime? Birthday { get; set; }

        public string Name { get; set; }


        public string AuditUser { get; set; }

    }

}
