using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Threading.Tasks;


namespace Project
{
    public interface IDispatchMessageAppService : IApplicationService
    {
        Task CreateOrUpdate(DispatchPublishInput input);

        Task<DispatchPublishOutput> GetDispatchForEidt(Guid projectId);

        Task<DispatchPublishOutput> GetDispatchForView(Guid id);



        Guid CreateActive(string instanceId,long userId);
    }

}