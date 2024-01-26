using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Art_shop.Models;

namespace Art_shop.Controllers
{
    public class MembersController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: Members

        [HttpGet]
        public ActionResult Signup()
        {
            // Display the signup form
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Member a)
        {
            // Check if the username is already taken
            if (db.Members.Any(u => u.Name == a.Name))
            {
                ViewBag.ErrorMessage = "Username already exists. Please choose a different username.";
                return View();
            }

            // Check if other validation rules pass (e.g., PasswordHash requirements)

            // Assuming you have an 'admin_info' table in your database
            db.Members.Add(a);
            db.SaveChanges();

            // Redirect to login page after successful signup
            return RedirectToAction("login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Member a)
        {
            Member member = db.Members
                .Where(x => x.Email == a.Email && x.Password == a.Password)
                .FirstOrDefault();

            if (member != null)
            {
                Session["MemberID"] = member.MemberID.ToString();
                return RedirectToAction("Memberpanel");
            }
            else
            {
                // Handle the case where login fails, for example, set a ViewBag message
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }

        public ActionResult Memberpanel()
        {
            if (Session["MemberID"] == null)
            {
                return RedirectToAction("login");
            }

            return View();
        }

        public ActionResult Logout()
        {
            // Clear user session
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberID,Name,Email,Contact,Address,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,Name,Email,Contact,Address,Password")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
