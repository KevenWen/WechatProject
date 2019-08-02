
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WeChatHelloWorld1.Models;

namespace WeChatHelloWorld1
{
    public class Authentication
    {
        private string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

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
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                var weChatUser = db.WeChatUsers.Find(userInfo.openid);
                if (weChatUser == null)
                {
                    int rerfererID = 0;
                    if (HttpContext.Current.Session["refererID"] != null)
                    {
                        rerfererID = int.Parse(HttpContext.Current.Session["refererID"].ToString());
                    }

                    weChatUser = new WeChatUser
                    {
                        OpenID = userInfo.openid,
                        Nickname = userInfo.nickname,
                        Sex = userInfo.sex,
                        Country = userInfo.country,
                        Province = userInfo.province,
                        City = userInfo.city,
                        HeadImgUrl = userInfo.headimgurl,
                        SubscribeTime = DateTime.Now,
               
                    };
                    db.WeChatUsers.Add(weChatUser);
                    db.SaveChanges();
                   
                }
                else
                {
                    switch (weChatUser.UserType)
                    {
                        case 0:
                            var customer = db.User_AdminInfo.Where(m => m.WeChatOpenID == result.openid).FirstOrDefault();
                            {
                                if (customer != null)
                                {
                                    HttpContext.Current.Session["Customer"] = customer;    
                                    return true;
                                }
                            }
                            break;
                       
                        case 2:
                            var merchant = db.User_MerchantInfo.Where(c => c.WeChatOpenID == result.openid).FirstOrDefault();
                            if (merchant != null)
                            {
                                HttpContext.Current.Session["Customer"] = merchant;     
                                return true;
                            }
                            break;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {              
                
                return false;
            }
        }

    }

}


