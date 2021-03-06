﻿using System;
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
    public class User_CustomerInfoController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: User_CustomerInfo
        public ActionResult Index()
        {
            return View(db.User_CustomerInfo.ToList());
        }

        // GET: User_CustomerInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_CustomerInfo user_CustomerInfo = db.User_CustomerInfo.Find(id);
            if (user_CustomerInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_CustomerInfo);
        }

        // GET: User_CustomerInfo/Create
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

        // POST: User_CustomerInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WeChatOpenID,CustomerName,PhoneNumber,Address")] User_CustomerInfo user_CustomerInfo)
        {
            if (ModelState.IsValid)
            {
                user_CustomerInfo.WeChatOpenID = Session["OpenID"].ToString();
                db.User_CustomerInfo.Add(user_CustomerInfo);
                var wechatUser = db.WeChatUsers.Find(Session["OpenID"].ToString());
                wechatUser.UserType = 1;
                db.Entry(wechatUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Products", new { id = user_CustomerInfo.ID });
            }

            return View(user_CustomerInfo);
        }

        // GET: User_CustomerInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_CustomerInfo user_CustomerInfo = db.User_CustomerInfo.Find(id);
            if (user_CustomerInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_CustomerInfo);
        }

        // POST: User_CustomerInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WeChatOpenID,PhoneNumber,Address")] User_CustomerInfo user_CustomerInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_CustomerInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_CustomerInfo);
        }

        // GET: User_CustomerInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_CustomerInfo user_CustomerInfo = db.User_CustomerInfo.Find(id);
            if (user_CustomerInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_CustomerInfo);
        }

        // POST: User_CustomerInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            User_CustomerInfo user_CustomerInfo = db.User_CustomerInfo.Find(id);
            db.User_CustomerInfo.Remove(user_CustomerInfo);
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
