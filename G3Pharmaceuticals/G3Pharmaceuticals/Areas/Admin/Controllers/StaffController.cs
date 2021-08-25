using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class StaffController : Controller
    {
        // GET: Admin/Staff
        public PharmacyDBEntities db = new PharmacyDBEntities();

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Staffs.ToList();
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
        public ActionResult Create(Staff staff, HttpPostedFileBase Images)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (Images != null && Images.ContentLength > 0)
            {
                staff.Images = Images.FileName;
                string urlImages = Server.MapPath("~/Content/images/id photo/" + staff.Images);
                Images.SaveAs(urlImages);
            }
            staff.Password = Encode.EncodeMD5(staff.Password);
            if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Staffs.Find(id);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Staff staff, HttpPostedFileBase newImages)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (newImages != null && newImages.ContentLength > 0)
            {
                staff.Images = newImages.FileName;
                string urlImages = Server.MapPath("~/Content/images/id photo/" + staff.Images);
                newImages.SaveAs(urlImages);
            }
            string oldPass = db.Staffs.Where(x => x.StaffID == staff.StaffID).Select(x => x.Password).FirstOrDefault().ToString();
            if (staff.Password != oldPass)
            {
                staff.Password = Encode.EncodeMD5(staff.Password);
            }
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Staffs.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Staffs.Find(id);
            return View(model);
        }
        [HttpGet]
        public JsonResult IsUserNameExist(string username)
        {
            return Json(!db.Staffs.Any(x => x.Username.ToLower() == username.ToLower()), JsonRequestBehavior.AllowGet);
        }
    }
}