using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProwatchWebApp.Models;
using System.Web.Security;

namespace ProwatchWebApp.Controllers
{
    public class proUsersController : Controller
    {
        private ProWatchEntities db = new ProWatchEntities();

        // GET: proUsers
        public ActionResult Index()
        {
            return View(db.proUsers.ToList());
        }

        // GET: proUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proUser proUser = db.proUsers.Find(id);
            if (proUser == null)
            {
                return HttpNotFound();
            }
            return View(proUser);
        }


        // GET: prouser/signIn
        public ActionResult signIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(proUser user)

        {
            string firstname = db.proUsers.Where(y => y.email == user.email && y.password == user.password).Select(y => y.email).SingleOrDefault();
            if (firstname != null)
            {
                FormsAuthentication.SetAuthCookie(firstname, false);
                return RedirectToAction("Dashboard", "projects");               
            }
            TempData["Message"] = "Email or Password is Incorrect";
           
            return View();
           
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }


        // GET: proUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: proUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "firstname,lastname,email,password,phoneNumber")] proUser proUser)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now.Date;
                proUser.dateCreated = dt;
                db.proUsers.Add(proUser);
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(proUser.firstname, false);
                return RedirectToAction("Dashboard", "projects");
            }

            return View(proUser);
        }

        // GET: proUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proUser proUser = db.proUsers.Find(id);
            if (proUser == null)
            {
                return HttpNotFound();
            }
            return View(proUser);
        }

        // POST: proUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,firstname,lastname,email,password,phoneNumber,dateCreated,dateModified")] proUser proUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proUser);
        }

        // GET: proUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proUser proUser = db.proUsers.Find(id);
            if (proUser == null)
            {
                return HttpNotFound();
            }
            return View(proUser);
        }

        // POST: proUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proUser proUser = db.proUsers.Find(id);
            db.proUsers.Remove(proUser);
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
