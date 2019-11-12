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
            var user = db.users.Where(model => model.u_email == u.u_email && model.u_pass == u.u_pass).FirstOrDefault();

            if (user.u_type == "Admin")
            {
                Session["UserID"] = u.u_id.ToString();
                Session["UserName"] = u.u_email.ToString();
                return RedirectToAction("../Home/Index");
            }
            else if (user.u_type == "IT Coordinator")
            {
                Session["UserID"] = u.u_id.ToString();
                Session["UserName"] = u.u_email.ToString();
                return RedirectToAction("../Home/Index");
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