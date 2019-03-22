using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskGL.Enum;
using ZCYX.FRMSCore.Application.Dto;

namespace TaskGL.Service.TaskManagementStatistics.Dto
{
    public class TaskUserStatisticsRequest: PagedAndSortedInputDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TaskStatisticsDropTypeEnum? SearchCode { get; set; }
        public string SearchId { get; set; }
        public List<long> OrgUser { get; set; }
    }
}
