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
    public class WeChatUsersController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: WeChatUsers
        public ActionResult Index()
        {
            return View(db.WeChatUsers.ToList());
        }

        // GET: WeChatUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeChatUser weChatUser = db.WeChatUsers.Find(id);
            if (weChatUser == null)
            {
                return HttpNotFound();
            }
            return View(weChatUser);
        }

        // GET: WeChatUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WeChatUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OpenID,Nickname,Sex,Province,City,Country,HeadImgUrl,SubscribeTime,Language,UserType")] WeChatUser weChatUser)
        {
            if (ModelState.IsValid)
            {
                db.WeChatUsers.Add(weChatUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(weChatUser);
        }

        // GET: WeChatUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeChatUser weChatUser = db.WeChatUsers.Find(id);
            if (weChatUser == null)
            {
                return HttpNotFound();
            }
            return View(weChatUser);
        }

        // POST: WeChatUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OpenID,Nickname,Sex,Province,City,Country,HeadImgUrl,SubscribeTime,Language,UserType")] WeChatUser weChatUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weChatUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(weChatUser);
        }

        // GET: WeChatUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeChatUser weChatUser = db.WeChatUsers.Find(id);
            if (weChatUser == null)
            {
                return HttpNotFound();
            }
            return View(weChatUser);
        }

        // POST: WeChatUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            WeChatUser weChatUser = db.WeChatUsers.Find(id);
            db.WeChatUsers.Remove(weChatUser);
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
