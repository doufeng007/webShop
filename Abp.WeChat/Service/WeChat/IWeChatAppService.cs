using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.WorkFlow;
using Abp.WeChat.Enum;

namespace Abp.WeChat
{
    public interface IWeChatAppService : IApplicationService
    {
        /// <summary>
        /// 获取openId
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string GetOpenId(string code);


        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTemplateIdByType(TemplateMessageBusinessTypeEnum type);
    }
}