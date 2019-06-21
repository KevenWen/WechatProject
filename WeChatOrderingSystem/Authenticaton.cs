
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace WeChatHelloWorld1
{
    public class Authentication
    {
        private string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
    

        public bool UserInfoCallback(string code, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(code))
            {
                errorMessage = "您拒绝了授权！";
                return false;
            }

            //if (state != HttpContext.Current.Session["State"] as string)
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下，
            //    //建议用完之后就清空，将其一次性使用
            //    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
            //    errorMessage = "验证失败！请从正规途径进入！";
            //    return false;
            //}

            OAuthAccessTokenResult result = null;

            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(appId, appSecret, code);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            if (result != null && !result.errcode.Equals(ReturnCode.请求成功))
            {
                errorMessage = result.errmsg;
                return false;
            }
            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            // Session["OAuthAccessToken"] = result;
            HttpContext.Current.Session["OpenID"] = result.openid;
            HttpContext.Current.Session["AccessToken"] = result.access_token;
            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息           
                return false;

            }
            
        }

    }


