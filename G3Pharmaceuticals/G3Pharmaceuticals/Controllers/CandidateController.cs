using G3Pharmaceuticals.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace G3Pharmaceuticals.Controllers
{
    public class CandidateController : Controller
    {
        #region Variables 
        // GET: Candidate
        public PharmacyDBEntities db = new PharmacyDBEntities();

        #endregion

        #region Index

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Register

        public ActionResult Register()
        {
            ViewBag.Gender = new SelectList("Male", "Female");
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Register(Candidate candidate)
        {
            //Encode
            candidate.Password = Encode.EncodeMD5(candidate.Password);
            candidate.CandidateID = db.Candidates.OrderByDescending(p => p.CandidateID).First().CandidateID + 1;
            if (ModelState.IsValid)
            {
                db.Candidates.Add(candidate);
                db.SaveChanges();
                return RedirectToAction("Index", "Candidate");
            }
            return View(candidate);
        }

        #endregion

        #region Log in

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            //Encode
            password = Encode.EncodeMD5(password);
            Candidate rs = db.Candidates.Where(p => p.Username.Equals(username) && p.Password.Equals(password)).FirstOrDefault();
            if (rs != null)
            {
                Session["candidate"] = rs.CandidateID;
                var can = db.Candidates.Find(rs.CandidateID);                
                return RedirectToAction("Profile", "Candidate", new { id = rs.CandidateID });
            }
            else
            {
                return RedirectToAction("LogIn", "Candidate");
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

        #region Profile

        public new ActionResult Profile(int id)
        {
            Candidate candidate = db.Candidates.Find(id);
            return View(candidate);
        }

        #endregion

        #region Update

        public ActionResult Update(int id)
        {
            if (Session["candidate"] == null)
            {
                return RedirectToAction("LogIn", "Candidate");
            }           
            var model = db.Candidates.Find(id);
            Session["cv"] = model.CV;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Candidate candidate, HttpPostedFileBase cvUpload)
        {
            if (Session["candidate"] == null)
            {
                return RedirectToAction("LogIn", "Candidate");
            }            
            if (ModelState.IsValid)
            {
                candidate.Password = Encode.EncodeMD5(candidate.Password);
                if (cvUpload != null)
                {                    
                    string filename = Path.GetFileName(cvUpload.FileName);
                    string extension = Path.GetExtension(cvUpload.FileName);
                    if (extension == "pdf" || extension == "PDF")
                    {
                        string path = Path.Combine(Server.MapPath("~/Content/CV Files"), filename);
                        string oldCV = Request.MapPath(Session["cv"].ToString());
                        candidate.CV = filename;
                        cvUpload.SaveAs(path);
                        db.Entry(candidate).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {
                            cvUpload.SaveAs(path);
                            if (System.IO.File.Exists(oldCV))
                            {
                                System.IO.File.Delete(oldCV);
                            }
                            return RedirectToAction("Profile", "Candidate", new { id = candidate.CandidateID });
                        }
                    }
                    else
                    {
                        TempData["notice"] = "Select pdf file only.";
                    }
                }
                else
                {
                    candidate.CV = Session["cv"].ToString();
                    db.Entry(candidate).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        return RedirectToAction("Profile", "Candidate", new { id = candidate.CandidateID });
                    }
                }
            }
            return View();
        }

        #endregion

        #region Contact Us 

        public ActionResult ContactUs()
        {
            return View();
        }

        #endregion

        #region join Us 

        public ActionResult JoinUs()
        {
            return View();
        }

        #endregion

        
    }
}