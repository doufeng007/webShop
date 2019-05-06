using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using System.Linq.Dynamic;
using System.Diagnostics;
using Abp.Domain.Repositories;
using System.Web;
using Castle.Core.Internal;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ZCYX.FRMSCore;
using Abp.File;
using Abp.WorkFlow;
using ZCYX.FRMSCore.Application;
using ZCYX.FRMSCore.Extensions;
using ZCYX.FRMSCore.Model;
using Abp;
using Abp.Authorization;
using Abp.WeChat.Enum;
using Abp.Runtime.Caching;
using Abp.WeChat.Entity;

namespace Abp.WeChat
{
    public class WeChatAppService : FRMSCoreAppServiceBase, IWeChatAppService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<AbpWxTemplate, Guid> _repository;
        public WeChatAppService(ICacheManager cacheManager, IRepository<AbpWxTemplate, Guid> repository)
        {
            _cacheManager = cacheManager;
            _repository = repository;
        }

        public string GetOpenId(string code)
        {
            var token = WeChat.OAuthManager.GetAccessToken(SystemParameterHelper.AppId, SystemParameterHelper.AppSecret, code);
            if (token.errcode == ReturnCode.请求成功)
            {
                return token.openid;
            }
            else
                throw new UserFriendlyException((int)ErrorCode.CodeValErr, "获取微信用户OpenId失败");

        }


        public string GetTemplateIdByType(TemplateMessageBusinessTypeEnum type)
        {
            var datas = GetAbpWxTemplatesFromCache();
            var item = datas.FirstOrDefault(r => r.BType == type);
            return item.WxTemplateId;
        }

        public List<AbpWxTemplate> GetAbpWxTemplatesFromCache()
        {
            var cacheName = "AbpWxTemplate";
            return _cacheManager
               .GetCache(cacheName)
               .Get<string, List<AbpWxTemplate>>(cacheName, f => GetList());

        }


        public List<AbpWxTemplate> GetList()
        {
            var query = _repository.GetAll();
            return query.ToList();
        }
    }
}