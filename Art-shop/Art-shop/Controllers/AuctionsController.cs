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
    public class AuctionsController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: Auctions
        public ActionResult Index()
        {
            var auctions = db.Auctions.Include(a => a.Artwork).Include(a => a.Category).Include(a => a.Artist);
            return View(auctions.ToList());
        }

        // GET: Auctions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // GET: Auctions/Create
        public ActionResult Create()
        {
            ViewBag.ArtworkID = new SelectList(db.Artworks, "ArtworkID", "ArtworkTitle");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName");
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuctionID,ArtworkID,StartDate,EndDate,StartingBid,CurrentBid,ArtistID,CategoryID")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Auctions.Add(auction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtworkID = new SelectList(db.Artworks, "ArtworkID", "ArtworkTitle", auction.ArtworkID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", auction.CategoryID);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", auction.ArtistID);
            return View(auction);
        }

        // GET: Auctions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtworkID = new SelectList(db.Artworks, "ArtworkID", "ArtworkTitle", auction.ArtworkID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", auction.CategoryID);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", auction.ArtistID);
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuctionID,ArtworkID,StartDate,EndDate,StartingBid,CurrentBid,ArtistID,CategoryID")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtworkID = new SelectList(db.Artworks, "ArtworkID", "ArtworkTitle", auction.ArtworkID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", auction.CategoryID);
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName", auction.ArtistID);
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
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
