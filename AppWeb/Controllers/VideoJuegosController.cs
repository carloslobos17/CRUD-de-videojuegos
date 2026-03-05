using AppWeb.Data;
using AppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWeb.Controllers
{
    public class VideoJuegosController : Controller
    {
        private readonly AppDbContext _context;

        public VideoJuegosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var juegos = await _context.Videojuegos.ToListAsync();
            return View(juegos);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Videojuego videojuegos)
        {
            if (!ModelState.IsValid) return View(videojuegos);
            _context.Videojuegos.Add(videojuegos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var juego = await _context.Videojuegos.FindAsync(id);
            if (juego == null) return NotFound();

            return View(juego);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, Videojuego videojuegos)
        {
            if (id != videojuegos.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(videojuegos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Videojuegos.Any(e => e.Id == videojuegos.Id)) return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(videojuegos);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var juego = await _context.Videojuegos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (juego == null) return NotFound();

            return View(juego);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var juego = await _context.Videojuegos.FindAsync(id);

            if (juego != null)
            {
                _context.Videojuegos.Remove(juego);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(Index));
        }
    }
}