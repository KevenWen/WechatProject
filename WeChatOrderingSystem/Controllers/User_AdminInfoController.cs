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
    public class User_AdminInfoController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: User_AdminInfo
        public ActionResult Index()
        {
            return View(db.User_AdminInfo.ToList());
        }

        // GET: User_AdminInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_AdminInfo user_AdminInfo = db.User_AdminInfo.Find(id);
            if (user_AdminInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_AdminInfo);
        }

        // GET: User_AdminInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User_AdminInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WeChatOpenID,AdminName,PhoneNumber")] User_AdminInfo user_AdminInfo)
        {
            if (ModelState.IsValid)
            {
                // user_AdminInfo.WeChatOpenID = "admopenid234545";
                db.User_AdminInfo.Add(user_AdminInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user_AdminInfo);
        }

        // GET: User_AdminInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_AdminInfo user_AdminInfo = db.User_AdminInfo.Find(id);
            if (user_AdminInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_AdminInfo);
        }

        // POST: User_AdminInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,WeChatOpenID,AdminName,PhoneNumber")] User_AdminInfo user_AdminInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_AdminInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user_AdminInfo);
        }

        // GET: User_AdminInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_AdminInfo user_AdminInfo = db.User_AdminInfo.Find(id);
            if (user_AdminInfo == null)
            {
                return HttpNotFound();
            }
            return View(user_AdminInfo);
        }

        // POST: User_AdminInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            User_AdminInfo user_AdminInfo = db.User_AdminInfo.Find(id);
            db.User_AdminInfo.Remove(user_AdminInfo);
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
