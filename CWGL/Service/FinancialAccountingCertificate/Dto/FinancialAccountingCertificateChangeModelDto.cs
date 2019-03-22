using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore;

namespace CWGL
{
    
    public class FinancialAccountingCertificateChangeModelDto
    {
        [LogColumn("会计科目")]
        public List<FACertificateDetailChangeDto> Details { get; set; } = new List<FACertificateDetailChangeDto>();

    }
}
