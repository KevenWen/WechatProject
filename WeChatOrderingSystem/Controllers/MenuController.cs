using System.Web.Mvc;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.NeuChar;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin;

namespace WeChatHelloWorld1.Controllers
{
    public class MenuController : Controller
    {

        public static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string AppSecret = Config.SenparcWeixinSetting.WeixinAppSecret;


        public ActionResult CreateMenuForMerchant()
        {
            ButtonGroup bg = new ButtonGroup();
            // 添加注册菜单
            bg.button.Add(new SingleViewButton()
            {
                name = "注册",
                url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", AppId, Url.Encode("https://www.share-parking.com/User/Create")),
                type = MenuButtonType.view.ToString(), 
            });
            bg.button.Add(new SingleViewButton()
            {
                name = "菜单",
                url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", AppId, Url.Encode("https://www.share-parking.com/Menu/")),
                type = MenuButtonType.view.ToString(),
                
            });
            bg.button.Add(new SingleViewButton()
            {
                name = "我的订单",
                url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect", AppId, Url.Encode("https://www.share-parking.com/Order/")),
                type = MenuButtonType.view.ToString(),

            });
            var accessToken = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
            var result = CommonApi.CreateMenu(accessToken, bg);

            return Json(new
            {
                Success = result.errmsg == "ok",
                Message = "菜单更新成功。"
            }, JsonRequestBehavior.AllowGet);


        }
    }
}