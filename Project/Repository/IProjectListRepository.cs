using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Application;

namespace Project
{
    public interface IProjectListRepository : IFRMSCoreSqlQeuryRepository<ProjectBase, Guid, ProjectListDto, Guid>
    {
       
    }
}
