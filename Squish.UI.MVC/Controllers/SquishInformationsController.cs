using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Squish.DATA.EF.Models;
using System.Drawing;
using Squish.UI.MVC.Utilities;

namespace Squish.UI.MVC.Controllers
{
    public class SquishInformationsController : Controller
    {
        private readonly SQUISHContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SquishInformationsController(SQUISHContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: SquishInformations
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var sQUISHContext = _context.SquishInformations.Include(s => s.Species).Include(s => s.Status);
            return View(await sQUISHContext.ToListAsync());
        }

        //public async Task<IActionResult> TiledProducts(string searchTerm, int squishId = 0, int page = 1)
        //{
        //    // Paged List - Step 4
        //    int pageSize = 6;// our tiled view displays rows of 3 products, so this will indicate how many total products to show on a page


        //    var products = _context.SquishInformations.Where(p => p.Squish).Include(p => p.Supplier).Include(p => p.OrderProducts).ToList();


        //    // DDL - Step 1
        //    // Note: we copied this line from the existing functionality in Products.Create()
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");


        //    // DDL - Step 3
        //    // Add logic to filter the results by categoryId

        //    if (squishId != 0)
        //    {
        //        squishInformation = products.Where(p => p.CategoryId == categoryId).ToList();
        //        // Recreate the dropdown list so the current category is still selected
        //        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
        //    }

        //    if (!String.IsNullOrEmpty(searchTerm))
        //    {
        //        products = products.Where(p =>
        //                            p.ProductName.ToLower().Contains(searchTerm.ToLower())
        //                            || p.Supplier.SupplierName.ToLower().Contains(searchTerm.ToLower())
        //                            || p.ProductDescription.ToLower().Contains(searchTerm.ToLower())
        //                            || p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())).ToList();

        //        ViewBag.SearchTerm = searchTerm;
        //        ViewBag.NbrResults = products.Count;
        //    }
        //    else
        //    {
        //        ViewBag.SearchTerm = null;
        //        ViewBag.NbrResults = null;
        //    }

        //}


        // GET: SquishInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SquishInformations == null)
            {
                return NotFound();
            }

            var squishInformation = await _context.SquishInformations
                .Include(s => s.Species)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(m => m.SquishId == id);
            if (squishInformation == null)
            {
                return NotFound();
            }

            return View(squishInformation);
        }

        // GET: SquishInformations/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["SpeciesId"] = new SelectList(_context.SquishSpecies, "SpeciesId", "SpeciesName");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            return View();
        }

        // POST: SquishInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SquishId,Squishname,SpeciesId,Description,SquishSize,SquishColor,Price,StatusId,SquishPic,Image")] SquishInformation squishInformation)
        {
            if (ModelState.IsValid)
            {
                
                //Check to see if a file was uploaded
                if (squishInformation.SquishPic != null)
                {
                    //Check the file type 
                    //- retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(squishInformation.Image.FileName);

                    //- Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //- verify the uploaded file has an extension matching one of the extensions in the list above
                    //- AND verify file size will work with our .NET app
                    if (validExts.Contains(ext.ToLower()) && squishInformation.SquishPic.Length < 4_194_303)//underscores don't change the number, they just make it easier to read
                    {
                        //Generate a unique filename
                        squishInformation.SquishPic = Guid.NewGuid() + ext;

                        //Save the file to the web server (here, saving to wwwroot/images)
                        //To access wwwroot, add a property to the controller for the _webHostEnvironment (see the top of this class for our example)
                        //Retrieve the path to wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        //variable for the full image path --> this is where we will save the image
                        string fullImagePath = webRootPath + "/images/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await squishInformation.Image.CopyToAsync(memoryStream);//transfer file from the request to server memory
                            using (var img = Image.FromStream(memoryStream))//add a using statement for the Image class (using System.Drawing)
                            {
                                //now, send the image to the ImageUtility for resizing and thumbnail creation
                                //items needed for the ImageUtility.ResizeImage()
                                //1) (int) maximum image size
                                //2) (int) maximum thumbnail image size
                                //3) (string) full path where the file will be saved
                                //4) (Image) an image
                                //5) (string) filename
                                int maxImageSize = 500;//in pixels
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, squishInformation.SquishPic, img, maxImageSize, maxThumbSize);
                                //myFile.Save("path/to/folder", "filename"); - how to save something that's NOT an image

                            }
                        }
                    }
                }
                else
                {
                    //If no image was uploaded, assign a default filename
                    //Will also need to download a default image and name it 'noimage.png' -> copy it to the /images folder
                    squishInformation.SquishPic = "noimage.png";
                }
                _context.Add(squishInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpeciesId"] = new SelectList(_context.SquishSpecies, "SpeciesId", "SpeciesName", squishInformation.SpeciesId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", squishInformation.StatusId);
            return View(squishInformation);
        }

        // GET: SquishInformations/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SquishInformations == null)
            {
                return NotFound();
            }

            var squishInformation = await _context.SquishInformations.FindAsync(id);
            if (squishInformation == null)
            {
                return NotFound();
            }
            ViewData["SpeciesId"] = new SelectList(_context.SquishSpecies, "SpeciesId", "SpeciesName", squishInformation.SpeciesId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", squishInformation.StatusId);
            return View(squishInformation);
        }

        // POST: SquishInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SquishId,Squishname,SpeciesId,Description,SquishSize,SquishColor,Price,StatusId")] SquishInformation squishInformation)
        {
            if (id != squishInformation.SquishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(squishInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SquishInformationExists(squishInformation.SquishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpeciesId"] = new SelectList(_context.SquishSpecies, "SpeciesId", "SpeciesName", squishInformation.SpeciesId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", squishInformation.StatusId);
            return View(squishInformation);
        }

        // GET: SquishInformations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SquishInformations == null)
            {
                return NotFound();
            }

            var squishInformation = await _context.SquishInformations
                .Include(s => s.Species)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(m => m.SquishId == id);
            if (squishInformation == null)
            {
                return NotFound();
            }

            return View(squishInformation);
        }

        // POST: SquishInformations/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SquishInformations == null)
            {
                return Problem("Entity set 'SQUISHContext.SquishInformations'  is null.");
            }
            var squishInformation = await _context.SquishInformations.FindAsync(id);
            if (squishInformation != null)
            {
                _context.SquishInformations.Remove(squishInformation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SquishInformationExists(int id)
        {
            return _context.SquishInformations.Any(e => e.SquishId == id);
        }
    }
}