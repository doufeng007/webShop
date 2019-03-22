using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class WorkFlowTaskRepository : FRMSCoreRepositoryBase<WorkFlowTask, Guid>, IWorkFlowTaskRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;

        public WorkFlowTaskRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public int CompletaWorkFlowInstanceExecuteSql(string sql)
        {
            var ret = _dbContextProvider.GetDbContext().Database.ExecuteSqlCommand(sql);
            return ret;
        }

        public DefaultMemberModel GetDefaultMemberExecuteQuery(string sql)
        {
            var ret = _dbContextProvider.GetDbContext().Set<DefaultMemberModel>().FromSql(sql).FirstOrDefault();
            return ret;
        }

        public SqlConditionResultModel GetSqlConditionResult(string sql)
        {
            var ret = _dbContextProvider.GetDbContext().Set<SqlConditionResultModel>().FromSql(sql).FirstOrDefault();
            return ret;
        }

        public override WorkFlowTask Insert(WorkFlowTask entity)
        {
            GetTodoType(entity);
            return base.Insert(entity);
        }

        public override Guid InsertAndGetId(WorkFlowTask entity)
        {
            GetTodoType(entity);
            return base.InsertAndGetId(entity);
        }

        public override Task<Guid> InsertAndGetIdAsync(WorkFlowTask entity)
        {
            GetTodoType(entity);
            return base.InsertAndGetIdAsync(entity);
        }

        public override Task<WorkFlowTask> InsertAsync(WorkFlowTask entity)
        {
            GetTodoType(entity);
            return base.InsertAsync(entity);
        }

        private void GetTodoType(WorkFlowTask entity)
        {
            var workFlowCacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<WorkFlowCacheManager>();
            var instalwf = workFlowCacheManager.GetWorkFlowModelFromCache(entity.FlowID);
            var step = instalwf.Steps.FirstOrDefault(r => r.ID == entity.StepID);
            entity.TodoType = step.TodoType;
        }

    }


}
