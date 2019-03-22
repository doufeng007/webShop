using Abp.Application.Services.Dto;
using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZCYX.FRMSCore
{
    public class DocmentQrcode : EventData
    {
        public Guid QrCode { get; set; }
    }

    public class ProjectLeaderChange : EventData
    {
        public Guid ProjectId { get; set; }


        public long UserId { get; set; }
    }
}
