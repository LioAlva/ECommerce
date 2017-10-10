using ECommerce.Classes;
using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    [Authorize(Roles ="User")]
    public class CustomersController : Controller
    {
        private ECommerceContext db = new ECommerceContext();

        // GET: Customers
        public ActionResult Index()
        {

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            var qry = (from cu in db.Customers
                       join cc in db.CompanyCustomers on cu.CustomerId equals cc.CustomerId
                       join co in db.Companies on cc.CompanyId equals co.CompanyId
                       where co.CompanyId == user.CompanyId
                       select new { cu }).ToList();//todos los customers 

            var customers = new List<Customer>();

            foreach (var item in qry)
            {
                customers.Add(item.cu);
            }

            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");

            return View();
        }

        // POST: Customers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Customer customer)
        {
            if (ModelState.IsValid)
            {//cuando se manipula mas de dos tablas se usan transaciones
                using (var transaction=db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Customers.Add(customer);
                        var response = DBHelper.SaveChanges(db);//TODO:Validate when the custoimer email change, para pendientes para no perderse del dessarrollo
                        if (!response.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, response.Message);
                            transaction.Rollback();
                            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
                            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
                            return View(customer);
                        }

                        UsersHelper.CreateUserASP(customer.UserName, "Customer");
                        var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

                        var companyCustomer = new CompanyCustomer
                        {
                            CompanyId = user.CompanyId,
                            CustomerId = customer.CustomerId
                        };
                        db.CompanyCustomers.Add(companyCustomer);
                        db.SaveChanges();

                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }                    
            }

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(0), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(customer.DepartmentId), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Customer customer)
        {
            if (ModelState.IsValid)
            {
              
                db.Entry(customer).State = EntityState.Modified;

                var response= DBHelper.SaveChanges(db);//TODO:Validate when the custoimer email change, para pendientes para no perderse del dessarrollo
                if (response.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty,response.Message);
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(customer.DepartmentId), "CityId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            return View(customer);
        }
        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Customers.Find(id);
            // db.Customers.Remove(customer);
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var companyCustomer = db.CompanyCustomers.Where(u => u.CompanyId == user.CompanyId && customer.CustomerId == id).FirstOrDefault();
            using (var transaction=db.Database.BeginTransaction())
            {
                db.CompanyCustomers.Remove(companyCustomer);
                var response = DBHelper.SaveChanges(db);//TODO:Validate when the custoimer email change, para pendientes para no perderse del dessarrollo
                if (response.Succeeded)
                { //No le qitare el permiso de usuario
                    transaction.Commit();
                    return RedirectToAction("Index");//vista index de customers
                }
                transaction.Rollback();
                ModelState.AddModelError(string.Empty, response.Message);
                return View(customer);
            }
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
