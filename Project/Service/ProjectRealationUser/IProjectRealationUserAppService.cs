using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project
{
    public interface IProjectRealationUserAppService: IApplicationService
    {
        void Create(ProjectRelationUserCreate input);
        string GetProjectRealationMember(ProjectRelationUserCreate input);
    }
}
