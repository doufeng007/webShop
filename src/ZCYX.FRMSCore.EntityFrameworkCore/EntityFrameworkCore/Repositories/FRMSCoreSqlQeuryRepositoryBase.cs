using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class FRMSCoreSqlQeuryRepository<TEntity, TPrimaryKey, TResultEntiy, TResultPrimaryKey> : IFRMSCoreSqlQeuryRepository<TEntity, TPrimaryKey, TResultEntiy, TResultPrimaryKey>, IRepository, ITransientDependency
        where TEntity : class, IEntity<TPrimaryKey>
        where TResultEntiy : class, IEntity<TResultPrimaryKey>
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;

        public FRMSCoreSqlQeuryRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public TResultEntiy SqlExecuteQuery(string sql)
        {
            var ret = _dbContextProvider.GetDbContext().Set<TResultEntiy>().FromSql(sql).FirstOrDefault();
            return ret;
        }

        public TResultEntiy SqlExecuteQuery(string sql, object param = null)
        {
            var ret = _dbContextProvider.GetDbContext().Set<TResultEntiy>().FromSql(sql, param).FirstOrDefault();
            return ret;
        }

        public async Task<List<TResultEntiy>> SqlExecuteQueryList(string sql, object param = null)
        {
            var ret = await _dbContextProvider.GetDbContext().Set<TResultEntiy>().FromSql(sql, param).ToListAsync();
            return ret;
        }

        public async Task<PagedResultDto<TResultEntiy>> SqlExecuteQueryPageList(SqlExecutePageListQueryInput input)
        {
            var query = _dbContextProvider.GetDbContext().Set<TResultEntiy>().FromSql(input.Sql, input.param);
            var totalCount = await query.CountAsync();
            var output = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            return new PagedResultDto<TResultEntiy>(totalCount, output);
        }

        public IQueryable<TResultEntiy> ListExecuteQuery(SqlExecutePageListQueryInput input)
        {
            var query = _dbContextProvider.GetDbContext().Set<TResultEntiy>().FromSql(input.Sql, input.param);
            return query;
        }
    }





}
