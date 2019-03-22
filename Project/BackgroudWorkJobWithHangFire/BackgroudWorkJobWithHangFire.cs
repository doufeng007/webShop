using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;

namespace Project
{
    public class BackgroudWorkJobWithHangFire : IBackgroudWorkJobWithHangFire, ITransientDependency
    {
        public void CreateJobForProjectForceSubmit(Guid projectId)
        {
            RecurringJob.AddOrUpdate<IProjectSupplementAppService>($"projectforcesubmit-{projectId}", x => x.JobForProjectForceSubmit(projectId), Cron.HourInterval(2));
        }
        /// <summary>
        /// 根据项目工时自动创建工作项到期待办提醒
        /// </summary>
        /// <param name="porjectId"></param>
        /// <param name="taskId"></param>
        public void CreatProjectProgressTask(Guid instancId, Guid taskId, int hours)
        {
            RecurringJob.AddOrUpdate<IProjectProgressComplateAppService>($"projectprocess-{taskId}", x => x.TimeOut(instancId, taskId), Cron.HourInterval(hours));
            //RecurringJob.AddOrUpdate<IProjectProgressComplateAppService>($"projectprocess-{instancId}", x => x.TimeOut(instancId, taskId), Cron.MinuteInterval(10));
        }
        public void RemoveProjectProgressTask(Guid instancId)
        {
            RecurringJob.RemoveIfExists($"projectprocess-{instancId}");
        }
        public void RemoveIfExistsJobForProjectForceSubmit(Guid projectId)
        {
            RecurringJob.RemoveIfExists($"projectforcesubmit-{projectId}");

        }


        
    }
}