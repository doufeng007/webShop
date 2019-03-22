using Abp.AutoMapper;
using Abp.WorkFlow;
using Abp.WorkFlow.Service.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    [AutoMap(typeof(OAUseCar))]
    public class OAUseCarInputDto: CreateWorkFlowInstance
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }

        public string AuditUser { get; set; }

        public string Reason { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Person { get; set; }

        public string Destination { get; set; }

        public decimal? Mileage { get; set; }
        public string AuditUserText { get; set; }


        public string ApplyUserText { get; set; }

        public DateTime CreationTime { get; set; }
    }
    [AutoMap(typeof(OAUseCar))]
    public class OAUseCarDto: WorkFlowTaskCommentResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }

        public string AuditUser { get; set; }

        public string Reason { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Person { get; set; }

        public string Destination { get; set; }

        public decimal? Mileage { get; set; }
        public string AuditUserText { get; set; }


        public string ApplyUserText { get; set; }

        public DateTime CreationTime { get; set; }
    }

    [AutoMap(typeof(OAUseCar))]
    public class OAUseCarListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }
        public string StatusTitle { get; set; }
        public string AuditUser { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Person { get; set; }

        public decimal? Mileage { get; set; }
        public string AuditUserText { get; set; }


        public string ApplyUserText { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
