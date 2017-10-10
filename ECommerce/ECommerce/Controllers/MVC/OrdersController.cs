using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ECommerce.Models;
using ECommerce.Classes;

namespace ECommerce.Controllers
{
    [Authorize(Roles ="User")]
    public class OrdersController : Controller
    {
        //tiene que tener persistencia
        //persona grabo 10 product y salio del sistema el pueda ver los 10 prod usaremos una tabla tmporal
        //crearemos la tabla order orderdetail y orderdetailtmp y cada que agreguemos un producto lo vamos a tirar a la tmp cuando el usuario 
        //de grabar orden pasamos todo lo de orderdetailtmp a orderdetail
        //se observa un movimiento implica manejar varias tablas.
        //registro que pasamos lo borramos de la temporal y todo ello lo pondremos en una transaccion.
        private ECommerceContext db = new ECommerceContext();

        public ActionResult DeleteProduct(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orderDetailTmp = db.OrderDetailTmps.Where(odt=>odt.UserName==User.Identity.Name && odt.ProductId==id).FirstOrDefault();
            if (orderDetailTmp == null)
            {
                return HttpNotFound();
            }

            db.OrderDetailTmps.Remove(orderDetailTmp);
            db.SaveChanges();
            return RedirectToAction("Create");
        }



        public ActionResult AddProduct() {
            //vista que tenga PRODUCTO Y Cantidad 
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId,true), "ProductId", "Description");

            return PartialView();
        }
        [HttpPost]
        public ActionResult AddProduct(AddProductView view)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            if (ModelState.IsValid) {
                var orderDetailTmps = db.OrderDetailTmps.Where(odt=>odt.UserName==User.Identity.Name && odt.ProductId==view.ProductId).FirstOrDefault();
                var product = db.Products.Find(view.ProductId);
                if (orderDetailTmps==null)
                {
                    orderDetailTmps = new OrderDetailTmp
                    {
                        Description = product.Description,
                        Price       = product.Price,
                        ProductId   = product.ProductId,
                        Quantity    = view.Quantity,
                        TaxRate     = product.Tax.Rate,
                        UserName    = User.Identity.Name
                    };
                    db.OrderDetailTmps.Add(orderDetailTmps);
                }
                else
                {
                    orderDetailTmps.Quantity += view.Quantity;
                    db.Entry(orderDetailTmps).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewBag.ProductId = new SelectList(CombosHelper.GetProducts(user.CompanyId,true),"ProductId","Description");
            return PartialView(view);
        }
        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var orders = db.Orders.Where(o=>o.CompanyId==user.CompanyId).Include(o => o.Customer).Include(o => o.State);
            return View(orders.ToList());
        }
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
        // GET: Orders/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName");
            var view = new NewOrderView
            {
                Date = DateTime.Now,
                Details = db.OrderDetailTmps.Where(odt => odt.UserName == User.Identity.Name).ToList(),
            };
            return View(view);
        }
        // POST: Orders/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewOrderView view)
        {
            if (ModelState.IsValid)
            {             
                    try
                    {
                        var response = MovementsHelper.NewOrder(view, User.Identity.Name);
                        if (response.Succeeded) {                          
                            return RedirectToAction("Index");
                        }
                        ModelState.AddModelError(string.Empty,response.Message);
                    }
                    catch (Exception)
                    {

                        throw;
                    }             
            }
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            ViewBag.CustomerId = new SelectList(CombosHelper.GetCustomers(user.CompanyId), "CustomerId", "FullName");
            view.Details = db.OrderDetailTmps.Where(odt => odt.UserName == User.Identity.Name).ToList();
            return View(view);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName", order.CustomerId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", order.StateId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "UserName", order.CustomerId);
            ViewBag.StateId = new SelectList(db.States, "StateId", "Description", order.StateId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
