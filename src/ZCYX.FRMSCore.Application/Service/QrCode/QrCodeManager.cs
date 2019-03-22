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
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ZCYX.FRMSCore.Model;

namespace ZCYX.FRMSCore.Application
{
    [RemoteService(IsEnabled = false)]
    public class QrCodeManager : ApplicationService
    {
        private readonly IRepository<QrCode, Guid> _repository;

        public QrCodeManager(IRepository<QrCode, Guid> repository)
        {
            this._repository = repository;
        }
        public Guid GetCreateId(QrCodeType type)
        {
            var id = Guid.NewGuid();
            var model = new QrCode()
            {
                Id = id,
                Type = type
            };
            _repository.Insert(model);
            return id;
        }

        public void UpdateType(QrCode input)
        {
            if (!_repository.GetAll().Any(x => x.Id == input.Id))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            var model= _repository.GetAll().First(x => x.Id == input.Id);
            model.Type = input.Type;
            _repository.Update(model);
        }

        public QrCode Get(Guid id)
        {
            if (!_repository.GetAll().Any(x => x.Id == id))
            {
                throw new UserFriendlyException((int)ErrorCode.DataAccessErr, "该数据不存在。");
            }
            return _repository.GetAll().First(x => x.Id == id);
        }

    }
}
