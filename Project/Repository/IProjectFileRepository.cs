using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectFileRepository : IRepository<ProjectFile, Guid>
    {
        
    }


    
}
