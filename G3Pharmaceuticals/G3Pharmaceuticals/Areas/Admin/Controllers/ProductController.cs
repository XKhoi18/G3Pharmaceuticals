using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public PharmacyDBEntities db = new PharmacyDBEntities();

        #region Index Page

        // GET: Admin/Product
        public ActionResult Index(string sortOrder, string searchString)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.PNameSortParm = String.IsNullOrEmpty(sortOrder) ? "pname_desc" : "Pname";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "Type";
            ViewBag.ManufacturerSortParm = sortOrder == "Manufacturer" ? "manufacturer_desc" : "Manufacturer";

            var products = from s in db.Products select s;
            switch (sortOrder)
            {
                case "pname_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "id_desc":
                    products = products.OrderByDescending(s => s.ProductID);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.DateOfManufacture);
                    break;
                case "type_desc":
                    products = products.OrderByDescending(s => s.TypeID);
                    break;
                case "manufacturer_desc":
                    products = products.OrderByDescending(s => s.ManufacturerID);
                    break;
                case "Pname":
                    products = products.OrderBy(s => s.ProductName);
                    break;
                case "Price":
                    products = products.OrderBy(s => s.Price);
                    break;
                case "Date":
                    products = products.OrderBy(s => s.DateOfManufacture);
                    break;
                case "Type":
                    products = products.OrderBy(s => s.TypeID);
                    break;
                case "Manufacturer":
                    products = products.OrderBy(s => s.ManufacturerID);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductID);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductID.ToString().Contains(searchString) || s.ProductName.Contains(searchString));
            }
            return View(products.ToList());
        }
        #endregion

        #region Create Page
        public ActionResult Create()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.TypeProduct = new SelectList(db.TypeProducts.ToList(), "typeID", "typeName");
            ViewBag.Manufacturer = new SelectList(db.Manufacturers.ToList(), "manufacturerID", "manufacturerName");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase Images)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (Images != null && Images.ContentLength > 0)
            {
                product.Images = Images.FileName;
                string urlImages = Server.MapPath("~/Content/images/products/" + product.Images);
                Images.SaveAs(urlImages);
            }
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listTypeProduct = new SelectList(db.TypeProducts, "typeID", "typeName", product.TypeID);
            ViewBag.listManufacturer = new SelectList(db.Manufacturers, "manufacturerID", "manufacturerName", product.ManufacturerID);
            return View(product);
        }
        #endregion

        #region Edit Page
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            var model = db.Products.Find(id);
            ViewBag.TypeProduct = new SelectList(db.TypeProducts.ToList(), "typeID", "typeName");
            ViewBag.Manufacturer = new SelectList(db.Manufacturers.ToList(), "manufacturerID", "manufacturerName");
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(Product product, HttpPostedFileBase newImages)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (newImages != null && newImages.ContentLength > 0)
            {
                product.Images = newImages.FileName;
                string urlImages = Server.MapPath("~/Content/images/products/" + product.Images);
                newImages.SaveAs(urlImages);
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            ViewBag.listTypeProduct = new SelectList(db.TypeProducts, "typeID", "typeName", product.TypeID);
            ViewBag.listManufacturer = new SelectList(db.Manufacturers, "manufacturerID", "manufacturerName", product.ManufacturerID);
            return View(product);
        }
        #endregion

        #region Delete Page
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Products.Find(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Product item = db.Products.Find(id);
            db.Products.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
        #endregion

        #region Details Page
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Products.Find(id);
            return View(model);
        }
        #endregion

    }
}