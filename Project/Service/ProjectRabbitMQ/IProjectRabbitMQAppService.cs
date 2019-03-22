using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    [RemoteService(IsEnabled = false)]
    public interface IProjectRabbitMQAppService : IApplicationService
    {

        void PushProjectToXieZou(Guid projectId);

    }

}
