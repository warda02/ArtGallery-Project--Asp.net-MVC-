 using System.Linq;
using System.Web.Mvc;
using Art_shop.Models;

namespace Art_shop.Controllers
{
    public class user_infoController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // Existing actions...

        // GET: user_info/Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: user_info/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "first_name,last_name,email,password,mobile,address1,address2")] user_info user_info)
        {
            if (ModelState.IsValid)
            {
                // Perform additional validation as needed
                // For simplicity, the password is stored as plain text in this example
                db.user_info.Add(user_info);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user_info);
        }

        // GET: user_info/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: user_info/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = db.user_info.SingleOrDefault(u => u.email == email && u.password == password);

            if (user != null)
            {
                // Store user ID in session for authentication
                Session["UserId"] = user.user_id;
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        // GET: user_info/Dashboard
        public ActionResult Dashboard()
        {
            // Check if user is authenticated
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve user data based on the user ID
            int userId = (int)Session["UserId"];
            var user = db.user_info.Find(userId);

            if (user == null)
            {
                // If user not found, redirect to login
                return RedirectToAction("Login");
            }

            // Pass the user data to the view
            return View(user);
        }

        // GET: user_info/Logout
        public ActionResult Logout()
        {
            // Clear user session
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Auctions()
        {
            return View();
        }
    }
}
