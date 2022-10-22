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
    public class SquishSpeciesController : Controller
    {
        private readonly SQUISHContext _context;

        public SquishSpeciesController(SQUISHContext context)
        {
            _context = context;
        }

        // GET: SquishSpecies
        public async Task<IActionResult> Index()
        {
              return View(await _context.SquishSpecies.ToListAsync());
        }

        // GET: SquishSpecies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SquishSpecies == null)
            {
                return NotFound();
            }

            var squishSpecy = await _context.SquishSpecies
                .FirstOrDefaultAsync(m => m.SpeciesId == id);
            if (squishSpecy == null)
            {
                return NotFound();
            }

            return View(squishSpecy);
        }

        // GET: SquishSpecies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SquishSpecies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpeciesId,SpeciesName,SpeciesDescription")] SquishSpecy squishSpecy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(squishSpecy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(squishSpecy);
        }

        // GET: SquishSpecies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SquishSpecies == null)
            {
                return NotFound();
            }

            var squishSpecy = await _context.SquishSpecies.FindAsync(id);
            if (squishSpecy == null)
            {
                return NotFound();
            }
            return View(squishSpecy);
        }

        // POST: SquishSpecies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpeciesId,SpeciesName,SpeciesDescription")] SquishSpecy squishSpecy)
        {
            if (id != squishSpecy.SpeciesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(squishSpecy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SquishSpecyExists(squishSpecy.SpeciesId))
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
            return View(squishSpecy);
        }

        // GET: SquishSpecies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SquishSpecies == null)
            {
                return NotFound();
            }

            var squishSpecy = await _context.SquishSpecies
                .FirstOrDefaultAsync(m => m.SpeciesId == id);
            if (squishSpecy == null)
            {
                return NotFound();
            }

            return View(squishSpecy);
        }

        // POST: SquishSpecies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SquishSpecies == null)
            {
                return Problem("Entity set 'SQUISHContext.SquishSpecies'  is null.");
            }
            var squishSpecy = await _context.SquishSpecies.FindAsync(id);
            if (squishSpecy != null)
            {
                _context.SquishSpecies.Remove(squishSpecy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SquishSpecyExists(int id)
        {
          return _context.SquishSpecies.Any(e => e.SpeciesId == id);
        }
    }
}
