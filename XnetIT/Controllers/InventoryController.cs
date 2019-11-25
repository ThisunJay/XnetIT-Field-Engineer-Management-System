using Microsoft.Reporting.WebForms;
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
        public ActionResult Index(string State, string ModelNumber, string SerialNumber, string Availability)
        {

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var items = (from item in db.items select item);

                if (!String.IsNullOrEmpty(State))
                {
                    items = items.Where(i => i.i_state.Contains(State));
                }
                else if (!String.IsNullOrEmpty(ModelNumber))
                {
                    items = items.Where(i => i.model_number.Contains(ModelNumber));
                }
                else if (!String.IsNullOrEmpty(SerialNumber))
                {
                    items = items.Where(i => i.serial_number.Contains(SerialNumber));
                }
                else if (!String.IsNullOrEmpty(Availability))
                {
                    items = items.Where(i => i.i_availability.Equals(Availability));
                }
                //if (!String.IsNullOrEmpty(Search))
                //{
                //    engineers = engineers.Where(e => e.e_name.Contains(Search));
                //}
                //return Json(new { data = items }, JsonRequestBehavior.AllowGet);
                return View(items.ToList());
            }

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

                itemToCreate.i_status = "In Warehouse";
                itemToCreate.i_availability = "Available";

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

        public ActionResult Reports(string ReportType)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/InventoryReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "InventoryDataSet";
            reportDataSource.Value = db.items.ToList();
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
            Response.AddHeader("content-disposition", "attachment:filename= items_report." + fileNameExtension);
            return File(renderedByte, fileNameExtension);

        }
    }
}
