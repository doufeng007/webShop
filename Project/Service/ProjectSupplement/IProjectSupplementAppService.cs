using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectSupplementAppService : IApplicationService
    {

        //ListResultDto<ProjectSupplementListDto> GetProjectSupplements();
        Task<PagedResultDto<ProjectSupplementListDto>> GetProjectSupplements(GetProjectSupplementListInput input);

        ListResultDto<ProjectSupplementListDto> GetAllProjectSupplements(GetProjectSupplementListInput input);


        Task<bool> ProjectExitSupplements(GetProjectSupplementListInput input);

        void JobForProjectForceSubmit(Guid projectId);

        /// <summary>
        /// 强行提交后 自动提示
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task CertainSubmite(Guid projectId);


    }

}
