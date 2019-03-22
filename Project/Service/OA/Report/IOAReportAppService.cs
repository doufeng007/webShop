using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.WorkFlow;
using Project.Service.OA.Report.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.OA.Report
{
    /// <summary>
    /// 工作汇报
    /// </summary>
    public interface IOAReportAppService : IApplicationService
    {
        InitWorkFlowOutput Create(OAReportInputDto input);

        void Update(OAReportInputDto input);
        Task<PagedResultDto<OAReportListDto>> GetAll(SearchReportInput input);

        OAReportDto Get(GetWorkFlowTaskCommentInput input);

        string UpdateState(Guid input);
    }
}
