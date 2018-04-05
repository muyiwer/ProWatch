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
    public class UserProjectsController : Controller
    {
        private ProWatchEntities db = new ProWatchEntities();

        // GET: UserProjects
        public ActionResult Index()
        {
            var userProjects = db.UserProjects.Include(u => u.project).Include(u => u.proUser);
            return View(userProjects.ToList());
        }

        // GET: UserProjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProject userProject = db.UserProjects.Find(id);
            if (userProject == null)
            {
                return HttpNotFound();
            }
            return View(userProject);
        }

        // GET: UserProjects/Create
        public ActionResult Create()
        {
            ViewBag.projectID = new SelectList(db.projects, "projectID", "projectName");
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname");
            return View();
        }

        // POST: UserProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "upID,createdBy,projectID,userID")] UserProject userProject)
        {
            if (ModelState.IsValid)
            {
                db.UserProjects.Add(userProject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.projectID = new SelectList(db.projects, "projectID", "projectName", userProject.projectID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", userProject.userID);
            return View(userProject);
        }

        // GET: UserProjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProject userProject = db.UserProjects.Find(id);
            if (userProject == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectID = new SelectList(db.projects, "projectID", "projectName", userProject.projectID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", userProject.userID);
            return View(userProject);
        }

        // POST: UserProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "upID,createdBy,projectID,userID")] UserProject userProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectID = new SelectList(db.projects, "projectID", "projectName", userProject.projectID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", userProject.userID);
            return View(userProject);
        }

        // GET: UserProjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserProject userProject = db.UserProjects.Find(id);
            if (userProject == null)
            {
                return HttpNotFound();
            }
            return View(userProject);
        }

        // POST: UserProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProject userProject = db.UserProjects.Find(id);
            db.UserProjects.Remove(userProject);
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
