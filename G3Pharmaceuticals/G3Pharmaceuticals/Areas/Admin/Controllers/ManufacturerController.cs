using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        // GET: Admin/Manufacturer
        public PharmacyDBEntities db = new PharmacyDBEntities();

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Manufacturers.ToList();
            return View(models);
        }
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.nations = new SelectList(db.Nations.ToList(), "nationID", "nationName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Manufacturer manufacturer)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Manufacturers.Add(manufacturer);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Manufacturer");
        }
        public ActionResult Edit(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Manufacturers.Find(id);
            ViewBag.nations = new SelectList(db.Nations.ToList(), "nationID", "nationName", model.nation);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Manufacturer manufacturer)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(manufacturer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Manufacturer");
            }
            return View(manufacturer);
        }
        public ActionResult Delete(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Manufacturers.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Manufacturer item = db.Manufacturers.Find(id);
            db.Manufacturers.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Manufacturer");
        }
        public ActionResult Details(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Manufacturers.Find(id);
            return View(model);
        }
        [HttpGet]
        public JsonResult IsIDNameExist(string manufacturerID)
        {
            return Json(!db.Manufacturers.Any(x => x.manufacturerID.ToLower() == manufacturerID.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}