using Microsoft.Reporting.WebForms;
using System;
using System.Linq;
using System.Web.Mvc;
using XnetIT.Models;
using XnetIT.ViewModels;

namespace XnetIT.Controllers
{
    public class EngineersController : Controller
    {
        private xnetDBEntities db = new xnetDBEntities();

        // GET: Engineers
        public ActionResult Index(string Search, string Address, string Skills, int? i)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var engineers = from eng in db.engineers select eng;

                if (!String.IsNullOrEmpty(Search))
                {
                    engineers = engineers.Where(e => e.e_name.Contains(Search));
                }
                else if (!String.IsNullOrEmpty(Address))
                {
                    engineers = engineers.Where(e => e.e_address.Contains(Address));
                }
                else if (!String.IsNullOrEmpty(Skills))
                {
                    engineers = engineers.Where(e => e.skills.Contains(Skills));
                }

                return View(engineers.ToList());
            }

        }

        // GET: Engineers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Engineers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Engineers/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "e_id")] engineer engineerToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                db.engineers.Add(engineerToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Engineers/Edit/5
        public ActionResult Edit(int id)
        {
            var toEdit = (from eng in db.engineers where eng.e_id == id select eng).First(); 

            return View(toEdit);
        }

        // POST: Engineers/Edit/5
        [HttpPost]
        public ActionResult Edit(engineer toEdit)
        {
            try
            {
                var originalEng = (from eng in db.engineers where eng.e_id == toEdit.e_id select eng).First();

                if (!ModelState.IsValid)
                    return View(originalEng);

                db.Entry(originalEng).CurrentValues.SetValues(toEdit);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Engineers/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            engineer eng = db.engineers.Find(id);

            if (eng == null) {
                return HttpNotFound();
            }
            return View(eng);
        }

        // POST: Engineers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                engineer eng = db.engineers.Find(id);
                db.engineers.Remove(eng);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

        public ActionResult ViewRatings(int id)
        {
            EngineerAndRatings EngAndRat = new EngineerAndRatings();
            EngAndRat.Engineer = (from eng in db.engineers where eng.e_id == id select eng).First();
            EngAndRat.Ratings = (from rat in db.eng_ratings where rat.e_id == id select rat).ToList();

            return View(EngAndRat);
        }

        public ActionResult Reports(string ReportType)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/EngineerReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "EngineerDataSet";
            reportDataSource.Value = db.engineers.ToList();
            localReport.DataSources.Add(reportDataSource);
            String reportType = ReportType;
            String mimeType;
            String encoding;
            String fileNameExtension;

            if (reportType == "PDF")
            {
                fileNameExtension = "PDF";
            }
            else if (reportType == "Excel")
            {
                fileNameExtension = "xlsx";
            }

            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localReport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension,
                out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment:filename= eng_report." + fileNameExtension);
            return File(renderedByte, fileNameExtension);
            
        }

        public ActionResult ViewJobAssigns()
        {
            var jobs = (from j in db.assign_engineer select j).ToList();

            return View(jobs);
        }
    }
}
