using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OACustomer))]
    public class OACustomerInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Tel { get; set; }
        public string Phone { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public string Des { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
    }
    [AutoMap(typeof(OACustomer))]
    public class OACustomerDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Tel { get; set; }
        public string Phone { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public string Des { get; set; }

        public int Status { get; set; }

        public string AuditUser { get; set; }
    }
    [AutoMap(typeof(OACustomer))]
    public class OACustomerListDto: BusinessWorkFlowListOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string Tel { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AuditUser { get; set; }
    }
}
