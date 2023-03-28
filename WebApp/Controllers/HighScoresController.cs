using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class HighScoresController : Controller
    {
        private readonly WebAppContext _context;

        public HighScoresController(WebAppContext context)
        {
            _context = context;
        }

        // GET: HighScores
        public async Task<IActionResult> Index()
        {
              return _context.HighScores != null ? 
                          View(await _context.HighScores.ToListAsync()) :
                          Problem("Entity set 'WebAppContext.HighScores'  is null.");
        }

        // GET: HighScores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HighScores == null)
            {
                return NotFound();
            }

            var highScore = await _context.HighScores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (highScore == null)
            {
                return NotFound();
            }

            return View(highScore);
        }

        // GET: HighScores/Create
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HighScores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Create([Bind("Id,Name,Score")] HighScore highScore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(highScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(highScore);
        }

        // GET: HighScores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HighScores == null)
            {
                return NotFound();
            }

            var highScore = await _context.HighScores.FindAsync(id);
            if (highScore == null)
            {
                return NotFound();
            }
            return View(highScore);
        }

        // POST: HighScores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Score")] HighScore highScore)
        {
            if (id != highScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(highScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HighScoreExists(highScore.Id))
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
            return View(highScore);
        }

        // GET: HighScores/Delete/5
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HighScores == null)
            {
                return NotFound();
            }

            var highScore = await _context.HighScores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (highScore == null)
            {
                return NotFound();
            }

            return View(highScore);
        }

        // POST: HighScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HighScores == null)
            {
                return Problem("Entity set 'WebAppContext.HighScores'  is null.");
            }
            var highScore = await _context.HighScores.FindAsync(id);
            if (highScore != null)
            {
                _context.HighScores.Remove(highScore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HighScoreExists(int id)
        {
          return (_context.HighScores?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
