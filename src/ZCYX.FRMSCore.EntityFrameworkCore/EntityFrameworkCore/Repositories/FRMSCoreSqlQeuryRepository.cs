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

namespace ZCYX.FRMSCore.EntityFrameworkCore.Repositories
{
    public class SqlQeuryRepository : FRMSCoreSqlQeuryRepository<WorkFlow, Guid, DefaultMemberModel, Guid>, ISqlQeuryRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;

        public SqlQeuryRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
    }

    public class SubFlowActiveRepository : FRMSCoreSqlQeuryRepository<WorkFlow, Guid, SubWorkFlowExecuteResult, Guid>, ISubFlowActiveRepository
    {
        private IDbContextProvider<FRMSCoreDbContext> _dbContextProvider;

        public SubFlowActiveRepository(IDbContextProvider<FRMSCoreDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
    }


}
