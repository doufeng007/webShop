using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.WeChat
{
    /// <summary>
    /// 通用接口
    /// 通用接口用于和微信服务器通讯，一般不涉及自有网站服务器的通讯
    /// 见 http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E5%8F%A3%E6%96%87%E6%A1%A3&oldid=103
    /// </summary>
    public partial class CommonApi
    {
        /// <summary>
        /// 获取凭证接口
        /// </summary>
        /// <param name="grant_type">获取access_token填写client_credential</param>
        /// <param name="appid">第三方用户唯一凭证</param>
        /// <param name="secret">第三方用户唯一凭证密钥，既appsecret   6981609be49d16714f35b9036951d605</param>
        /// <returns></returns>
        public static AccessTokenResult GetToken(string appid, string secret, string grant_type = "client_credential")
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}",
                                    grant_type, appid, secret);

            AccessTokenResult result = HttpClientHelper.GetResponse<AccessTokenResult>(url);
            return result;
        }

        /// <summary>
        /// 用户信息接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static WeixinUserInfoResult GetUserInfo(string accessToken, string openId)
        {
            var url = string.Format("http://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}",
                                    accessToken, openId);
            WeixinUserInfoResult result = HttpClientHelper.GetResponse<WeixinUserInfoResult>(url);
            return result;
        }

        /// <summary>
        /// 获取调用微信JS接口的临时票据
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static JsApiTicketResult GetTicket(string appId, string secret, string type = "jsapi")
        {
            var accessToken = AccessTokenContainer.TryGetToken(appId, secret);

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}",
                                    accessToken, type);

            JsApiTicketResult result = HttpClientHelper.GetResponse<JsApiTicketResult>(url);
            return result;
        }

        public static JsApiTicketResult GetTicketByAccessToken(string accessToken, string type = "jsapi")
        {

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}",
                                    accessToken, type);

            JsApiTicketResult result = HttpClientHelper.GetResponse<JsApiTicketResult>(url);
            return result;
        }

    }

}
