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
    public class MemberArtworksController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: MemberArtworks
        public ActionResult Index()
        {
            var memberArtworks = db.MemberArtworks.Include(m => m.Member);
            return View(memberArtworks.ToList());
        }

        // GET: MemberArtworks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberArtwork memberArtwork = db.MemberArtworks.Find(id);
            if (memberArtwork == null)
            {
                return HttpNotFound();
            }
            return View(memberArtwork);
        }

        // GET: MemberArtworks/Create
        public ActionResult Create()
        {
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name");
            return View();
        }

        // POST: MemberArtworks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtworkID,MemberID,ArtworkTitle,ArtworkImage,Category")] MemberArtwork memberArtwork, HttpPostedFileBase ArtworkImageFile)
        {
            if (ModelState.IsValid)
            {
                // Check if the file was uploaded
                if (ArtworkImageFile != null && ArtworkImageFile.ContentLength > 0)
                {
                    // Specify the folder path
                    string folderPath = Server.MapPath("~/Images/");

                    // Create the folder if it doesn't exist
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
                    memberArtwork.ArtworkImage = fileName;

                    // Add the Artwork to the database
                    db.MemberArtworks.Add(memberArtwork);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", memberArtwork.MemberID);
            return View(memberArtwork);
        }

        // GET: MemberArtworks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberArtwork memberArtwork = db.MemberArtworks.Find(id);
            if (memberArtwork == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", memberArtwork.MemberID);
            return View(memberArtwork);
        }

        // POST: MemberArtworks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtworkID,MemberID,ArtworkTitle,ArtworkImage,Category")] MemberArtwork memberArtwork)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberArtwork).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", memberArtwork.MemberID);
            return View(memberArtwork);
        }

        // GET: MemberArtworks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberArtwork memberArtwork = db.MemberArtworks.Find(id);
            if (memberArtwork == null)
            {
                return HttpNotFound();
            }
            return View(memberArtwork);
        }

        // POST: MemberArtworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberArtwork memberArtwork = db.MemberArtworks.Find(id);
            db.MemberArtworks.Remove(memberArtwork);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MyArt()
        {
 return View();
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
