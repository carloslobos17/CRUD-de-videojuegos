using AppWeb.Data;
using AppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

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
        public async Task<IActionResult> Create(Videojuego videojuegos, IFormFile archivoImagen)
        {
            if (!ModelState.IsValid) 
                return View(videojuegos);
            if (archivoImagen != null && archivoImagen.Length > 0)
            {
                var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivoImagen.FileName);

                var ruta = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/imagenes", nombreArchivo);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await archivoImagen.CopyToAsync(stream);
                }
                videojuegos.imagen = "/imagenes/" + nombreArchivo;
            }


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

        public async Task<IActionResult> Edit(int id, Videojuego videojuegos, IFormFile? archivoImagen)
        {
            if (id != videojuegos.Id) 
                return NotFound();

            var juegoDB = await _context.Videojuegos.FindAsync(id);
            if (juegoDB == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                juegoDB.Titulo = videojuegos.Titulo;
                juegoDB.Precio = videojuegos.Precio;
                juegoDB.Categoria = videojuegos.Categoria;
                juegoDB.Descripcion = videojuegos.Descripcion;
                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    if (!string.IsNullOrEmpty(juegoDB.imagen))
                    {
                        var rutaAnterior = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            juegoDB.imagen.TrimStart('/')
                            );
                        if(System.IO.File.Exists(rutaAnterior))
                            System.IO.File.Delete(rutaAnterior);
                    }
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivoImagen.FileName);

                    var rutaNueva = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/imagenes",
                        nombreArchivo
                        );
                    using (var stream = new FileStream(rutaNueva, FileMode.Create))
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                //try
                //{
                //    _context.Update(videojuegos);
                //    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!_context.Videojuegos.Any(e => e.Id == videojuegos.Id)) return NotFound();
                //    else
                //        throw;
                //}

                ////return RedirectToAction(nameof(Index));
            }
            return View(juegoDB);
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