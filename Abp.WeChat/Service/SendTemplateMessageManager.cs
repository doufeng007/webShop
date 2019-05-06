using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

using Abp.Domain.Repositories;

using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Configuration;
using Abp.Net.Mail;
using Newtonsoft.Json;
using Abp.Application.Services;

namespace Abp.WeChat
{
    public class SendTemplateMessageManager : ApplicationService
    {



        /// <summary>
        /// 享叮当公众号发送模板消息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="templateId">模板ID</param>
        /// <param name="openId">微信openId</param>
        /// <param name="url">点击详情跳转路径</param>
        /// <param name="data"></param>
        /// <param name="result"></param>
        public static bool SendTemplateMessage(string templateId, string openId, string url, object data,
            out TemplateMessageResult result)
        {
            var isSuccess = false;
            result = new TemplateMessageResult();
            var accessToken = AccessTokenContainer.TryGetToken(SystemParameterHelper.AppId, SystemParameterHelper.AppSecret);
            

            var getInfoUrl = string.Format(SystemParameterHelper.GetBaseUserInfoApi, accessToken, openId);
            var userInfo = HttpClientHelper.GetResponse<WeixinUserInfoResult>(getInfoUrl);
            //判断用户是否关注公众号
            switch (userInfo.subscribe)
            {
                case 0:
                    break;
                default:
                    var sendUrl = string.Format(SystemParameterHelper.SendMessageApi, accessToken);
                    var msg = new TemplateMessage
                    {
                        template_id = templateId,
                        touser = openId,
                        url = url,
                        data = data
                    };
                    //序列化实体为json
                    string json = JsonConvert.SerializeObject(msg,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});

                    //调用消息发送接口
                    result = HttpClientHelper.PostResponse<TemplateMessageResult>(string.Format(sendUrl, accessToken),
                        json);
                    isSuccess = true;
                    break;
            }
            return isSuccess;
        }

      


    }



    public static class SystemParameterHelper
    {

        /// <summary>
        /// 享叮当公众号相关信息
        /// </summary>
        public static string AppId = ConfigurationManager.AppSettings["WeChatAppId"];

        public static string AppSecret = ConfigurationManager.AppSettings["WeChatAppSecret"];

        /// <summary>
        /// 发送模板消息API
        /// </summary>
        public static string SendMessageApi = ConfigurationManager.AppSettings["SendMessageApi"];

        /// <summary>
        /// 获取用信息API
        /// </summary>
        public static string GetBaseUserInfoApi = ConfigurationManager.AppSettings["GetBaseUserInfoApi"];



    }
}
