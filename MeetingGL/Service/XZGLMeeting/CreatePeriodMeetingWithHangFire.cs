using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Abp.Domain.Repositories;
using Abp;
using Microsoft.EntityFrameworkCore;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore;
using Dapper;
using Abp.UI;
using ZCYX.FRMSCore.Model;
using Cronos;

namespace MeetingGL
{
    public class CreatePeriodMeetingWithHangFire : FRMSCoreAppServiceBase, ICreatePeriodMeetingWithHangFire
    {
        public void CreateOrUpdateJobForeCreatePeriodMeeting(MeetingPeriodRule roleModel, Guid flowId)
        {

            var roleActiveTime = new DateTime(roleModel.ActiveStartTime.Year, roleModel.ActiveStartTime.Month, roleModel.ActiveStartTime.Day, roleModel.StartTime.Hour, roleModel.StartTime.Minute, 0);
            var needChangeCreateAt = false;
            var cornStr = "";
            if (roleModel.PeriodType == PeriodType.按天)
            {
                needChangeCreateAt = true;
                if (roleModel.PreTimeType == PreTimeType.分钟)
                {
                    roleActiveTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                }
                else if (roleModel.PreTimeType == PreTimeType.小时)
                {
                    roleActiveTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                }
                else
                {
                    roleActiveTime = roleActiveTime.AddDays(0 - roleModel.PreTimeNum);
                }
                //var mprRepository = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<IRepository<MeetingPeriodRule, Guid>>();
                //var roleModel = mprRepository.Get(roleModel.Id);
                if (roleActiveTime.Year == DateTime.Now.Year && roleActiveTime.Month == DateTime.Now.Month && roleActiveTime.Day == DateTime.Now.Day)
                {
                    if (DateTime.Now < roleActiveTime)
                        roleModel.NextCreateTime = roleActiveTime;
                    else
                        roleModel.NextCreateTime = roleActiveTime.AddDays(roleModel.PeriodNumber1);
                }
                else
                    roleModel.NextCreateTime = roleActiveTime;
                return;
            }
            else if (roleModel.PeriodType == PeriodType.按周)
            {

                var createMeetingTime = DateTime.Now;
                var needChangeWeek = false;
                if (roleModel.PreTimeType == PreTimeType.分钟)
                {
                    createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                    if (createMeetingTime.Day != roleActiveTime.Day)
                        cornStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 - 1), createMeetingTime.Hour, createMeetingTime.Minute);
                    else
                    {
                        if (roleModel.PeriodNumber1 == 7)
                            cornStr = Cron.Weekly(DayOfWeek.Sunday, createMeetingTime.Hour, createMeetingTime.Minute);
                        else
                            cornStr = Cron.Weekly((DayOfWeek)roleModel.PeriodNumber1, createMeetingTime.Hour, createMeetingTime.Minute);
                    }
                }
                else if (roleModel.PreTimeType == PreTimeType.小时)
                {
                    createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                    if (createMeetingTime.Day != roleActiveTime.Day)
                        cornStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 - 1), createMeetingTime.Hour, createMeetingTime.Minute);
                    else
                    {
                        if (roleModel.PeriodNumber1 == 7)
                            cornStr = Cron.Weekly(DayOfWeek.Sunday, createMeetingTime.Hour, createMeetingTime.Minute);
                        else
                            cornStr = Cron.Weekly((DayOfWeek)roleModel.PeriodNumber1, createMeetingTime.Hour, createMeetingTime.Minute);
                    }
                }
                else
                {

                    roleModel.PreTimeNum = roleModel.PreTimeNum % 7;
                    if (roleModel.PreTimeNum <= roleModel.PeriodNumber1)
                        cornStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 - roleModel.PreTimeNum), createMeetingTime.Hour, createMeetingTime.Minute);
                    else
                        cornStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 + (7 - roleModel.PreTimeNum)), createMeetingTime.Hour, createMeetingTime.Minute);
                }

            }
            else if (roleModel.PeriodType == PeriodType.按月)
            {
                var createMeetingTime = DateTime.Now;
                if (roleModel.PeriodNumber1 == 1) //每月几号
                {
                    if (roleModel.PeriodNumber2 < 1 || roleModel.PeriodNumber2 > 28)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "每月只能选择1-28号");
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {

                            if (roleModel.PeriodNumber2 == 1)
                            {
                                cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                            }
                            else
                            {
                                cornStr = Cron.Monthly(roleModel.PeriodNumber2 - 1, createMeetingTime.Hour, createMeetingTime.Minute);
                            }

                        }
                        else
                        {
                            cornStr = Cron.Monthly(roleModel.PeriodNumber2, createMeetingTime.Hour, createMeetingTime.Minute);
                        }
                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                            }
                            else
                            {
                                cornStr = Cron.Monthly(roleModel.PeriodNumber2 - 1, createMeetingTime.Hour, createMeetingTime.Minute);
                            }
                        }
                        else
                            cornStr = Cron.Monthly(roleModel.PeriodNumber2, createMeetingTime.Hour, createMeetingTime.Minute);
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        if ((roleModel.PreTimeNum - roleModel.PeriodNumber2) > 0)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前天数不能比指定天数大");
                        if (roleModel.PeriodNumber2 == roleModel.PreTimeNum)
                            cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L * ? ";
                        else
                            cornStr = Cron.Monthly(roleModel.PeriodNumber2 - roleModel.PreTimeNum, createMeetingTime.Hour, createMeetingTime.Minute);

                    }

                }
                else if (roleModel.PeriodNumber1 == 2) //每月最后1天 暂不支持每月最后第几天
                {

                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            //   cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  {roleModel.PeriodNumber2 + 1}L * ? ";
                            throw new UserFriendlyException("提前时间不能破坏每月最后一天");

                        }
                        else
                        {
                            cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                        }
                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            //cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber2 + 1}L * ? ";
                            throw new UserFriendlyException("提前时间不能破坏每月最后一天");

                        }
                        else
                            cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        //if (roleModel.PreTimeNum + roleModel.PeriodNumber2 > 28)
                        //    throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"每月最后第{roleModel.PeriodNumber1}天开会， 不能提前{roleModel.PreTimeNum}天创建会议！");
                        //cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber2 + roleModel.PreTimeNum}L * ? ";
                        throw new UserFriendlyException("暂不实现");
                    }
                }

            }
            else if (roleModel.PeriodType == PeriodType.按年)
            {
                var createMeetingTime = DateTime.Now;
                if (roleModel.PeriodNumber1 == 1) //每月几号
                {
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} 31 12 ? ";
                                }
                                else
                                {
                                    cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }
                            else
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L {roleModel.PeriodNumber2 - 1} ? ";
                                }
                                else
                                {
                                    cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }



                        }
                        else
                        {
                            cornStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber3} {roleModel.PeriodNumber2} ? ";
                        }
                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L 12 ? ";
                                }
                                else
                                {
                                    cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }
                            else
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L {roleModel.PeriodNumber2 - 1} ? ";
                                }
                                else
                                {
                                    cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }
                        }
                        else
                            cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3} {roleModel.PeriodNumber2} ? ";
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        if (roleModel.PeriodNumber3 == roleModel.PreTimeNum)
                        {
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L 12 ? ";
                            }
                            else
                                cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L {roleModel.PeriodNumber2 - 1} ? ";
                        }
                        else if (roleModel.PeriodNumber3 > roleModel.PreTimeNum)
                            cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3 - roleModel.PreTimeNum} {roleModel.PeriodNumber2} ? ";
                        else
                            throw new UserFriendlyException("暂不支持提前天数大约选择日期数");
                    }

                }
                else if (roleModel.PeriodNumber1 == 2) //每月最1天
                {
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            throw new UserFriendlyException("提前时间不能破坏每月最后一天");
                        }
                        else
                        {
                            cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L {roleModel.PeriodNumber2} ? ";
                        }
                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            throw new UserFriendlyException("提前时间不能破坏每月最后一天");

                        }
                        else
                            cornStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L {roleModel.PeriodNumber2} ? ";
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        throw new UserFriendlyException("暂不实现");
                    }
                }
            }


            RecurringJob.AddOrUpdate<IXZGLMeetingAppService>($"periodMeeting-{roleModel.MeetingId}", x => x.CreatePeriodSelf(roleModel.MeetingId, flowId), cornStr, System.TimeZoneInfo.Local);
        }


        public MeetingPeriodRule MakePeriodMeetingCronAndNextTime(MeetingPeriodRule roleModel, Guid flowId)
        {
            var hasAdvanceLessOneDay = false;
            var roleActiveTime = new DateTime(roleModel.ActiveStartTime.Year, roleModel.ActiveStartTime.Month, roleModel.ActiveStartTime.Day, roleModel.StartTime.Hour, roleModel.StartTime.Minute, 0);
            var cronStr = "";
            var createMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Utc);
            if (roleModel.PeriodType == PeriodType.按天)
            {
                if (roleModel.PreTimeType == PreTimeType.分钟)
                {
                    createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                    if (createMeetingTime.Day != roleActiveTime.Day)
                        hasAdvanceLessOneDay = true;
                }
                else if (roleModel.PreTimeType == PreTimeType.小时)
                {
                    createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                    if (createMeetingTime.Day != roleActiveTime.Day)
                        hasAdvanceLessOneDay = true;
                }
                else
                {
                    createMeetingTime = roleActiveTime.AddDays(0 - roleModel.PreTimeNum);
                    hasAdvanceLessOneDay = true;
                }
                roleModel.NextCreateTime = createMeetingTime;
                roleModel.HasAdvanceLessOneDay = hasAdvanceLessOneDay;
                return roleModel;

            }
            else if (roleModel.PeriodType == PeriodType.按周)
            {
                if (roleModel.PreTimeType == PreTimeType.分钟)
                {
                    createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                    if (createMeetingTime.Day != roleActiveTime.Day)
                    {
                        cronStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 - 1), createMeetingTime.Hour, createMeetingTime.Minute);
                        hasAdvanceLessOneDay = true;
                    }
                    else
                    {
                        if (roleModel.PeriodNumber1 == 7)
                            cronStr = Cron.Weekly(DayOfWeek.Sunday, createMeetingTime.Hour, createMeetingTime.Minute);
                        else
                            cronStr = Cron.Weekly((DayOfWeek)roleModel.PeriodNumber1, createMeetingTime.Hour, createMeetingTime.Minute);
                    }
                }
                else if (roleModel.PreTimeType == PreTimeType.小时)
                {
                    createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                    if (createMeetingTime.Day != roleActiveTime.Day)
                    {
                        cronStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 - 1), createMeetingTime.Hour, createMeetingTime.Minute);
                        hasAdvanceLessOneDay = true;
                    }
                    else
                    {
                        if (roleModel.PeriodNumber1 == 7)
                            cronStr = Cron.Weekly(DayOfWeek.Sunday, createMeetingTime.Hour, createMeetingTime.Minute);
                        else
                            cronStr = Cron.Weekly((DayOfWeek)roleModel.PeriodNumber1, createMeetingTime.Hour, createMeetingTime.Minute);
                    }
                }
                else
                {
                    hasAdvanceLessOneDay = true;
                    roleModel.PreTimeNum = roleModel.PreTimeNum % 7;
                    if (roleModel.PreTimeNum <= roleModel.PeriodNumber1)
                        cronStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 - roleModel.PreTimeNum), createMeetingTime.Hour, createMeetingTime.Minute);
                    else
                        cronStr = Cron.Weekly((DayOfWeek)(roleModel.PeriodNumber1 + (7 - roleModel.PreTimeNum)), createMeetingTime.Hour, createMeetingTime.Minute);
                }

            }
            else if (roleModel.PeriodType == PeriodType.按月)
            {
                if (roleModel.PeriodNumber1 == 1) //每月几号
                {
                    if (roleModel.PeriodNumber2 < 1 || roleModel.PeriodNumber2 > 28)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "每月只能选择1-28号");
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        if (roleModel.PreTimeNum > 1440)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前分钟数不能超过一天");
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            if (roleModel.PeriodNumber2 == 1)
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ?";
                            else
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber2 - 1} * ? ";
                        }
                        else
                            cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber2} * ? ";
                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        if (roleModel.PreTimeNum > 24)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前小时数不能超过24小时");
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            if (roleModel.PeriodNumber2 == 1)
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                            else
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber2 - 1} * ? ";
                        }
                        else
                            cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber2} * ? ";
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        hasAdvanceLessOneDay = true;
                        if (roleModel.PreTimeNum > 30)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前天数不能大于30");
                        if (roleModel.PreTimeNum > roleModel.PeriodNumber2)
                            cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L-{roleModel.PreTimeNum - roleModel.PeriodNumber2} * ? ";
                        else if (roleModel.PeriodNumber2 == roleModel.PreTimeNum)
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L * ? ";
                        else
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber2 - roleModel.PreTimeNum} * ? ";
                    }

                }
                else if (roleModel.PeriodNumber1 == 2) //每月最后几天 暂不支持每月最后第几天
                {
                    if (roleModel.PeriodNumber2 > 5)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "最大只支持最后5天");
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        if (roleModel.PreTimeNum > 1440)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前分钟数不能超过一天");
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L-{roleModel.PeriodNumber2} * ? ";
                        }
                        else
                        {
                            if (roleModel.PeriodNumber2 == 1)
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                            else
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L-{roleModel.PeriodNumber2 - 1} * ? ";
                        }

                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        if (roleModel.PreTimeNum > 24)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前小时数不能超过24小时");
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L-{roleModel.PeriodNumber2} * ? ";
                        }
                        else
                        {
                            if (roleModel.PeriodNumber2 == 1)
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L * ? ";
                            else
                                cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L-{roleModel.PeriodNumber2 - 1} * ? ";
                        }
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        hasAdvanceLessOneDay = true;
                        if (roleModel.PreTimeNum + roleModel.PeriodNumber2 > 30)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"每月最后第{roleModel.PeriodNumber1}天开会， 不能提前{roleModel.PreTimeNum}天创建会议！");
                        cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L-{roleModel.PeriodNumber2 + roleModel.PreTimeNum - 1} * ? ";
                    }
                }

            }
            else if (roleModel.PeriodType == PeriodType.按年)
            {
                if (roleModel.PeriodNumber1 == 1) //每月几号
                {
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        if (roleModel.PreTimeNum > 1440)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前分钟数不能超过一天");
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} 31 12 ? ";
                                }
                                else
                                {
                                    cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }
                            else
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} L {roleModel.PeriodNumber2 - 1} ? ";
                                }
                                else
                                {
                                    cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }



                        }
                        else
                        {
                            cronStr = $"{createMeetingTime.Minute} {createMeetingTime.Hour} {roleModel.PeriodNumber3} {roleModel.PeriodNumber2} ? ";
                        }
                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        if (roleModel.PreTimeNum > 24)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前小时数不能超过24小时");
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L 12 ? ";
                                }
                                else
                                {
                                    cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }
                            else
                            {
                                if (roleModel.PeriodNumber3 == 1)
                                {
                                    cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L {roleModel.PeriodNumber2 - 1} ? ";
                                }
                                else
                                {
                                    cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                                }
                            }
                        }
                        else
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3} {roleModel.PeriodNumber2} ? ";
                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        hasAdvanceLessOneDay = true;
                        if (roleModel.PreTimeNum > 30)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前天数不能大于30");
                        if (roleModel.PeriodNumber3 == roleModel.PreTimeNum)
                        {
                            if (roleModel.PeriodNumber2 == 1)
                            {
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L 12 ? ";
                            }
                            else
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L {roleModel.PeriodNumber2 - 1} ? ";
                        }
                        else if (roleModel.PeriodNumber3 > roleModel.PreTimeNum)
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} {roleModel.PeriodNumber3 - roleModel.PreTimeNum} {roleModel.PeriodNumber2} ? ";
                        else
                        {
                            if (roleModel.PeriodNumber2 == 1)
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L-{roleModel.PreTimeNum - roleModel.PeriodNumber3} 12 ? ";
                            else
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L-{roleModel.PreTimeNum - roleModel.PeriodNumber3} {roleModel.PeriodNumber2 - 1} ? ";
                        }
                    }

                }
                else if (roleModel.PeriodNumber1 == 2) //每年几月最后几天
                {
                    if (roleModel.PeriodNumber3 > 5)
                        throw new UserFriendlyException((int)ErrorCode.CodeValErr, "最大只支持最后5天");
                    if (roleModel.PreTimeType == PreTimeType.分钟)
                    {
                        if (roleModel.PreTimeNum > 1440)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前分钟数不能超过一天");
                        createMeetingTime = roleActiveTime.AddMinutes(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L-{roleModel.PeriodNumber3} {roleModel.PeriodNumber2} ? ";
                        }
                        else
                        {
                            if (roleModel.PeriodNumber3 == 1)
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L {roleModel.PeriodNumber2} ? ";
                            else
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L-{roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                        }

                    }
                    else if (roleModel.PreTimeType == PreTimeType.小时)
                    {
                        if (roleModel.PreTimeNum > 24)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, "提前小时数不能超过24小时");
                        createMeetingTime = roleActiveTime.AddHours(0 - roleModel.PreTimeNum);
                        if (createMeetingTime.Day != roleActiveTime.Day)
                        {
                            hasAdvanceLessOneDay = true;
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L-{roleModel.PeriodNumber3} {roleModel.PeriodNumber2} ? ";
                        }
                        else
                        {
                            if (roleModel.PeriodNumber3 == 1)
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L {roleModel.PeriodNumber2} ? ";
                            else
                                cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour}  L-{roleModel.PeriodNumber3 - 1} {roleModel.PeriodNumber2} ? ";
                        }

                    }
                    else if (roleModel.PreTimeType == PreTimeType.天)
                    {
                        hasAdvanceLessOneDay = true;
                        if (roleModel.PreTimeNum + roleModel.PeriodNumber2 > 30)
                            throw new UserFriendlyException((int)ErrorCode.CodeValErr, $"每年{roleModel.PeriodNumber2}月最后第{roleModel.PeriodNumber3}天开会， 不能提前{roleModel.PreTimeNum}天创建会议！");
                        else
                            cronStr = $"{roleActiveTime.Minute} {roleActiveTime.Hour} L-{roleModel.PeriodNumber3 + roleModel.PreTimeNum - 1} {roleModel.PeriodNumber2} ? ";
                    }
                }
            }
            cronStr = "0 " + cronStr;
            roleModel.CronExpression = cronStr;
            roleModel.NextCreateTime = GetNextOccurrence(createMeetingTime, roleModel.CronExpression);
            roleModel.HasAdvanceLessOneDay = hasAdvanceLessOneDay;
            return roleModel;
        }


        public DateTime GetNextOccurrence(DateTime time, string cronExpression)
        {
            var cronDemo = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
            DateTime? nextTime = null;
            if (time.Kind != DateTimeKind.Utc)
            {
                var dt = new DateTimeOffset(time, TimeZoneInfo.Local.GetUtcOffset(time));
                nextTime = cronDemo.GetNextOccurrence(dt.UtcDateTime);
            }
            else
                nextTime = cronDemo.GetNextOccurrence(time);
            if (nextTime.HasValue)
                return nextTime.Value;
            else
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "获取下次执行时间失败");
        }





        public void JobForCycleForDay(Guid flowId)
        {
            RecurringJob.AddOrUpdate<IXZGLMeetingAppService>($"periodMeeting-CycleForDay", x => x.CreatePeriodJobForCycleForDay(flowId, DateTime.Now), Cron.MinuteInterval(5), System.TimeZoneInfo.Local);
        }

        /// <summary>
        /// 根据项目工时自动创建工作项到期待办提醒
        /// </summary>
        /// <param name="porjectId"></param>
        /// <param name="taskId"></param>
        public void Cancle(Guid id)
        {
            RecurringJob.RemoveIfExists($"periodMeeting-{id}");

        }
    }
}