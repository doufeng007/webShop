using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;

namespace B_H5
{
    public class BackgroudWorkJobWithHangFire : ITransientDependency
    {

        public void CreatJob()
        {
            RecurringJob.AddOrUpdate<IB_AgencySalesAppService>($"monthSaleDis", x => x.CreateSalesDiscount(), Cron.HourInterval(5));
        }
        

        public void RemoveJob(Guid instancId)
        {
            RecurringJob.RemoveIfExists($"monthSaleDis");
        }
       



    }
}