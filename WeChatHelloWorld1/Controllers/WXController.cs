using System;
using System.IO;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using System.Web.Security;
using System.Net;
using System.Text;
using Newtonsoft.Json;
//using Senparc.Weixin.Work.Containers;
//using Senparc.Weixin.Work.Entities.Menu;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;
using Senparc.NeuChar;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin;

namespace WeChatHelloWorld1.Controllers
{
    public class WXController : Controller
    {
        public static readonly string Token = Config.SenparcWeixinSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string CorpId = Config.SenparcWeixinSetting.WeixinCorpId;
        public static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string AppSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        // GET: WX
        public ActionResult Index(string echoStr, string signature, string timestamp, string nonce)
        {
            string WeiXinToken = "Hightch.Wu@quest.com";//要和你微信公众平台设置的保持一致          

            if (CheckSignature(WeiXinToken, signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    return Content(echoStr);
                }
            }

            return Content("");
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        public static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            //tmpStr = Membership.CreateUser(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult MenuManager()
        {
            return View();
        }

        public ActionResult TestButton()
        {
            CreateMenuWithButtonGroup();
            return Content("create ok");
        }


        [HttpPost]
        public ActionResult Index()
        {
            string postString = string.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(Request.InputStream);
            return Content(TextHandle(doc));
        }

        /// <summary>
        /// 处理信息并应答
        /// </summary>
        public string TextHandle(XmlDocument xmldoc)
        {
            string responseContent = "";

            XmlNode ToUserName = xmldoc.SelectSingleNode("/xml/ToUserName");
            XmlNode FromUserName = xmldoc.SelectSingleNode("/xml/FromUserName");
            XmlNode Content = xmldoc.SelectSingleNode("/xml/Content");
            // XmlNode Event = xmldoc.SelectSingleNode("/xml/Event");
            if (Content.InnerText == "menuC")
            {
                responseContent = string.Format(Message_Text,
                        FromUserName.InnerText,
                        ToUserName.InnerText,
                        DateTime.Now.Ticks,
                        "Going to create Menu.");
                CreateMenu();
            }
            else if (Content.InnerText == "menuD")
            {
                responseContent = string.Format(Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "Going to remove Menu.");
                DeleteMenu();
            }
            else if (Content.InnerText == "Order")
            {
                responseContent = string.Format(Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    CreateMenuWithButtonGroup());
            }
            else if (Content != null)
            {
                responseContent = string.Format(Message_Text,
                    FromUserName.InnerText,
                    ToUserName.InnerText,
                    DateTime.Now.Ticks,
                    "您输入的内容为：" + Content.InnerText);
            }
            return responseContent;
        }

        private string CreateMenuWithButtonGroup()
        {
            try
            {
                var accessToken = AccessTokenContainer.TryGetAccessToken(AppId, AppSecret);
                ButtonGroup bg = new ButtonGroup();
                // 添加 Order 菜单
                bg.button.Add(new SingleClickButton()
                {
                    name = "Order",
                    key = "OrderClick",
                    type = MenuButtonType.click.ToString(), //默认已经设为此类型，这里只作为演示
                });
                var result = CommonApi.CreateMenu(accessToken, bg);
                return result.ToString();
            }
            catch (Exception e)
            {
                return e.StackTrace.ToString();
            }
        }

        /// <summary>
        /// 普通文本消息
        /// </summary>
        public static string Message_Text
        {
            get { return @"<xml>
                            <ToUserName><![CDATA[{0}]]></ToUserName>
                            <FromUserName><![CDATA[{1}]]></FromUserName>
                            <CreateTime>{2}</CreateTime>
                            <MsgType><![CDATA[text]]></MsgType>
                            <Content><![CDATA[{3}]]></Content>
                            </xml>"; }
        }

        public ActionResult CreateMenu()
        {
            string accessT = get_accessToken();
            string re = CMenu(accessT);
            return Content(re);
        }

        public string CMenu(string accessToken)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + accessToken;
            string postData = "{\"button\":[{\"type\":\"miniprogram\",\"name\":\"calendar\",\"url\":\"http://mp.weixin.qq.com\",\"appid\":\"wx286b93c14bbf93aa\",\"pagepath\":\"pages/lunar/index\"},{\"type\":\"view\",\"name\":\"Search\",\"url\":\"http://www.baidu.com\"}]}]}";
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(url) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //WriteLog("response" + response);
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                Response.Write(content);
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        public ActionResult DeleteMenu()
        {
            string accessT = get_accessToken();
            string postUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + accessT;
            string re = DMenu(accessT);
            return Content(re);
        }

        public string DMenu(string posturl)
        {
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                Response.Write(content);
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }

        }

        public static string get_accessToken()
        {
            string grant_type = "client_credential";
            string appid = "wx950ef0bc651572ce";//你的appid
            string secret = "8f8dc23f7dc4ae67f69a892dc6787218";//你的secret
            string tokenUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}", grant_type, appid, secret);
            //提供向 URI 标识的资源发送数据和从 URI 标识的资源接收数据的公共方法，
            //WebClient 类提供向 URI 标识的任何本地、Intranet 或 Internet 资源发送数据以及从这些资源接收数据的公共方法
            WebClient client = new WebClient();
            //从微信API下载access_token 并返回access_token 的值
            string strReturn = Encoding.UTF8.GetString(client.DownloadData(tokenUrl));
            //  string accesstoken = strReturn.Substring(strReturn.IndexOf("access_token\":\"") + 15, strReturn.IndexOf("\",\"expires") - (strReturn.IndexOf("access_token\":\"")+15)); 
            //反序列化JsonConvert.DeserializeObject
            GetAccess_Token getAccess_Token = JsonConvert.DeserializeObject<GetAccess_Token>(strReturn);
            return getAccess_Token.access_token;
        }

        public class GetAccess_Token
        {
            //微信返回的accesstoken
            public string access_token;
            //微信返回的accesstoken的有效时间 7200
            public int expires_in;


            public string Access_token
            {
                get
                {
                    return this.access_token;
                }
                set
                {
                    this.access_token = value;
                }
            }
            public int Expires_in
            {
                get
                {
                    return this.expires_in;
                }
                set
                {
                    this.expires_in = value;
                }
            }

        }



    }
}
