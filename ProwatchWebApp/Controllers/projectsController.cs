using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProwatchWebApp.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using PagedList;

namespace ProwatchWebApp.Controllers
{
    public class projectsController : Controller
    {
        private ProWatchEntities1 db = new ProWatchEntities1();

        [Authorize]
        public ActionResult Dashboard(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var email = User.Identity.Name;
            var firstname = db.proUsers.Where(y => y.email == email).Select(y => y.firstname).FirstOrDefault();
            ViewBag.firstname = firstname;

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
            var students = from s in db.projects
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.projectName.Contains(searchString)
                                       || s.projectMaterial.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.projectName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.projectStatu.projectStatusName);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.projectStatu.projectStatusName);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.projectName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);


			var userId = db.proUsers.Where(y => y.email == email).Select(y => y.userID).FirstOrDefault();
			
            var allProject = db.projects.Where(y=>y.userID==userId).Select(y=>y.projectStatusID).Count();
            ViewBag.allProject = allProject;

            var projectStatusID = db.projectStatus.Where(y => y.projectStatusID == 1).Select(y => y.projectStatusID).FirstOrDefault();
            var finishedProject = db.projects.Where(y => y.projectStatusID == projectStatusID).Count();
            ViewBag.finishedProject = finishedProject;

            var projectStatusID2 = db.projectStatus.Where(y => y.projectStatusID == 2).Select(y => y.projectStatusID).FirstOrDefault();
            var ongoingProject = db.projects.Where(y => y.projectStatusID == projectStatusID2).Count();
            ViewBag.ongoingProject = ongoingProject;
            var report = db.reports.Where(y=>y.userID==userId).Count();
            ViewBag.report = report;
			var pendingTask = db.UserProjects.Where(y => y.userID == userId).Select(y => y.projectID).FirstOrDefault();
			var task = db.tasks.Where(y => y.projectID == pendingTask).Select(y => y.projectStatusID).Count();
			var notification = db.tasks.Where(y => y.assignTo == email).Count();
            ViewBag.Notification =notification;
			ViewBag.Task = task;
            return View(students.ToPagedList(pageNumber, pageSize));
           

        }

        // GET: projects
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.Completed = "Completed";
            ViewBag.Completed = "Not started";
            ViewBag.InProgress = "In-progress";
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
            var students = from s in db.projects
            select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.projectName.Contains(searchString)
                                       || s.projectMaterial.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.projectName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.projectStatu.projectStatusName);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.projectStatu.projectStatusName);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.projectName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));            
            
        }
        [HttpGet]
        public ActionResult AddUserToProject(string sortOrder, string searchString, string currentFilter, int? page)
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
            var students = from s in db.UserProjects
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.project.projectName.Contains(searchString)
                                       || s.proUser.email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.project.projectID);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.proUser.email);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.proUser.email);
                    break;
                default:  // Name ascending 
                    students = students.OrderBy(s => s.project.projectID);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
            // string projects = Request.Form["projectName"];
            // int id = Convert.ToInt16(Request.Form[""]);

            // var emailByProject = db.UserProjects.Select(y => new { y.proUser.email });
            // var project = db.projects.Where(y => y.projectName == projects).Select(y => y.projectName).FirstOrDefault();
            // ViewBag.ProjectName = project;
            //eturn View(emailByProject.ToList());
        }

             [HttpPost]
            public ActionResult AddUserToProject(UserProject user, string projectName, string email)
        {
            if (ModelState.IsValid)
            {
               
                
                var projectID = db.projects.Where(y => y.projectName == projectName).Select(y => y.projectID).FirstOrDefault();
                var userID= db.proUsers.Where(y => y.email == email).Select(y => y.userID).FirstOrDefault();
                user.userID = userID;
                user.projectID = projectID;
                user.createdBy = email;
                db.UserProjects.Add(user);
                db.SaveChanges();
               
                return RedirectToAction("AddUserToProject", "projects");
            }

            return View();
        }

        // GET: projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: projects/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create(string projectStatus, project project, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
              
               
                var user = User.Identity.Name;
                var userID = db.proUsers.Where(y => y.email == user).Select(y => y.userID).FirstOrDefault();
                var projectStatusID = db.projectStatus.Where(y => y.projectStatusName == projectStatus).Select(y => y.projectStatusID).FirstOrDefault();
                DateTime dt = DateTime.Now.Date;
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/File/Image"),
                                               Path.GetFileName(file.FileName));                  
                    file.SaveAs(path);
                    project.fileUrl = path; 
                    project.projectStatusID = projectStatusID;
                    project.userID = userID;
                   // project.dateCreated = dt;
                    db.projects.Add(project);
                    
                    
                    db.SaveChanges();
                    
                    return RedirectToAction("AddUserToProject");
                }
                   
            }
           
            return View(project);
        }

            [HttpGet]
            public ActionResult UploadFile()
        {
            return View();
        }
       [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/File/Image"),
                                               Path.GetFileName(file.FileName));
                    string dpath = Path.Combine(Server.MapPath("~/File/CrunchedImage"),
                                               Path.GetExtension(file.FileName));
                    file.SaveAs(path);
                    Image obj;
                    Bitmap newing;
                    obj = Image.FromFile(path);
                    ImageFormat ImgFormat = obj.RawFormat;
                    newing = new Bitmap(obj, 25, 25);

                    newing.Save(dpath, ImgFormat);
                    ViewBag.path = dpath;
                    return RedirectToAction("Create");
                }

            }
            catch (Exception ex)
            {
                return Json(ex.Message + ex.StackTrace, JsonRequestBehavior.AllowGet);
            }

            return Json("request not sent");
        }

        // GET: projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.projectStatusID = new SelectList(db.projectStatus, "projectStatusID", "projectStatusName", project.projectStatusID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", project.userID);
            return View(project);
        }

        // POST: projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "projectID,projectName,projectDescription,projectMaterial,fileUrl,dateCreated,dateModified,createdBy,userID,projectStatusID")] project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.projectStatusID = new SelectList(db.projectStatus, "projectStatusID", "projectStatusName", project.projectStatusID);
            ViewBag.userID = new SelectList(db.proUsers, "userID", "firstname", project.userID);
            return View(project);
        }

        // GET: projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            project project = db.projects.Find(id);
            db.projects.Remove(project);
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
