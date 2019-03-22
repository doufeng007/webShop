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
    public interface IProjcetAuditResultCheckRoleAppService : IApplicationService
    {
        Task<PagedResultDto<ProjcetAuditResultCheckRoleDto>> GetList(PagedAndSortedInputDto input);

        Task CreatorUpdate(CreateorUpdateCheckRoleInput input);

        Task<ProjcetAuditResultCheckRoleDto> GetAsync(EntityDto<Guid> input);
    }
}
