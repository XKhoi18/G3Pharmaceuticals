using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class NationController : Controller
    {
        // GET: Admin/Nation
        public PharmacyDBEntities db = new PharmacyDBEntities();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Nations.ToList();
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
        public ActionResult Create(Nation nation)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Nations.Add(nation);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Nation");
        }
        public ActionResult Edit(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Nations.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Nation nation)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(nation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Nation");
            }
            return View(nation);
        }
        public ActionResult Delete(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Nations.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Nation item = db.Nations.Find(id);
            db.Nations.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Nation");
        }
        public ActionResult Details(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Nations.Find(id);
            return View(model);
        }
        [HttpGet]
        public JsonResult IsIDNameExist(string nationID)
        {
            return Json(!db.Nations.Any(x => x.nationID.ToLower() == nationID.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}