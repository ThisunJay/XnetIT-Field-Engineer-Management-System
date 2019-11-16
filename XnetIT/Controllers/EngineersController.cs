using System;
using System.Linq;
using System.Web.Mvc;
using XnetIT.Models;

namespace XnetIT.Controllers
{
    public class EngineersController : Controller
    {
        private xnetDBEntities db = new xnetDBEntities();

        // GET: Engineers
        public ActionResult Index(string Search, int? i)
        {
            
            var engineers = from eng in db.engineers select eng;

            if (!String.IsNullOrEmpty(Search))
            {
                engineers = engineers.Where(e => e.e_name.Contains(Search));
            }

            return View(engineers.ToList());
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

        public ActionResult ViewRatings(int id)
        {
            //var engRatings = from eng in db.eng_ratings where eng.u_id

            return View();
        }
        
    }
}
