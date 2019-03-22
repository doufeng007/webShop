using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IQrCodeAppService : IApplicationService
    {
        Task<QrCode> Get(EntityDto<Guid> input);
    }
}