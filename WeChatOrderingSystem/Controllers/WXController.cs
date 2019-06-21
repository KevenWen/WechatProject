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
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler;

namespace WeChatHelloWorld1.Controllers
{
    public class WXController : Controller
    {
        public static readonly string Token = Config.SenparcWeixinSetting.Token;//与微信公众账号后台的Token设置保持一致，区分大小写。
        public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.EncodingAESKey;//与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        public static readonly string CorpId = Config.SenparcWeixinSetting.WeixinCorpId;
        public static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
        public static readonly string AppSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        /*
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
        */

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// PS：此方法为简化方法，效果与OldPost一致。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            if (!CheckSignature(Token, postModel.Signature, postModel.Timestamp, postModel.Nonce))
            {
                return Content("参数错误！");
            }

            string logFileName = "weChatLog" + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                #region 打包 PostModel 信息

                postModel.Token = Token;//根据自己后台的设置保持一致
                postModel.EncodingAESKey = EncodingAESKey;//根据自己后台的设置保持一致
                                                          //postModel.AppId = AppId;//根据自己后台的设置保持一致

                #endregion

                //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
                var maxRecordCount = 10;

                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);

                #region 设置消息去重

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能

                #endregion

                #region 记录 Request 日志

                var logPath = Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }

                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}_{2}.txt", logFileName,
                    messageHandler.RequestMessage.FromUserName,
                    messageHandler.RequestMessage.MsgType)));
                if (messageHandler.UsingEcryptMessage)
                {
                    messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}_{2}.txt", logFileName,
                        messageHandler.RequestMessage.FromUserName,
                        messageHandler.RequestMessage.MsgType)));
                }

                #endregion


                //执行微信处理过程
                messageHandler.Execute();

                #region 记录 Response 日志

                //测试时可开启，帮助跟踪数据

                //if (messageHandler.ResponseDocument == null)
                //{
                //    throw new Exception(messageHandler.RequestDocument.ToString());
                //}
                if (messageHandler.ResponseDocument != null)
                {
                    messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}_{2}.txt", logFileName,
                        messageHandler.ResponseMessage.ToUserName,
                        messageHandler.ResponseMessage.MsgType)));
                }

                if (messageHandler.UsingEcryptMessage && messageHandler.FinalResponseDocument != null)
                {
                    //记录加密后的响应信息
                    messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}_{2}.txt", logFileName,
                        messageHandler.ResponseMessage.ToUserName,
                        messageHandler.ResponseMessage.MsgType)));
                }

                #endregion

                return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new WeixinResult(messageHandler);//v0.8+
                //return new FixWeixinBugWeixinResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                #region 异常处理
                WeixinTrace.Log("MessageHandler错误：{0}", ex.Message);

                using (TextWriter tw = new StreamWriter(Server.MapPath("~/App_Data/Error_" + logFileName + ".txt")))
                {
                    tw.WriteLine("ExecptionMessage:" + ex.Message);
                    tw.WriteLine(ex.Source);
                    tw.WriteLine(ex.StackTrace);
                    //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                    var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);
                    if (messageHandler.ResponseDocument != null)
                    {
                        tw.WriteLine(messageHandler.ResponseDocument.ToString());
                    }

                    if (ex.InnerException != null)
                    {
                        tw.WriteLine("========= InnerException =========");
                        tw.WriteLine(ex.InnerException.Message);
                        tw.WriteLine(ex.InnerException.Source);
                        tw.WriteLine(ex.InnerException.StackTrace);
                    }

                    tw.Flush();
                    tw.Close();
                }                
                return Content(ex.Message);
                #endregion
            }
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


        //[HttpPost]
        //public ActionResult Index()
        //{
        //    string postString = string.Empty;
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(Request.InputStream);
        //    return Content(TextHandle(doc));
        //}

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
                    type = MenuButtonType.view.ToString(), //默认已经设为此类型，这里只作为演示
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
