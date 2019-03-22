using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Abp.File;
using Abp.Runtime.Caching;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using ZCYX.FRMSCore;

namespace Docment
{
    public class DocmentQrcodeHandler : IEventHandler<DocmentQrcode>, ISingletonDependency
    {
        private readonly IDocmentAppService _iDocmentAppServiceRepository;
        public DocmentQrcodeHandler(IDocmentAppService iDocmentAppServiceRepository)
        {
            _iDocmentAppServiceRepository = iDocmentAppServiceRepository;
        }

        public void HandleEvent(DocmentQrcode eventData)
        {
            _iDocmentAppServiceRepository.GetFlowingDocment(eventData.QrCode);

        }
    }
}
