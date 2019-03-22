using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IProjectAuditGroupAppService : IApplicationService
    {

        Task<PagedResultDto<ProjectAuditGroupListDto>> GetProjectAuditGroups(GetProjectAuditGroupListInput input);


        Task<GetProjectAuditGroupForEditOutput> GetProjectAuditGroupForEdit(NullableIdDto<Guid> input);


        Task CreateOrUpdateProjectAuditGroup(CreateOrUpdateProjectAuditGroupInput input);


        Task DeleteProjectAuditGroup(EntityDto<Guid> input);


        Task<Guid> CreateOrUpdate(CreateOrUpdateProjectAuditGroupInput input);

        void CopyForProjectAuditGroup(CopyForProjectAuditGroupInput input);
    }

}
