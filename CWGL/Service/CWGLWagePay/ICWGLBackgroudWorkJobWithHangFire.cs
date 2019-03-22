using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWGL
{
    [RemoteService(IsEnabled = false)]
    public interface ICWGLBackgroudWorkJobWithHangFire
    {
        void CreateOrUpdateJobForAutoCreateWageTodo(Guid system_WageConfigId, int day);


        void RemoveAutoCreateWageTodo(Guid system_WageConfigId);
    }
}
