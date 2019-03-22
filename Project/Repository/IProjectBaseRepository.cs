using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectBaseRepository : IRepository<ProjectBase, Guid>
    {
        List<ValidateForPorjectResult> GetValidateModelResult<T>(T obj);


        
    }


    
}
