using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace Abp.WorkFlow
{
    public interface IWorkFlowTaskRepository : IRepository<WorkFlowTask, Guid>
    {
        int CompletaWorkFlowInstanceExecuteSql(string sql);


        DefaultMemberModel GetDefaultMemberExecuteQuery(string sql);


        SqlConditionResultModel GetSqlConditionResult(string sql);
    }


    

    public interface ISqlQeuryRepository : IFRMSCoreSqlQeuryRepository<WorkFlow, Guid, DefaultMemberModel, Guid>
    {

    }

    public interface ISubFlowActiveRepository : IFRMSCoreSqlQeuryRepository<WorkFlow, Guid, SubWorkFlowExecuteResult, Guid>
    {
    }


    
}
