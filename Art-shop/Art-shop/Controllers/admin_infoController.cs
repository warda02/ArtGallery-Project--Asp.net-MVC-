using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Art_shop.Models;

namespace Art_shop.Controllers
{
    public class admin_infoController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        [HttpGet]
        public ActionResult Signup()
        {
            // Display the signup form
            return View();
        }

        [HttpPost]
        public ActionResult Signup(admin_info a)
        {
            // Check if the username is already taken
            if (db.admin_info.Any(u => u.admin_name == a.admin_name))
            {
                ViewBag.ErrorMessage = "Username already exists. Please choose a different username.";
                return View();
            }

            // Check if other validation rules pass (e.g., PasswordHash requirements)

            // Assuming you have an 'admin_info' table in your database
            db.admin_info.Add(a);
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
        public ActionResult Login(admin_info a)
        {
            admin_info admin = db.admin_info
                .Where(x => x.admin_name == a.admin_name && x.admin_password == a.admin_password)
                .FirstOrDefault();

            if (admin != null)
            {
                Session["admin_id"] = admin.admin_id.ToString();
                return RedirectToAction("adminpanel");
            }
            else
            {
                // Handle the case where login fails, for example, set a ViewBag message
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }

        public ActionResult adminpanel()
        {
            if (Session["admin_id"] == null)
            {
                return RedirectToAction("login");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "admin_info");
        }
    }
}
