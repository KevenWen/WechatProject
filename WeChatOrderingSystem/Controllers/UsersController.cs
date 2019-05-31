using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Containers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeChatHelloWorld1.Models;

namespace WeChatHelloWorld1.Controllers
{
    public class UsersController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create(string code)
        {
            if (code != null)
            {
                Authentication auth = new Authentication();
                string errorMessage;        
                auth.UserInfoCallback(code, out errorMessage);
                ViewBag.OpenID = Session["OpenID"].ToString();
            }
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OpenID,UserName,Address,Phone")] User user)
        {
            if (ModelState.IsValid)
            {

                // Create wechatUser
                //if (Session["AccessToken"] == null)
                //{
                //    var accessToken = AccessTokenContainer.TryGetAccessToken(Config.SenparcWeixinSetting.WeixinAppId, Config.SenparcWeixinSetting.WeixinAppSecret);
                //    Session["AccessToken"] = accessToken;
                //}

                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(Session["AccessToken"].ToString(), Session["OpenID"].ToString());
                var weChatUser = db.WeChatUsers.Find(userInfo.openid);
                if (weChatUser == null)
                {
                    weChatUser = new WeChatUser
                    {
                        OpenID = userInfo.openid,
                        Nickname = userInfo.nickname,
                        Sex = userInfo.sex,
                        Country = userInfo.country,
                        Province = userInfo.province,
                        City = userInfo.city,
                        HeadImgUrl = userInfo.headimgurl,
                        SubscribeTime = DateTime.Now
                    };
                    db.WeChatUsers.Add(weChatUser);
                    // Create User
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OpenID,UserName,Address,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
