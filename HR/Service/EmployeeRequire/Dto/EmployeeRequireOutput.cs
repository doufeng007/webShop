using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR
{
    [AutoMap(typeof(EmployeeRequire))]
    public class EmployeeRequireOutput:InitWorkFlowOutput
    {
    }
}
