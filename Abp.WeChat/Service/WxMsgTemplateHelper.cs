using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Abp.Runtime.Caching;

namespace Abp.WeChat
{
    public static class WxMsgTemplateHelper
    {
        private const string fileName = "WxMsgTemplate";


        public static List<Dictionary<string, string>> GetInfoFromCache(string elementName)
        {
            var _cacheManager = Abp.AbpBootstrapper.Create<Abp.Modules.AbpModule>().IocManager.IocContainer.Resolve<ICacheManager>();
            var cacheName = "InstalledWorkFlow";
            return _cacheManager
               .GetCache(cacheName)
               .Get<string, List<Dictionary<string, string>>>(elementName, f => GetInfo(elementName));
        }

        public static List<Dictionary<string, string>> GetInfo(string elementName)
        {
            var filepath = AppDomain.CurrentDomain.BaseDirectory + $"/Config/{fileName}.config";
            var resDic = new List<Dictionary<string, string>>();
            var xml = XElement.Load(filepath);
            var templateInfo = xml.Element(elementName);
            templateInfo.Elements().ToList().ForEach(x =>
            {
                var child = new Dictionary<string, string>();
                x.Elements().ToList().ForEach(y =>
                {
                    child.Add(y.Name.ToString(), y.Value);
                });
                resDic.Add(child);
            });
            return resDic;
        }
    }
}
