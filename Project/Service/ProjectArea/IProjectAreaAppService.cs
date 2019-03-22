using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCYX.FRMSCore.Application.Dto;

namespace Project
{
   public interface IProjectAreaAppService: IApplicationService
    {
        PagedResultDto<ProjectAreaDto> GetList(PagedAndSortedInputDto input);

        void CreatorUpdate(ProjectAreaDto input);

        long GetAreaInProjectUserId(Guid projectid);
    }
}
