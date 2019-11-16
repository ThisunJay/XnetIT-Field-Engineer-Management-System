using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XnetIT.Models;

namespace XnetIT.Controllers
{
    public class HomeController : Controller
    {
        private xnetDBEntities db = new xnetDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Exclude = "u_id")] user userToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                db.users.Add(userToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(user u)
        {
            if (!ModelState.IsValid)
                return View();

            //var user = _db.users.Where(model => model.UserName_ == u.UserName_ && model.Password_ == u.Password_).FirstOrDefault();
            var users = db.users.Where(model => model.u_email == u.u_email && model.u_pass == u.u_pass).FirstOrDefault();
            //var originalEng = (from eng in db.engineers where eng.e_id == toEdit.e_id select eng).First();

            if (users is null)
                return Content("Invaid Login");

            var us = (from ab in db.users where ab.u_id == users.u_id select ab).First();

            

            if (users is null)
            {
                return View();
            }
            else if (us.u_type == "Admin")
            {
                Session["UserID"] = us.u_id.ToString();
                Session["UserName"] = us.u_email.ToString();
                Session["UserType"] = us.u_type.ToString();
                return RedirectToAction("../Jobs/Index");
            }
            else if (us.u_type == "IT Coordinator")
            {
                Session["UserID"] = us.u_id.ToString();
                Session["UserName"] = us.u_type.ToString();
                Session["UserType"] = us.u_type.ToString();
                return RedirectToAction("../Engineers/Index");
            }
            else {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}