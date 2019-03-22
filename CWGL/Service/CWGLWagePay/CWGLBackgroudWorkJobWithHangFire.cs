using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Abp.Application.Services;

namespace CWGL
{
    [RemoteService(IsEnabled = false)]
    public class CWGLBackgroudWorkJobWithHangFire : ICWGLBackgroudWorkJobWithHangFire, ITransientDependency
    {
        /// <summary>
        /// 创建和修改 自动创建财务发工资todo 的后台工作
        /// </summary>
        /// <param name="projectId"></param>
        public void CreateOrUpdateJobForAutoCreateWageTodo(Guid system_WageConfigId, int day)
        {
            RecurringJob.AddOrUpdate<ICWGLWagePayAppService>($"wage-{system_WageConfigId}", x => x.AutoCreate(), Cron.Monthly(day, 0));
        }

        public void RemoveAutoCreateWageTodo(Guid system_WageConfigId)
        {
            RecurringJob.RemoveIfExists($"wage-{system_WageConfigId}");
        }
        
    }
}