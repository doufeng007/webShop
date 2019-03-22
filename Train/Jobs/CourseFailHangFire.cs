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
    public class CourseFailHangFire : ICourseFailHangFire, ITransientDependency
    {
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="courseId"></param>
        public void CreateJob(Guid courseId)
        {
            RecurringJob.AddOrUpdate<IUserCourseRecordAppService>($"usercoursefail-{courseId}",
                x => x.DeductionScore(courseId), Cron.HourInterval(1));
        }

        /// <summary>
        ///删除任务
        /// </summary>
        /// <param name="courseId"></param>
        public void RemoveJob(Guid courseId)
        {
            RecurringJob.RemoveIfExists($"usercoursefail-{courseId}");
        }
    }
}