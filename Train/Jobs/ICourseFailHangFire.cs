using System;
using System.Collections.Generic;
using System.Text;

namespace Train.Jobs
{
    public interface ICourseFailHangFire
    {
        void CreateJob(Guid courseId);

        void RemoveJob(Guid courseId);
    }
}
