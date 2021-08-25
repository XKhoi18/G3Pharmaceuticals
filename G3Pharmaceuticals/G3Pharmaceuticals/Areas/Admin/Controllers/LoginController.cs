using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G3Pharmaceuticals.Models;
using System.Web.Security;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public PharmacyDBEntities db = new PharmacyDBEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            password = Encode.EncodeMD5(password);
            var result = db.Staffs.Where(p => p.Username.Equals(username) && p.Password.Equals(password)).FirstOrDefault();
            if (result != null)
            {
                Session["admin"] = result.StaffID;
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        #region Logout Function

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Login");
        }

        #endregion
    }
}