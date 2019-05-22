using Abp.Application.Services;
using Abp.Reflection.Extensions;
using Abp.UI;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using ZCYX.FRMSCore.Configuration;
using ZCYX.FRMSCore.Model;

namespace AliSms
{
    [RemoteService(IsEnabled = false)]
    public class AliSmsManager : ApplicationService
    {
        private readonly IConfigurationRoot _appConfiguration;
        public static string AliAccessKeyId { get; set; }

        public static string AliAccessSecret { get; set; }


        public AliSmsManager()
        {
            var coreAssemblyDirectoryPath = typeof(AliSmsManager).GetAssembly().GetDirectoryPathOrNull();
            _appConfiguration = AppConfigurations.Get(coreAssemblyDirectoryPath);
            AliAccessKeyId = _appConfiguration["App:AliAccessKeyId"].ToString();

            AliAccessSecret = _appConfiguration["App:AliAccessSecret"].ToString();
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        public void SendSms(string templateCode, string signName, string phoneNumber, string templateParam)
        {
            IClientProfile profile = DefaultProfile.GetProfile("default", AliAccessKeyId, AliAccessSecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;


            //request.AddQueryParameters("TemplateCode", "SMS_164860191");
            //request.AddQueryParameters("SignName", "乌生青");
            //request.AddQueryParameters("PhoneNumbers", "15680670908");
            //request.AddQueryParameters("TemplateParam", "{\"code\":\"556677\"}");

            request.AddQueryParameters("TemplateCode", templateCode);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("PhoneNumbers", phoneNumber);
            request.AddQueryParameters("TemplateParam", templateParam);

            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                dynamic retData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Data);
                if (retData.Code != "OK")
                {
                    Abp.Logging.LogHelper.Logger.Error($"阿里短信接口失败：{retData.Message}");
                    throw new UserFriendlyException((int)ErrorCode.CodeValErr, "阿里短信接口失败");
                }

            }
            catch (ServerException e)
            {
                Abp.Logging.LogHelper.Logger.Error($"阿里短信接口失败：{e.Message}|{e.ErrorMessage}");
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "阿里短信接口失败");
            }
            catch (ClientException e)
            {
                Abp.Logging.LogHelper.Logger.Error($"阿里短信接口失败：{e.Message}|{e.ErrorMessage}");
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "阿里短信接口失败");
            }
        }
    }
}
