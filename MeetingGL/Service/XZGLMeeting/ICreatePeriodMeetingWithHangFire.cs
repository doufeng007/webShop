using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingGL
{
    public interface ICreatePeriodMeetingWithHangFire : IApplicationService
    {
        void CreateOrUpdateJobForeCreatePeriodMeeting(MeetingPeriodRule model, Guid flowId);

        MeetingPeriodRule MakePeriodMeetingCronAndNextTime(MeetingPeriodRule roleModel, Guid flowId);


        DateTime GetNextOccurrence(DateTime time, string cronExpression);

        void JobForCycleForDay(Guid flowId);
        void Cancle(Guid id);
    }
}
