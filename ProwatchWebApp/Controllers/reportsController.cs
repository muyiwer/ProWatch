using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProwatchWebApp.Models;

namespace ProwatchWebApp.Controllers
{
    public class reportsController : Controller
    {
        private ProWatchEntities1 db = new ProWatchEntities1();

        // GET: reports
        public ActionResult Index()
        {
            var reports = db.reports.Include(r => r.projectStatu).Include(r => r.proUser).Include(r => r.task);
            return View(reports.ToList());
        }

        // GET: reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: reports/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "taskName,reportDescription")] report report)
        {
            if (ModelState.IsValid)
            {
				var user = User.Identity.Name;
				var userid = db.proUsers.Where(y => y.email == user).Select(y => y.userID).FirstOrDefault();
				report.userID = userid;
				var taskID = db.tasks.Where(y => y.taskName == report.taskName).Select(y => y.taskID).FirstOrDefault();
				report.taskID = taskID;
				db.reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(report);
        }

        // GET: reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectStatusID = new SelectList(db.projectStatus, "projectStatusID", "projectStatusName", report.projectStatusID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", report.userID);
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", report.taskID);
            return View(report);
        }

        // POST: reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "reportID,taskName,reportDescription,createdBy,userID,projectStatusID,taskID")] report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectStatusID = new SelectList(db.projectStatus, "projectStatusID", "projectStatusName", report.projectStatusID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", report.userID);
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", report.taskID);
            return View(report);
        }

        // GET: reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            report report = db.reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            report report = db.reports.Find(id);
            db.reports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
