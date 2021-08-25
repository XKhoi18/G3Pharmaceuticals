using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G3Pharmaceuticals.Models;
using System.Data.Entity;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class TypeProductController : Controller
    {
        // GET: Admin/TypeProduct
        public PharmacyDBEntities db = new PharmacyDBEntities();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.TypeProducts.ToList();
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
        public ActionResult Create(TypeProduct typeProduct, HttpPostedFileBase Images)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (Images != null && Images.ContentLength > 0)
            {
                typeProduct.images = Images.FileName;
                string urlImages = Server.MapPath("~/Content/images/typeproduct/" + typeProduct.images);
                Images.SaveAs(urlImages);
            }
            if (ModelState.IsValid)
            {
                db.TypeProducts.Add(typeProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeProduct);
        }
        public ActionResult Edit(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.TypeProducts.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(TypeProduct typeProduct, HttpPostedFileBase newImages)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (newImages != null && newImages.ContentLength > 0)
            {
                typeProduct.images = newImages.FileName;
                string urlImages = Server.MapPath("~/Content/images/typeproduct/" + typeProduct.images);
                newImages.SaveAs(urlImages);
            }
            if (ModelState.IsValid)
            {
                db.Entry(typeProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeProduct);
        }
        public ActionResult Details(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.TypeProducts.Find(id);
            return View(model);
        }
        public ActionResult Delete(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.TypeProducts.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TypeProduct item = db.TypeProducts.Find(id);
            db.TypeProducts.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "TypeProduct");
        }
        [HttpGet]
        public JsonResult IsIDNameExist(string typeID)
        {
            return Json(!db.TypeProducts.Any(x => x.typeID.ToLower() == typeID.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}