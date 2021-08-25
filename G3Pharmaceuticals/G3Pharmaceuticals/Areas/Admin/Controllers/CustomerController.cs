using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        public PharmacyDBEntities db = new PharmacyDBEntities();

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Customers.ToList();
            return View(models);
        }
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            customer.PassWord = Encode.EncodeMD5(customer.PassWord);
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Customers.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            string oldPass = db.Customers.Where(x => x.CustomerID == customer.CustomerID).Select(x => x.PassWord).FirstOrDefault().ToString();
            if (customer.PassWord != oldPass)
            {
                customer.PassWord = Encode.EncodeMD5(customer.PassWord);
            }
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Customers.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Customers.Find(id);
            return View(model);
        }
        [HttpGet]
        public JsonResult IsUserNameExist(string username)
        {
            return Json(!db.Customers.Any(x => x.UserName.ToLower() == username.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}