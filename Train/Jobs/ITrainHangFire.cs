using System;
using System.Collections.Generic;
using System.Text;

namespace Train.Jobs
{
    public interface ITrainHangFire
    {
        void CreateJob(Guid trainId,DateTime dt);
    }
}
