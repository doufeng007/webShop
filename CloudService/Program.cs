using CloudApiService;
using System;

namespace CloudService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var client = new CloudApiService.APIWebServiceSoapClient(APIWebServiceSoapClient.EndpointConfiguration.APIWebServiceSoap);
            var ret = client.getCountryAsync("fe6db94a-68d4-47fa-838b-42955529807380010");












        }
    }
}
