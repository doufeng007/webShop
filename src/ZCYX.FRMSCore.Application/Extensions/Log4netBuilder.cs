using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZCYX.FRMSCore.Extensions
{
    public static class Log4netBuilder
    {
        public static void AddExceptionlog4Net(this IApplicationBuilder builder, string configfile, IConfiguration configuration)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configfile);
                XmlElement root = xmlDoc.DocumentElement;
                var rootlist = new List<XmlElement>();
                string appenderName = "Exceptionless";
                foreach (XmlElement appder in root.GetElementsByTagName("appender"))
                {
                    if (appder.GetAttribute("name") == appenderName)
                    {
                        rootlist.Add(appder);
                    }
                }
                rootlist.ForEach(x => root.RemoveChild(x));
                XmlElement appender = xmlDoc.CreateElement("appender");
                appender.SetAttribute("name", appenderName);
                appender.SetAttribute("type", "Exceptionless.Log4net.ExceptionlessAppender,Exceptionless.Log4net");
                appender.AppendChild(SetParm(xmlDoc, "apiKey", "value", configuration["Monitor:Exceptionless:apiKey"] ?? ""));
                appender.AppendChild(SetParm(xmlDoc, "serverUrl", "value", configuration["Monitor:Exceptionless:serverUrl"] ?? "http://localhost:8081"));
                var layout = SetParm(xmlDoc, "layout", "type", "log4net.Layout.PatternLayout");
                layout.AppendChild(SetParm(xmlDoc, "conversionPattern", "value", "%-4timestamp [%thread] %-5level %logger %ndc - %message%newline"));
                appender.AppendChild(layout);

                root.AppendChild(appender);
                var param = root.GetElementsByTagName("root")[0];
                var rootnodes = new List<XmlNode>();
                foreach (XmlNode appderref in param.ChildNodes)
                {
                    if (appderref.OuterXml != null && appderref.OuterXml.IndexOf(appenderName) > 0)
                    {
                        rootnodes.Add(appderref);
                    }
                }
                rootnodes.ForEach(x => param.RemoveChild(x));
                param.AppendChild(SetParm(xmlDoc, "appender-ref", "ref", appenderName));
                root.AppendChild(param);
                xmlDoc.Save(configfile);
            }
            catch (Exception ex)
            {
                // ignored

            }
        }

        static XmlElement SetParm(XmlDocument xmlDoc, string name, string key, string value)
        {
            XmlElement param = xmlDoc.CreateElement(name);
            param.SetAttribute(key, value);
            return param;
        }
    }
}
