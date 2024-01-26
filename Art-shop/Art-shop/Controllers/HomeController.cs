using Art_shop.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web;
using System;

namespace Art_shop.Controllers
{
    public class HomeController : Controller
    {
        private GalleryEntities db = new GalleryEntities();

        // GET: Home/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/About
        public ActionResult About()
        {
            return View();
        }

        // GET: Home/Artists
        public ActionResult Artists()
        {
            // Retrieve artists with associated category information
            var artists = db.Artists.Include(a => a.Category).ToList();
            return View(artists);
        }

        // POST: Home/CreateArtist
        [HttpPost]
        public ActionResult CreateArtist(Artist artist, HttpPostedFileBase ArtistImageFile)
        {
            if (ModelState.IsValid)
            {
                // Check if ArtistImageFile is provided and save it
                if (ArtistImageFile != null && ArtistImageFile.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(ArtistImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    ArtistImageFile.SaveAs(filePath);
                    artist.ArtistImageTemp = fileName;
                }

                // Add artist to the database
                db.Artists.Add(artist);
                db.SaveChanges();

                return RedirectToAction("Artists");
            }

            // Repopulate ViewBag.CategoryList if validation fails
            ViewBag.CategoryList = new SelectList(db.Categories.ToList(), "CategoryID", "CategoryName");
            return View("Artists", artist);
        }

        // GET: Home/Artwork
        public ActionResult Artwork()
        {
            // Retrieve artworks with associated category and artist information
            var artworks = db.Artworks.Include(a => a.Category).Include(a => a.Artist).ToList();
            return View(artworks);
        }

        // POST: Home/CreateArtwork
        [HttpPost]
        public ActionResult CreateArtwork(Artwork artwork, HttpPostedFileBase ArtworkImageFile)
        {
            if (ModelState.IsValid)
            {
                // Check if ArtworkImageFile is provided and save it
                if (ArtworkImageFile != null && ArtworkImageFile.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Images");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(ArtworkImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    ArtworkImageFile.SaveAs(filePath);
                    artwork.ArtworkImage = fileName;
                }

                // Add artwork to the database
                db.Artworks.Add(artwork);
                db.SaveChanges();

                return RedirectToAction("Artwork");
            }

            // Repopulate ViewBag.CategoryList and ViewBag.ArtistList if validation fails
            ViewBag.CategoryList = new SelectList(db.Categories.ToList(), "CategoryID", "CategoryName");
            ViewBag.ArtistList = new SelectList(db.Artists.ToList(), "ArtistID", "ArtistName");
            return View("Artwork", artwork);
        }

        // GET: Home/Auctions
        public ActionResult Auctions()
        {
            // Retrieve auctions with associated artwork, category, and artist information
            var auctions = db.Auctions.Include(a => a.Artwork).Include(a => a.Category).Include(a => a.Artist).ToList();
            return View(auctions);
        }



        // POST: Home/Create (for creating artists)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistID,ArtistName,CategoryID,ArtistImageTemp")] Artist artist, HttpPostedFileBase ArtistImageFile)
        {
            if (ModelState.IsValid)
            {
                // Add artist to the database
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Artists");
            }

            // Repopulate ViewBag.CategoryList if validation fails
            ViewBag.CategoryList = new SelectList(db.Categories.ToList(), "CategoryID", "CategoryName");
            return View(artist);
        }

        // GET: Home/Shows
        public ActionResult Shows()
        {
            // Retrieve shows from the database
            var shows = db.Shows.ToList();

            // Pass the list of shows to the view
            return View(shows);
        }

        // POST: Home/Create (for creating shows)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Show show, HttpPostedFileBase Show_Image)
        {
            if (ModelState.IsValid)
            {
                // Check if the file was uploaded
                if (Show_Image != null && Show_Image.ContentLength > 0)
                {
                    // Specify the folder path
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

                    // Add the show to the database
                    db.Shows.Add(show);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            // Return the view with the show model if ModelState is not valid
            return View(show);
        }


        // GET: Home/User_panel
        public ActionResult User_panel()
        {
            return View();
        }

        // GET: Home/MemberArtwork
        public ActionResult MemberArtwork()
        {
            // Assuming db is an instance of your data context (GalleryEntities5)
            var memberArtworks = db.MemberArtworks
                .Include(m => m.Member) // Include the Member table
                .ToList();

            return View(memberArtworks);
        }

        // POST: Home/CreateMemberArtwork
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMemberArtwork(MemberArtwork memberartwork, HttpPostedFileBase ArtworkImageFile)
        {
            if (ModelState.IsValid)
            {
                // Check if ArtworkImageFile is provided and save it
                if (ArtworkImageFile != null && ArtworkImageFile.ContentLength > 0)
                {
                    string folderPath = Server.MapPath("~/Images/ArtworkImage");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(ArtworkImageFile.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    ArtworkImageFile.SaveAs(filePath);
                    memberartwork.ArtworkImage = fileName;
                }

                // Add member artwork to the database
                db.MemberArtworks.Add(memberartwork);
                db.SaveChanges();

                return RedirectToAction("MemberArtwork");
            }

            ViewBag.MemberID = new SelectList(db.Members, "MemberID", "Name", memberartwork.MemberID);
            return View(memberartwork);
        }

        // POST: Home/Create (for creating email_info)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email_id,email")] email_info email_info)
        {
            if (ModelState.IsValid)
            {
                // Add email_info to the database
                db.email_info.Add(email_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(email_info);
        }



        // Add to cart work//
          List<Cart> li = new List<Cart>();


        public ActionResult AddtoCart(int id)
        {
            var query = db.Artworks.SingleOrDefault(x => x.ArtworkID == id);
            return View(query);
        }

        [HttpPost]
        public ActionResult AddtoCart(int id, int qty)
        {
            Artwork a = db.Artworks.SingleOrDefault(x => x.ArtworkID == id);

            if (a == null)
            {
                // Handle the case where the artwork with the given ID is not found.
                return HttpNotFound("Artwork not found");
            }

            Cart c = new Cart();
            c.Artwork = a;
            c.qty = qty; // No need to convert to int, qty is already an int.
            c.bill = c.Price * c.qty;

            if (TempData["cart"] == null)
            {
                List<Cart> li = new List<Cart>();
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                int flag = 0;
                foreach (var item in li2)
                {
                    if (item.Artwork.ArtworkID == c.Artwork.ArtworkID)
                    {
                        item.qty += c.qty;
                        item.bill += c.bill;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }
                TempData["cart"] = li2;
            }

            TempData.Keep();

            return RedirectToAction("Artwork");
        }

        #region remove cart item

        public ActionResult remove(int? id)
        {
            if (id == null || TempData["cart"] == null)
            {
                TempData.Remove("total");
                TempData.Remove("cart");
            }
            else
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                Cart c = li2.SingleOrDefault(x => x.Artwork.ArtworkID == id);
                if (c != null)
                {
                    li2.Remove(c);
                    int s = li2.Sum(item => item.bill);
                    TempData["total"] = s;
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        public ActionResult Checkout()
        {
            return View();
        }







     }
}
