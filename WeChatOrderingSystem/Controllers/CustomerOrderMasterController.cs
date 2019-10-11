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
    public class CustomerOrderMasterController : Controller
    {
        private WeChatHelloWorld1Context db = new WeChatHelloWorld1Context();

        // GET: CustomerOrderMaster
        public ActionResult Index()
        {
            var orderList = db.CustomerOrders.ToList();
            List<CustomerOrderMaster> oMlist = new List<CustomerOrderMaster>();
            foreach (CustomerOrder o in orderList)
            {
                var temp = new CustomerOrderMaster() { CustomerOrder = o };
                oMlist.Add(temp);
            }
            return View(oMlist);
        }

        // GET: CustomerOrderMaster/Details/5
        public ActionResult Details(int? id)
        {
            id = 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerOrder customerOrder = db.CustomerOrders.Find(id);
            CustomerOrderMaster customerOrderMaster = new CustomerOrderMaster();


            customerOrderMaster.CustomerOrder = customerOrder;

            if (customerOrderMaster == null)
            {
                return HttpNotFound();
            }
            var sList = db.CustomerOrderStatues.Where(c => c.CustomerOrderID == id);
            if(sList.Any())
            {
                customerOrderMaster.StatueList = sList.ToList();
            }
            var pList = db.CustomerOrderProducts.Where(c => c.CustomerOrderID == id);
            if (pList.Any())
            {
                customerOrderMaster.ProductList = pList.ToList();
            }
            return View(customerOrderMaster);
        }

        // GET: CustomerOrderMaster/Create
        public ActionResult Create()
        {
            return View();
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
