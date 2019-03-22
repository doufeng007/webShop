using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public interface IArchivesManagerAppService : IApplicationService
    {

        //ListResultDto<ArchivesManagerListDto> GetArchivesManagers();
        Task<PagedResultDto<ArchivesManagerListOutputDto>> GetArchivesManagers(GetArchivesManagerListInput input);



        Task<GetArchivesManagerForEditOutput> GetArchivesManagerForEdit(NullableIdDto<Guid> input);


        Task CreateOrUpdateArchivesManager(CreateOrUpdateArchivesManagerInput input);


        Task<Guid> CreateArchivesManager(CreateOrUpdateArchivesManagerInput input);


        Task DeleteArchivesManager(EntityDto<Guid> input);


        Task<GetArchivesManagerForEditOutput> GetArchivesForWFProject(Guid projectId);


        Task<GetArchivesManagerForEditOutput> GetArchivesForWFProjectWithId(string instanceId);



        Guid CreateArchivesManagerActive(string instaceId);


    }

}
