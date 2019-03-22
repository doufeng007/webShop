using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Abp.File;
using Abp.Runtime.Caching;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZCYX.FRMSCore;

namespace MeetingGL
{
    public class MeetingRoomUseInfoByEventHandler : IEventHandler<MeetingRoomUseInfoByEvent>, ISingletonDependency
    {
        private readonly IRepository<MeetingRoomUseInfo, Guid> _repository;
        private readonly IRepository<XZGLMeetingRoom, Guid> _xZGLMeetingRoomRepository;
        public MeetingRoomUseInfoByEventHandler(IRepository<MeetingRoomUseInfo, Guid> repository, IRepository<XZGLMeetingRoom, Guid> xZGLMeetingRoomRepository)
        {
            _repository = repository;
            _xZGLMeetingRoomRepository = xZGLMeetingRoomRepository;
        }

        public  void HandleEvent(MeetingRoomUseInfoByEvent model)
        {
            var businessType = (MeetingRoomUseBusinessType)model.BusinessType;
            var dbmodel = _repository.GetAll().FirstOrDefault(x => x.MeetingRoomId == model.MeetingRoomId && x.BusinessId == model.BusinessId && x.BusinessType == businessType);
            if (model.IsDelete)
            {
                _repository.Delete(x => x.Id == dbmodel.Id);
                return;
            }
            if (dbmodel == null)
            {
                var newmodel = new MeetingRoomUseInfo()
                {
                    MeetingRoomId = model.MeetingRoomId,
                    BusinessId = model.BusinessId,
                    BusinessType = businessType,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    BusinessName = model.BusinessName,
                    Status = model.Status
                };

                var roomModel =  _xZGLMeetingRoomRepository.Get(model.MeetingRoomId);
                roomModel.BookingStatus = 1;
                 _repository.Insert(newmodel);
            }
            else
            {
                dbmodel.MeetingRoomId = model.MeetingRoomId;
                dbmodel.BusinessId = model.BusinessId;
                dbmodel.BusinessName = model.BusinessName;
                dbmodel.BusinessType = businessType;
                dbmodel.StartTime = model.StartTime;
                dbmodel.EndTime = model.EndTime;
                dbmodel.Status = model.Status;
                 _repository.Update(dbmodel);
            }
        }
    }
}
