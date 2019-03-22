using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IFRMSCoreSqlQeuryRepository<TEntity, TPrimaryKey, TResultEntiy, TResultPrimaryKey> : IRepository, ITransientDependency
      where TEntity : class, IEntity<TPrimaryKey>
      where TResultEntiy : class, IEntity<TResultPrimaryKey>

    {
        TResultEntiy SqlExecuteQuery(string sql);

        TResultEntiy SqlExecuteQuery(string sql, object param = null);


        Task<PagedResultDto<TResultEntiy>> SqlExecuteQueryPageList(SqlExecutePageListQueryInput input);


        IQueryable<TResultEntiy> ListExecuteQuery(SqlExecutePageListQueryInput input);
    }

    public class SqlExecutePageListQueryInput : ZCYX.FRMSCore.Application.Dto.PagedAndSortedInputDto
    {

        public string Sql { get; set; }


        public object[] param { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = " CreationTime desc";
            }
        }
    }
}
