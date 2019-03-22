using System;
using System.Collections.Generic;
using System.Text;
using Abp.Dependency;
using Hangfire;

namespace Train.Jobs
{
    /// <summary>
    /// 必修课程未完成扣积分
    /// </summary>
    public class TrainHangFire : ITrainHangFire, ITransientDependency
    {
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="courseId"></param>
        public void CreateJob(Guid trainId,DateTime dt)
        {
            DateTimeOffset time = new DateTimeOffset(dt, TimeZoneInfo.Local.GetUtcOffset(dt));
            BackgroundJob.Schedule<ITrainAppService>(x=>x.SendMessageForJoinUser(trainId), time);            
        }
    }
}