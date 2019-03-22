﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Application
{
    public interface IUserMenuInitAppService : IApplicationService
    {

        Task<bool> GetHasComplateInit(Guid menuId);

    }
}
