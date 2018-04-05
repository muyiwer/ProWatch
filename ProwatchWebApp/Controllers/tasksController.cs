using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProwatchWebApp.Models;
using PagedList;
namespace ProwatchWebApp.Controllers
{
    public class tasksController : Controller
    {
        private ProWatchEntities db = new ProWatchEntities();

        // GET: tasks
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var students = from s in db.tasks
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.taskName.Contains(searchString)
                                       || s.projectStatu.projectStatusName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.taskName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.projectStatu.projectStatusName);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.projectStatu.projectStatusName);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.taskName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));

        }

        // GET: tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: tasks/Create
        public ActionResult Create()
        {
            var user = User.Identity.Name;
            ViewBag.Name = user;
            return View();
        }

        // POST: tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(task task, string projectName)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.Name;              
                DateTime dt = DateTime.Now.Date;
                var projectID = db.projects.Where(y => y.projectName == projectName).Select(y => y.projectID).FirstOrDefault();
                task.projectID = projectID;
                var userID = db.proUsers.Where(y => y.email == user).Select(y => y.userID).FirstOrDefault();
                task.userID = userID;
                task.dateCreated = dt;
                db.tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(task);
        }

        [HttpPost]
        public ActionResult AutoComplete(proUser user)
        {
            try
            {
                string term = user.email;
                if (term.Length > 0)
                {
                    return Json(db.proUsers.Where(x => x.email.StartsWith(term)).Select(y => y.email).ToArray(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("No record", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message + ex.StackTrace, JsonRequestBehavior.AllowGet);
            }
           
        }

        // GET: tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectStatusID = new SelectList(db.projectStatus, "projectStatusID", "projectStatusName", task.projectStatusID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", task.userID);
            return View(task);
        }

        // POST: tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "taskID,taskName,observedBy,deadlineDate,dateCreated,dateModified,createdBy,userID,projectStatusID,assignTo")] task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectStatusID = new SelectList(db.projectStatus, "projectStatusID", "projectStatusName", task.projectStatusID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", task.userID);
            return View(task);
        }

        // GET: tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task task = db.tasks.Find(id);
            db.tasks.Remove(task);
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
