using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class OrderProductController : Controller
    {
        public PharmacyDBEntities db = new PharmacyDBEntities();
        // GET: Admin/OrderProduct
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Orderproes.ToList();
            return View(models);
        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Orderproes.Where(p => p.OrderID == id).FirstOrDefault();
            model.StaffID = (int)Session["admin"];
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Orderpro orderpro)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            if (ModelState.IsValid)
            {
                db.Entry(orderpro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "OrderProduct");
            }
            return View(orderpro);
        }
        public ActionResult DetailOrder(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.OrderDetails.Where(p => p.OrderID == id).ToList();
            return View(models);
        }
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Orderproes.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Orderpro item = db.Orderproes.Find(id);
            db.Orderproes.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "OrderProduct");
        }
    }
}