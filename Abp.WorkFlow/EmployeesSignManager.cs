using Abp.Application.Services;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.SignalR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Abp.File;
using ZCYX.FRMSCore.Application;

namespace Abp.WorkFlow
{
    [RemoteService(IsEnabled = false)]
    public class EmployeesSignManager : ApplicationService
    {
        private readonly IRepository<Employees_Sign, Guid> _repository;
        private readonly IAbpFileRelationAppService _abpFileRelationAppService;

        public EmployeesSignManager(IRepository<Employees_Sign, Guid> repository, IAbpFileRelationAppService abpFileRelationAppService)
        {
            this._repository = repository;
            _abpFileRelationAppService = abpFileRelationAppService;
        }
        public Guid? GetSignFileId(long userId)
        {
            var sign = _repository.GetAll().FirstOrDefault(x => x.UserId == userId && x.Status == GW_EmployeesSignStatusEnmu.启用);
            if (sign != null)
            {
                var file = _abpFileRelationAppService.GetList(new GetAbpFilesInput()
                {
                    BusinessId = sign.Id.ToString(),
                    BusinessType = (int)AbpFileBusinessType.员工签名图片
                });
                if (file.Count() > 0)
                    return file.First().Id;
            }
            return null;
        }

    }
}
