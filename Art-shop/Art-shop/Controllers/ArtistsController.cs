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
    public class ArtistsController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artists.ToList());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Artists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artist artist, HttpPostedFileBase ArtistImageFile)
        {
            if (ModelState.IsValid)
            {
                // Check if the file was uploaded
                if (ArtistImageFile != null && ArtistImageFile.ContentLength > 0)
                {
                    // Specify the folder path
                    string folderPath = Server.MapPath("~/Images");

                    // Create the folder if it doesn't exist
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Get the file name and combine it with the folder path
                    string fileName = Path.GetFileName(ArtistImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    // Save the file to the server
                    ArtistImageFile.SaveAs(filePath);

                    // Set the ArtistImageTemp property to the file name
                    artist.ArtistImageTemp = fileName;

                    // Add the artist to the database
                    db.Artists.Add(artist);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            // If the model state is not valid or the file was not uploaded,
            // repopulate the CategoryList before returning to the view
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View(artist);
        }

        // POST: Artists/CreateWithModel
        [HttpPost]
        [ActionName("CreateWithModel")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // If the model state is not valid, repopulate the CategoryList before returning to the view
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", artist.CategoryID);
            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }

            // Assuming you have a method to get categories from your database
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", artist.CategoryID);

            return View(artist);
        }

        // POST: Artists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID,ArtistName,Bio,Website,Country,ArtistImageTemp,CategoryID")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                // Update the state of the entity to Modified
                db.Entry(artist).State = EntityState.Modified;

                // If the CategoryID is not explicitly included in the form, set it from the original artist
                if (artist.CategoryID == 0)
                {
                    artist.CategoryID = db.Artists.Find(artist.ArtistID).CategoryID;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Assuming you have a method to get categories from your database
            ViewBag.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", artist.CategoryID);

            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
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
