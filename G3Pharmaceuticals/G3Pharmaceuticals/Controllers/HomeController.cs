using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G3Pharmaceuticals.Models;
using PagedList;
using System.Web.Security;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace G3Pharmaceuticals.Controllers
{
    public class HomeController : Controller
    {
        #region Variables

        public PharmacyDBEntities db = new PharmacyDBEntities();
        public IEnumerable<Product> AllListPaging(int page, int pageSize)
        {
            return db.Products.OrderBy(x => x.DateOfManufacture).ToPagedList(page, pageSize);
        }
        public IEnumerable<Product> AllListPagingByType(int page, int pageSize, string typeID)
        {
            return db.Products.OrderBy(x => x.DateOfManufacture).Where(x => x.TypeID.Equals(typeID)).ToPagedList(page, pageSize);
        }
        public IEnumerable<Product> AllListPagingByManu(int page, int pageSize, string manuID)
        {
            return db.Products.OrderBy(x => x.DateOfManufacture).Where(x => x.ManufacturerID.Equals(manuID)).ToPagedList(page, pageSize);
        }


        #endregion

        #region Index

        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var products = db.Products.ToList();
            return View(products);
        }

        #endregion

        #region Type Page & Details

        public ActionResult TypePage(string typeID, int page = 1, int pageSize = 9)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            ViewBag.menu2 = db.Manufacturers.ToList();
            if (typeID == null || typeID == "0")
            {
                var products = AllListPaging(page, pageSize);
                ViewBag.typeID = null;
                return View(products);
            }
            else
            {
                var products = AllListPagingByType(page, pageSize, typeID);
                ViewBag.typeID = typeID;
                return View(products);
            }
        }

        public ActionResult BrandPage(string manuID, int page = 1, int pageSize = 9)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            ViewBag.menu2 = db.Manufacturers.ToList();
            if (manuID == null || manuID == "0")
            {
                var products = AllListPaging(page, pageSize);
                ViewBag.manuID = null;
                return View(products);
            }
            else
            {
                var products = AllListPagingByManu(page, pageSize, manuID);
                ViewBag.manuID = manuID;
                return View(products);
            }
        }

        public ActionResult Details(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            Product product = db.Products.Find(id);
            return View(product);
        }

        #endregion

        #region About & History

        public ActionResult About()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }

        public ActionResult History()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }
        #endregion

        #region Contact

        public ActionResult Contact()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            ViewBag.Message = "Your contact page.";
            return View();
        }

        #endregion

        #region Log In

        public ActionResult LogIn()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            password = Encode.EncodeMD5(password);
            Customer rs = db.Customers.Where(p => p.UserName.Equals(username) && p.PassWord.Equals(password)).FirstOrDefault();
            if (rs != null)
            {
                Session["customer"] = rs.CustomerID;
                var cus = db.Customers.Find(rs.CustomerID);
                ViewBag.CustomerName = cus.CustomerName.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
            }
        }

        #endregion

        #region Logout Function

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            //Encode
            customer.PassWord = Encode.EncodeMD5(customer.PassWord);
            customer.CustomerID = db.Customers.OrderByDescending(p => p.CustomerID).First().CustomerID + 1;
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Login", "Home");
            }
            return View(customer);
        }

        #endregion

        #region Purchase History & Details

        public ActionResult PurchaseHistory()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            if (Session["customer"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            int customerid = (int)Session["customer"];
            var models = db.Orderproes.Where(p => p.CustomerID == customerid).ToList();
            return View(models);
        }
        public ActionResult DetailOrder(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            if (Session["customer"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            var models = db.OrderDetails.Where(p => p.OrderID == id).ToList();
            return View(models);
        }

        #endregion

        #region Quote

        public ActionResult Quote()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            ViewBag.quote = db.Customers.ToList();
            return View();
        }

        public ActionResult EditQuote()
        {           
            ViewBag.menu = db.TypeProducts.ToList();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuote( Customer customer)
        {
            if (Session["customer"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            var session = db.Customers.Find(Session["customer"]);
                if (ModelState.IsValid)
                {
                    try
                    {
                    customer.UserQuote = session.UserQuote;
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();                  
                    }
                    catch (Exception ex) { }    
                }
            return RedirectToAction("Quote", "Home");
        }
        #endregion
    }
}