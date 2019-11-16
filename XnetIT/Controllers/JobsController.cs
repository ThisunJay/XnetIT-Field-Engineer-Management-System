using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XnetIT.Models;
using XnetIT.ViewModels;

namespace XnetIT.Controllers
{
    public class JobsController : Controller
    {
        private xnetDBEntities db = new xnetDBEntities();

        // GET: Jobs
        public ActionResult Index(string Search)
        {
            var jobs = from job in db.jobs select job;

            if (!String.IsNullOrEmpty(Search))
            {
                jobs = jobs.Where(e => e.title.Contains(Search));
            }

            return View(jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "job_id")] job jobToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                db.jobs.Add(jobToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
                
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(int id)
        {
            var toEdit = (from jo in db.jobs where jo.job_id == id select jo).First();

            return View(toEdit);
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        public ActionResult Edit(job toEdit)
        {
            try
            {
                var originalJob = (from jo in db.jobs where jo.job_id == toEdit.job_id select jo).First();

                if (!ModelState.IsValid)
                    return View(originalJob);

                if (toEdit.j_date is null) {
                    toEdit.j_date = DateTime.Now;
                }

                db.Entry(originalJob).CurrentValues.SetValues(toEdit);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            job jo = db.jobs.Find(id);

            if (jo == null)
            {
                return HttpNotFound();
            }

            return View(jo);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                job jo = db.jobs.Find(id);
                db.jobs.Remove(jo);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //Get
        public ActionResult RateJob()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RateJob([Bind(Exclude = "r_id")] job_ratings jobToRate) {

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var userID = Session["UserId"];
                //jobToRate.u_id = (int) userID;
                jobToRate.date_and_time = DateTime.Now;
                //jobToRate.j_id = Request.QueryString["parameter1"];
                db.job_ratings.Add(jobToRate);
                db.SaveChanges();

                return RedirectToAction("Index", "Jobs");
            }

        }

        public ActionResult ViewJobsWithRatings()
        {
            var jobsList = (from job in db.jobs select job).ToList();
            var ratingsList = (from rat in db.job_ratings select rat).ToList();

            var jobAndRatings = new JobAndRatingsViewModel();
            jobAndRatings.Jobs = jobsList;
            jobAndRatings.Ratings = ratingsList;

            return View(jobAndRatings);
        }

        public ActionResult RatingsView(int id)
        {
            // fetch reviews for `id`
            var reviews = (from rate in db.job_ratings where rate.j_id == id select rate).ToList();

            if (Request.IsAjaxRequest())
            {
                // return partial for AJAX requests
                return PartialView("_ReviewsPartial", reviews);
            }
            else
            {
                // return full view for regular requests
                return PartialView("_ReviewsPartial");
            }
        }

        public ActionResult ViewRatings(int id)
        {
            var rat = (from rati in db.job_ratings where rati.j_id == id select rati).ToList();
            return View(rat);
        }

        public ActionResult RateEngineer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RateEngineer([Bind(Exclude = "ra_id")]eng_ratings rateEng)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                db.eng_ratings.Add(rateEng);
                db.SaveChanges();
                return RedirectToAction("Index", "Engineers");

            }
            catch
            {
                return View();
            }
        }

        public ActionResult AssignEngineer()
        {
            assign_engineer engModel = new assign_engineer();

            engModel.engineerCollection = db.engineers.ToList<engineer>();

            return View(engModel);
        }

        [HttpPost]
        public ActionResult AssignEngineer([Bind(Exclude = "a_id")] assign_engineer engToAssign)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                db.assign_engineer.Add(engToAssign);
                db.SaveChanges();

                //var job = from j in db.jobs where
                var job = (from j in db.jobs where j.job_id == engToAssign.job_id select j).First();

                //db.Entry(originalJob).CurrentValues.SetValues(toEdit);
                //db.SaveChanges();

                //db.Entry(user).Property(x => x.Password).IsModified = true;
                //db.SaveChanges();
                job.j_status = "Assigned";
                db.Entry(job).Property(x => x.j_status).IsModified = true;
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

    }
}
