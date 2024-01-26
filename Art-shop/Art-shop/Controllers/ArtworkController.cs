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
    public class ArtworkController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: Artwork
        public ActionResult Index()
        {
            var artworks = db.Artworks.Include(a => a.Category).Include(a => a.Artist);
            return View(artworks.ToList());
        }

        // GET: Artwork/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artwork artwork = db.Artworks.Find(id);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            return View(artwork);
        }

        // GET: Artwork/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName");
            return View();
        }

        // POST: Artwork/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtworkID,ArtworkTitle,Description,Price,CategoryID,ArtistID")] Artwork artwork, HttpPostedFileBase ArtworkImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ArtworkImageFile != null && ArtworkImageFile.ContentLength > 0)
                {
                    // Specify the folder path where you want to save the images
                    string folderPath = Server.MapPath("~/Images/Artworks");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Get the file name and combine it with the folder path
                    string fileName = Path.GetFileName(ArtworkImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    // Save the file to the server
                    ArtworkImageFile.SaveAs(filePath);

                    // Set the ArtworkImage property to the file name
                    artwork.ArtworkImage = fileName;
                }

                db.Artworks.Add(artwork);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", artwork.CategoryID);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", artwork.ArtistID);
            return View(artwork);
        }

        // GET: Artwork/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artwork artwork = db.Artworks.Find(id);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", artwork.CategoryID);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", artwork.ArtistID);
            return View(artwork);
        }

        // POST: Artwork/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtworkID,ArtworkTitle,Description,Price,CategoryID,ArtistID,ArtworkImage")] Artwork artwork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artwork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", artwork.CategoryID);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", artwork.ArtistID);
            return View(artwork);
        }

        // GET: Artwork/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artwork artwork = db.Artworks.Find(id);
            if (artwork == null)
            {
                return HttpNotFound();
            }
            return View(artwork);
        }

        // POST: Artwork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artwork artwork = db.Artworks.Find(id);
            db.Artworks.Remove(artwork);
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
