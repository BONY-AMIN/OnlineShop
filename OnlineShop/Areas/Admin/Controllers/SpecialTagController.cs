using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private ApplicationDbContext _db;
        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        //Index
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }

        //Create Get Action Method
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SpecialTag SpecialTag)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Add(SpecialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Special Tag has been saved";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(SpecialTag);

        }


        //Edit Get Action Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(SpecialTag SpecialTag)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Update(SpecialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Special Tag has been updated";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(SpecialTag);

        }

        //Details Get Action Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //Details Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Details(SpecialTag SpecialTag)
        {

            return RedirectToAction(actionName: nameof(Index));

        }

        //Delete Get Action Method

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }

            return View(specialTag);
        }

        //Delete Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTag SpecialTag)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != SpecialTag.Id)
            {
                return NotFound();
            }

            var specialTag = _db.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Remove(specialTag);
                await _db.SaveChangesAsync();
                TempData["save"] = "Special Tag has been deletd";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(specialTag);
        }
    }
}