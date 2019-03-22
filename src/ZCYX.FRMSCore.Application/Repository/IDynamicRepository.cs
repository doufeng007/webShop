using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IDynamicRepository : IRepository
    {
        IEnumerable<dynamic> Query(string sql, object param = null);
        IEnumerable<T> Query<T>(string sql, object param = null);

        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null);

        dynamic QueryFirst(string sql, object param = null);

        Task<dynamic> QueryFirstAsync(string sql, object param = null);

        int Execute(string sql, object param = null);

        Task<int> ExecuteAsync(string sql, object param = null);
    }
}
