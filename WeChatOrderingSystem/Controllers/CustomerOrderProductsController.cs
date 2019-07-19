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
    public class CustomerOrderProductsController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: CustomerOrderProducts
        public ActionResult Index()
        {
            return View(db.CustomerOrderProducts.ToList());
        }

        // GET: CustomerOrderProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrderProduct customerOrderProduct = db.CustomerOrderProducts.Find(id);
            if (customerOrderProduct == null)
            {
                return HttpNotFound();
            }
            return View(customerOrderProduct);
        }

        // GET: CustomerOrderProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerOrderProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerOrderProductID,CustomerOrderID,Name,Price,Status,ImagePath,Created,LatestModify,Comment,MerchantID")] CustomerOrderProduct customerOrderProduct)
        {
            if (ModelState.IsValid)
            {
                db.CustomerOrderProducts.Add(customerOrderProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerOrderProduct);
        }

        // GET: CustomerOrderProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrderProduct customerOrderProduct = db.CustomerOrderProducts.Find(id);
            if (customerOrderProduct == null)
            {
                return HttpNotFound();
            }
            return View(customerOrderProduct);
        }

        // POST: CustomerOrderProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerOrderProductID,CustomerOrderID,Name,Price,Status,ImagePath,Created,LatestModify,Comment,MerchantID")] CustomerOrderProduct customerOrderProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerOrderProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerOrderProduct);
        }

        // GET: CustomerOrderProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrderProduct customerOrderProduct = db.CustomerOrderProducts.Find(id);
            if (customerOrderProduct == null)
            {
                return HttpNotFound();
            }
            return View(customerOrderProduct);
        }

        // POST: CustomerOrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerOrderProduct customerOrderProduct = db.CustomerOrderProducts.Find(id);
            db.CustomerOrderProducts.Remove(customerOrderProduct);
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
