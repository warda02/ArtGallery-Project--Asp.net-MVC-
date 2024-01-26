using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Art_shop.Models;

namespace Art_shop.Controllers
{
    public class ShowsController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: Shows
        public ActionResult Index()
        {
            return View(db.Shows.ToList());
        }

        // GET: Shows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Shows.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // GET: Shows/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShowID,Title,Description,StartDate,EndDate,Venue,Organizer,AdminNotes,Show_Image")] Show show, HttpPostedFileBase Show_Image)
        {
            if (ModelState.IsValid)
            {
                // Check if an image file is uploaded
                if (Show_Image != null && Show_Image.ContentLength > 0)
                {
                    // Specify the folder path to save the image
                    string folderPath = Server.MapPath("~/Images");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Get the file name and combine it with the folder path
                    string fileName = Path.GetFileName(Show_Image.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    // Save the file to the server
                    Show_Image.SaveAs(filePath);

                    // Set the Show_Image property to the file name
                    show.Show_Image = fileName;
                }

                // Add the show to the database
                db.Shows.Add(show);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(show);
        }

        // GET: Shows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Shows.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShowID,Title,Description,StartDate,EndDate,Venue,Organizer,AdminNotes,Show_Image")] Show show)
        {
            if (ModelState.IsValid)
            {
                db.Entry(show).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(show);
        }

        // GET: Shows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Show show = db.Shows.Find(id);
            if (show == null)
            {
                return HttpNotFound();
            }
            return View(show);
        }

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Show show = db.Shows.Find(id);
            db.Shows.Remove(show);
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
