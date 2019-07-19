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
    public class CustomerOrderStatuesController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: CustomerOrderStatues
        public ActionResult Index()
        {
            return View(db.CustomerOrderStatues.ToList());
        }

        // GET: CustomerOrderStatues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrderStatue customerOrderStatue = db.CustomerOrderStatues.Find(id);
            if (customerOrderStatue == null)
            {
                return HttpNotFound();
            }
            return View(customerOrderStatue);
        }

        // GET: CustomerOrderStatues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrderStatues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatueID,CustomerOrderID,Statue,UpdateDate,Comment")] CustomerOrderStatue customerOrderStatue)
        {
            if (ModelState.IsValid)
            {
                db.CustomerOrderStatues.Add(customerOrderStatue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerOrderStatue);
        }

        // GET: CustomerOrderStatues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrderStatue customerOrderStatue = db.CustomerOrderStatues.Find(id);
            if (customerOrderStatue == null)
            {
                return HttpNotFound();
            }
            return View(customerOrderStatue);
        }

        // POST: CustomerOrderStatues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatueID,CustomerOrderID,Statue,UpdateDate,Comment")] CustomerOrderStatue customerOrderStatue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerOrderStatue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerOrderStatue);
        }

        // GET: CustomerOrderStatues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrderStatue customerOrderStatue = db.CustomerOrderStatues.Find(id);
            if (customerOrderStatue == null)
            {
                return HttpNotFound();
            }
            return View(customerOrderStatue);
        }

        // POST: CustomerOrderStatues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerOrderStatue customerOrderStatue = db.CustomerOrderStatues.Find(id);
            db.CustomerOrderStatues.Remove(customerOrderStatue);
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
