using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace G3Pharmaceuticals.Areas.Admin.Controllers
{
    public class AdminCandidateController : Controller
    {
        // GET: Admin/AdminCandidate
        public PharmacyDBEntities db = new PharmacyDBEntities();
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Candidates.ToList();
            return View(models);
        }
        public ActionResult Edit(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var model = db.Candidates.Where(p => p.CandidateID == id).FirstOrDefault();
            model.StaffID = (int)Session["admin"];
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Candidate candidate)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            if (ModelState.IsValid)
            {
                db.Entry(candidate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "AdminCandidate");
            }
            return View(candidate);
        }

        public ActionResult SendMail(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            Candidate candidate = db.Candidates.Where(x => x.CandidateID == id).SingleOrDefault();
            Email email = new Email();
            email.Address = candidate.Email;
            email.Tittle = "Greeting Candidate: " + candidate.FullName;
            email.Message = "Greeting Candidate: " + candidate.FullName;
            return View(email);
        }

        [HttpPost, ActionName("SendMail")]
        [ValidateInput(false)]
        public ActionResult SendMail(string Address, string Tittle, string Message)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Product");
            }
            string email = "testmvc5xuankhoi@gmail.com";
            string password = "testingmvc123456";
            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(Address));
            msg.Subject = Tittle;
            msg.Body = Message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
            return RedirectToAction("Index", "AdminCandidate");
        }
        public ActionResult Details(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var models = db.Candidates.Find(id);
            return View(models);
        }

        //public ActionResult Delete(int id)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    var model = db.Candidates.Find(id);
        //    return View(model);
        //}
        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    if (Session["admin"] == null)
        //    {
        //        return RedirectToAction("Index", "Login");
        //    }
        //    Candidate candidate = db.Candidates.Find(id);
        //    db.Candidates.Remove(candidate);
        //    db.SaveChanges();
        //    return RedirectToAction("Index", "OrderProduct");
        //}
    }
}