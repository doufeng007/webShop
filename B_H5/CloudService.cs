using Abp.Application.Services;
using ServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace B_H5
{
    [RemoteService(IsEnabled = false)]
    public class CloudService : ApplicationService
    {
        public APIWebServiceSoapClient SoapClient { get; set; }
        public CloudService()
        {
            SoapClient = new ServiceReference.APIWebServiceSoapClient(ServiceReference.APIWebServiceSoapClient.EndpointConfiguration.APIWebServiceSoap);
        }



    }
}
