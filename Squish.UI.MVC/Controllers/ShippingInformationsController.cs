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
    public class ShippingInformationsController : Controller
    {
        private readonly SQUISHContext _context;

        public ShippingInformationsController(SQUISHContext context)
        {
            _context = context;
        }

        // GET: ShippingInformations
        public async Task<IActionResult> Index()
        {
            var sQUISHContext = _context.ShippingInformations.Include(s => s.Order).Include(s => s.User);
            return View(await sQUISHContext.ToListAsync());
        }

        // GET: ShippingInformations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShippingInformations == null)
            {
                return NotFound();
            }

            var shippingInformation = await _context.ShippingInformations
                .Include(s => s.Order)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ShippingId == id);
            if (shippingInformation == null)
            {
                return NotFound();
            }

            return View(shippingInformation);
        }

        // GET: ShippingInformations/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["UserId"] = new SelectList(_context.UserAccountInfo, "UserId", "UserId");
            return View();
        }

        // POST: ShippingInformations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShippingId,Firstname,Lastname,Address,City,State,ZipCode,OrderId,UserId")] ShippingInformation shippingInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", shippingInformation.OrderId);
            ViewData["UserId"] = new SelectList(_context.UserAccountInfo, "UserId", "UserId", shippingInformation.UserId);
            return View(shippingInformation);
        }

        // GET: ShippingInformations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShippingInformations == null)
            {
                return NotFound();
            }

            var shippingInformation = await _context.ShippingInformations.FindAsync(id);
            if (shippingInformation == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", shippingInformation.OrderId);
            ViewData["UserId"] = new SelectList(_context.UserAccountInfo, "UserId", "UserId", shippingInformation.UserId);
            return View(shippingInformation);
        }

        // POST: ShippingInformations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShippingId,Firstname,Lastname,Address,City,State,ZipCode,OrderId,UserId")] ShippingInformation shippingInformation)
        {
            if (id != shippingInformation.ShippingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingInformationExists(shippingInformation.ShippingId))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", shippingInformation.OrderId);
            ViewData["UserId"] = new SelectList(_context.UserAccountInfo, "UserId", "UserId", shippingInformation.UserId);
            ViewData["UserId"] = new SelectList(_context.UserAccountInfo, "UserId", "UserId", shippingInformation.UserId);
            return View(shippingInformation);
        }

        // GET: ShippingInformations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShippingInformations == null)
            {
                return NotFound();
            }

            var shippingInformation = await _context.ShippingInformations
                .Include(s => s.Order)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.ShippingId == id);
            if (shippingInformation == null)
            {
                return NotFound();
            }

            return View(shippingInformation);
        }

        // POST: ShippingInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShippingInformations == null)
            {
                return Problem("Entity set 'SQUISHContext.ShippingInformations'  is null.");
            }
            var shippingInformation = await _context.ShippingInformations.FindAsync(id);
            if (shippingInformation != null)
            {
                _context.ShippingInformations.Remove(shippingInformation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippingInformationExists(int id)
        {
          return _context.ShippingInformations.Any(e => e.ShippingId == id);
        }
    }
}
