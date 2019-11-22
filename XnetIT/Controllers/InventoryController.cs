using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XnetIT.Models;

namespace XnetIT.Controllers
{
    public class InventoryController : Controller
    {
        private xnetDBEntities db = new xnetDBEntities();

        // GET: Inventory
        public ActionResult Index()
        {
            var items = from item in db.items select item;

            //if (!String.IsNullOrEmpty(Search))
            //{
            //    engineers = engineers.Where(e => e.e_name.Contains(Search));
            //}

            return View(items.ToList());
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "i_id")] item itemToCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                db.items.Add(itemToCreate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            var toEdit = (from ite in db.items where ite.i_id == id select ite).First();

            return View(toEdit);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        public ActionResult Edit(item toEdit)
        {
            try
            {
                var originalItem = (from ite in db.items where ite.i_id == toEdit.i_id select ite).First();

                if (!ModelState.IsValid)
                    return View(originalItem);

                db.Entry(originalItem).CurrentValues.SetValues(toEdit);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int? id)
        {
            item ite = db.items.Find(id);

            if (ite == null)
            {
                return HttpNotFound();
            }
            return View(ite);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                item ite = db.items.Find(id);
                db.items.Remove(ite);
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
