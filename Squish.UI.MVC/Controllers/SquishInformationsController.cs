using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Squish.DATA.EF.Models;

namespace Squish.UI.MVC.Controllers
{
    public class SquishInformationsController : Controller
    {
        private readonly SQUISHContext _context;

        public SquishInformationsController(SQUISHContext context)
        {
            _context = context;
        }

        // GET: SquishInformations
        public async Task<IActionResult> Index()
        {
            var sQUISHContext = _context.SquishInformations.Include(s => s.Species).Include(s => s.Status);
            return View(await sQUISHContext.ToListAsync());
        }

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
        public IActionResult Create()
        {
            ViewData["SpeciesId"] = new SelectList(_context.SquishSpecies, "SpeciesId", "SpeciesName");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId");
            return View();
        }

        // POST: SquishInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SquishId,Squishname,SpeciesId,Description,Seasonalid,SquishSize,SquishColor,Price,StatusId")] SquishInformation squishInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(squishInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpeciesId"] = new SelectList(_context.SquishSpecies, "SpeciesId", "SpeciesName", squishInformation.SpeciesId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "StatusId", "StatusId", squishInformation.StatusId);
            return View(squishInformation);
        }

        // GET: SquishInformations/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SquishId,Squishname,SpeciesId,Description,Seasonalid,SquishSize,SquishColor,Price,StatusId")] SquishInformation squishInformation)
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
