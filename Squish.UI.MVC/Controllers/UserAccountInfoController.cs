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
    public class UserAccountInfoController : Controller
    {
        private readonly SQUISHContext _context;

        public UserAccountInfoController(SQUISHContext context)
        {
            _context = context;
        }

        // GET: UserAccountInfoes
        public async Task<IActionResult> Index()
        {
              return View(await _context.UserAccountInfo.ToListAsync());
        }

        // GET: UserAccountInfoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UserAccountInfo == null)
            {
                return NotFound();
            }

            var userAccountInfo = await _context.UserAccountInfo
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userAccountInfo == null)
            {
                return NotFound();
            }

            return View(userAccountInfo);
        }

        // GET: UserAccountInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserAccountInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Address,City,State,ZipCode")] UserAccountInfo userAccountInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userAccountInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userAccountInfo);
        }

        // GET: UserAccountInfoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.UserAccountInfo == null)
            {
                return NotFound();
            }

            var userAccountInfo = await _context.UserAccountInfo.FindAsync(id);
            if (userAccountInfo == null)
            {
                return NotFound();
            }
            return View(userAccountInfo);
        }

        // POST: UserAccountInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,FirstName,LastName,Address,City,State,ZipCode")] UserAccountInfo userAccountInfo)
        {
            if (id != userAccountInfo.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userAccountInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserAccountInfoExists(userAccountInfo.UserId))
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
            return View(userAccountInfo);
        }

        // GET: UserAccountInfoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UserAccountInfo == null)
            {
                return NotFound();
            }

            var userAccountInfo = await _context.UserAccountInfo
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userAccountInfo == null)
            {
                return NotFound();
            }

            return View(userAccountInfo);
        }

        // POST: UserAccountInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UserAccountInfo == null)
            {
                return Problem("Entity set 'SQUISHContext.UserAccountInfos'  is null.");
            }
            var userAccountInfo = await _context.UserAccountInfo.FindAsync(id);
            if (userAccountInfo != null)
            {
                _context.UserAccountInfo.Remove(userAccountInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserAccountInfoExists(string id)
        {
          return _context.UserAccountInfo.Any(e => e.UserId == id);
        }
    }
}
