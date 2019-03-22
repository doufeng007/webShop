using Abp.UI;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCYX.FRMSCore.Web.Host.Startup
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public class ActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null && context.Exception.GetType() == typeof(Abp.UI.UserFriendlyException))
            {
                var code = ((Abp.UI.UserFriendlyException)context.Exception).Code;
                if (code == 0)
                {
                    var msg = context.Exception.Message;
                    if(msg.Contains("不能为空"))
                        throw new UserFriendlyException(999, context.Exception.Message);
                    if (msg.Length > 2)
                    {
                        var keywords = new[] { "用户", "角色", "密码", "名字", "无效", "邮箱", "对此", "已存" };
                        var key = msg.Substring(0, 2);
                        if(keywords.Count(x=>x==key)>0)
                            throw new UserFriendlyException(999, context.Exception.Message);
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {


        }
    }
}
