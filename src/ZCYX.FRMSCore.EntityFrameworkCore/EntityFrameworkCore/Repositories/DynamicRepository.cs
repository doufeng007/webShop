using Abp.WorkFlow.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.WorkFlow.Service.Dto;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Abp.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application;

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class DynamicRepository : IDynamicRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;

        public DynamicRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        DbTransaction Transaction
        {
            get
            {
                return _dbContextProvider.GetDbContext().Database.CurrentTransaction?.GetDbTransaction();
            }
        }
        DbConnection Connection
        {
            get { return _dbContextProvider.GetDbContext().Database.GetDbConnection(); }
        }

        public int Execute(string sql, object param = null)
        {
            return Connection.Execute(sql, param, Transaction);
        }

        public Task<int> ExecuteAsync(string sql, object param = null)
        {
            return Connection.ExecuteAsync(sql, param, Transaction);
        }

        public IEnumerable<dynamic> Query(string sql, object param = null)
        {
            return Connection.Query(sql, param, Transaction);
        }
        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return Connection.Query<T>(sql, param, Transaction);
        }
        public Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null)
        {
            return Connection.QueryAsync(sql, param, Transaction);
        }
        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            return Connection.QueryAsync<T>(sql, param, Transaction);
        }
        public dynamic QueryFirst(string sql, object param = null)
        {
            return Connection.QueryFirst(sql, param, Transaction);
        }

        public Task<dynamic> QueryFirstAsync(string sql, object param = null)
        {
            return Connection.QueryFirstAsync(sql, param, Transaction);
        }

       
        
    }
}
