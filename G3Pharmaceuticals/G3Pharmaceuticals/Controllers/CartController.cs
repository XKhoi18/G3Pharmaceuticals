using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Controllers
{
    public class CartController : Controller
    {
        public PharmacyDBEntities db = new PharmacyDBEntities();
        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.menu = db.TypeProducts.ToList();
            List<CartItem> list = (List<CartItem>)Session["cartSession"];
            return View(list);
        }
        public ActionResult AddItem(int id)
        {
            ViewBag.menu = db.TypeProducts.ToList();
            var cart = Session["cartSession"];
            List<CartItem> list = new List<CartItem>();
            // cart is null
            if (cart == null)
            {
                Product product = db.Products.Where(x => x.ProductID == id).SingleOrDefault();
                CartItem item = new CartItem();
                item.product = product;
                item.quantity = 1;
                list.Add(item);
                Session["cartSession"] = list;
            }
            else //cart != null
            {
                list = (List<CartItem>)Session["cartSession"];
                if (list.Exists(x => x.product.ProductID == id)) //exist
                {
                    foreach (var a in list)
                    {
                        if (a.product.ProductID == id)
                        {
                            a.quantity = a.quantity + 1;
                        }
                        Session["cartSession"] = list;
                    }
                }
                else // not exist
                {
                    Product product = db.Products.Where(x => x.ProductID == id).SingleOrDefault();
                    CartItem item = new CartItem();
                    item.product = product;
                    item.quantity = 1;
                    list.Add(item);
                    Session["cartSession"] = list;
                }
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult DeleteItem(int id)
        {
            List<CartItem> list = (List<CartItem>)Session["cartSession"];
            CartItem item = list.Where(p => p.product.ProductID.Equals(id)).FirstOrDefault();
            list.Remove(item);
            Session["cartSession"] = list;
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult UpdateItem(int id, int quantity) //match id on index Update -+
        {
            List<CartItem> list = (List<CartItem>)Session["cartSession"];
            if (quantity != 0)
            {
                list.Where(p => p.product.ProductID.Equals(id)).FirstOrDefault().quantity = quantity;
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult Order()
        {
            if (Session["customer"] == null)
            {
                return RedirectToAction("LogIn", "Home");
            }
            else // Order
            {
                Orderpro od = new Orderpro();
                od.OrderID = db.Orderproes.OrderByDescending(p => p.OrderID).First().OrderID + 1;
                od.OrderDate = DateTime.Now;
                od.CustomerID = (int)Session["customer"];
                od.Status = 0;
                db.Orderproes.Add(od);
                db.SaveChanges();
                List<CartItem> list = (List<CartItem>)Session["cartSession"];
                string message = "<p style='font-size: 16px;'>Your recent purchase from HighWind has been completed.</p>";
                message += "<p style='padding-bottom: 20px; font-size: 16px;'><strong>HERE'S WHAT YOU ORDERED: </strong></p>";
                message += "<table class='table'>";
                message += "<tr>";
                message += "<th width='500px' style='text-align:center; font-size: 14px;'>Product Name</th>";
                message += "<th width='200px' style='text-align:center; font-size: 14px;'>Price</th>";
                message += "<th width='200px' style='text-align:center; font-size: 14px;'>Quantity</th>";
                message += "<th width='200px' style='text-align:center; font-size: 14px;'>Total</th>";
                message += "</tr>";
                float sum = 0;
                foreach (CartItem item in list)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderID = od.OrderID;
                    orderDetail.ProductID = item.product.ProductID;
                    orderDetail.Quantity = item.quantity;
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();
                    message += "<tr>";
                    message += "<td style='text-align:left; font-size: 14px;'>" + item.product.ProductName + "</td>";
                    message += "<td style='text-align:center; font-size: 14px;'>" + item.product.Price + "</td>";
                    message += "<td style='text-align:center; font-size: 14px;'>" + item.quantity + "</td>";
                    message += "<td style='text-align:center; font-size: 14px;'><b>" + String.Format("{0:0,0 USD}", item.quantity * item.product.Price) + "</b></td>";
                    sum += (float)(item.quantity * item.product.Price);
                    message += "</tr>";
                }
                message += "<tr>";
                message += "<th colspan='3' style='text-align:right; font-size: 14px;'>Total Money: </th>";
                message += "<th style='text-align:center; font-size: 14px;'>" + String.Format("{0:0,0 USD}", sum) + "</th>";
                message += "</tr>";
                message += "</table>";
                message += "<p style='padding-top: 20px; font-size: 16px; color: black;'>This email message will serve as your receipt. You can also view your Purchase History at any time.</p>";
                message += "<p style='font-size: 16px; color: black;'>Refunds and/or returns may be granted for many products on HighWind.</p>";
                message += "<p style='padding-top: 20px; font-size: 16px; color: black;'><strong>Cheers, The HighWind Team!</strong></p>";
                //send to customer 
                Customer ct = db.Customers.Find(od.CustomerID);
                SendEmail(ct.Email, "Thank you for your recent purchase on HighWind www.highwind.com", message);
                Session["cartSession"] = null;
                return RedirectToAction("Index", "Cart");
            }
        }
        public void SendEmail(string address, string subject, string message)
        {
            string email = "testmvc5xuankhoi@gmail.com";
            string password = "testingmvc123456";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
        }
    }
}