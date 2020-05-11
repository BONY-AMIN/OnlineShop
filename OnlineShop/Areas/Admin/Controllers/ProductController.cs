using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment  _he;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(p=>p.ProductTypes).Include(s=>s.SpecialTag).ToList());
        }

        //Index PostSearch Action Method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount,decimal? largeAmount)
        {
            var products=_db.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).Where(s=>s.Price>= lowAmount && s.Price<=largeAmount).ToList();
            if(lowAmount == null || largeAmount == null)
            {
                 products = _db.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).ToList();
            }
            return View(products);
        }


        //Create Get Action Method
        public ActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(),"Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_db.SpecialTags.ToList(),"Id", "SpeciaTag");

            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(Products products,IFormFile image)
        {
           
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(p => p.Name == products.Name);
                if (searchProduct != null)
                {
                    ViewBag.meassage = "This product is already exist!";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["specialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpeciaTag");

                    return View(products);
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noimage.png";
                }

                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product type has been saved";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(products);
        
        }

        //Edit Get Action Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpeciaTag");

            var product = _db.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).FirstOrDefault(p=>p.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "Images/noimage.png";
                }

                _db.Products.Update(products);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been updated";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(products);

        }

        //Details Action Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Delete Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Delete Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Products products)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != products.Id)
            {
                return NotFound();
            }

            var product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been deleted";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(product);
        }

    }
}