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
    public class User_MerchantInfoController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: User_MerchantInfo
        public ActionResult Index()
        {
            return View(db.User_MerchantInfo.ToList());
        }

        // GET: User_MerchantInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_MerchantInfo user_MerchantInfo = db.User_MerchantInfo.Find(id);
            if (user_MerchantInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_MerchantInfo);
        }

        // GET: User_MerchantInfo/Create
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

        // POST: User_MerchantInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WeChatOpenID,MerchantName,MerchantAddress,PhoneNumber")] User_MerchantInfo user_MerchantInfo)
        {
            if (ModelState.IsValid)
            {
                // user_MerchantInfo.WeChatOpenID = "testopenid123456";
                user_MerchantInfo.WeChatOpenID = Session["OpenID"].ToString();
                db.User_MerchantInfo.Add(user_MerchantInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_MerchantInfo);
        }

        // GET: User_MerchantInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_MerchantInfo user_MerchantInfo = db.User_MerchantInfo.Find(id);
            if (user_MerchantInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_MerchantInfo);
        }

        // POST: User_MerchantInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WeChatOpenID,MerchantName,MerchantAddress,PhoneNumber")] User_MerchantInfo user_MerchantInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_MerchantInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_MerchantInfo);
        }

        // GET: User_MerchantInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_MerchantInfo user_MerchantInfo = db.User_MerchantInfo.Find(id);
            if (user_MerchantInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_MerchantInfo);
        }

        // POST: User_MerchantInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            User_MerchantInfo user_MerchantInfo = db.User_MerchantInfo.Find(id);
            db.User_MerchantInfo.Remove(user_MerchantInfo);
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
