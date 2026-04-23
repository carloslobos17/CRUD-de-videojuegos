using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AppWeb.Data;
using AppWeb.Filtros;
using AppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWeb.Controllers
{
	public class AccountController : Controller
	{
		private readonly AppDbContext _context;

		public AccountController(AppDbContext context)
		{
			_context = context;
		}

		[SessionAuthorize]
		public IActionResult Dashboard()
		{
            var categorias = _context.Categorias.ToList();
            ViewBag.Categorias = categorias;
            return View();
		}

		public IActionResult ObtenerDatos(string categoria)
		{
			var query = from v in _context.Videojuegos
						join c in _context.Categorias
						on v.idCategoria equals c.idCategoria
						select new { c.categoria};

            if (!string.IsNullOrEmpty(categoria) && categoria != "todas")
            {
                query = query.Where(x => x.categoria == categoria);
            }

            var data = query
                .GroupBy(x => x.categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Count()
                }).ToList();

            return Json(data);
        }

		public async Task<IActionResult> DetalleCompras(DateTime? desde, DateTime? hasta, int paginas = 1)
		{
			int paginador = 10;
			var query = _context.Detalle_Compras
				.Include(d => d.Compra)
				//.Include(c => c.Videojuegos)
                .AsQueryable();

			if (desde.HasValue)
			{
				query = query.Where(d => d.fechaHoraTransaccion >= desde.Value);
            }
			if (hasta.HasValue)
			{
				query = query.Where(d => d.fechaHoraTransaccion <= hasta.Value);
            }

			var totalregistros = await query.CountAsync();
			var datos = await query
				.OrderByDescending(d => d.fechaHoraTransaccion)
				.Skip((paginas - 1) * paginador)
				.Take(paginador)
				.Select(d => new VentasViewModel
				{
					IdCompra = d.idCompra,
					VideojuegosId = d.VideojuegosId,
					cantidad = d.cantidad,
					total = d.total,
					estadoCompra = d.estadoCompra,
					fechaHoraTransaccion = d.fechaHoraTransaccion,
					codigoTransaccion = d.codigoTransaccion
				}).ToListAsync();

			ViewBag.TotalPaginas = (int)Math.Ceiling((double)totalregistros / paginador);
			ViewBag.PaginaActual = paginas;
			ViewBag.desde = desde;
            ViewBag.hasta = hasta;

            return View(datos);
        }


        public IActionResult Login()
        {
            return View();
        } 

        [HttpPost]

		public IActionResult Login (Login model)
		{
			var user = _context.Usuarios.
				FirstOrDefault(u => u.Correo == model.correo);

			if (user != null)
			{
				string saltedPassword = user.Salt + model.password;

				using (SHA256 sha256 = SHA256.Create())
				{
					//byte[] inputBytes = Encoding.UTF8.GetBytes(saltedPassword);
                    byte[] inputBytes = Encoding.Unicode.GetBytes(saltedPassword);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);
					//Console.WriteLine("Salt DB:" + user.Salt);
					//Console.WriteLine("Password input: " + model.password);
					//Console.WriteLine("Salted: " + (user.Salt + model.password));
					//Console.WriteLine("Hash generado: " + Convert.ToBase64String(hashBytes));
					//Console.WriteLine("Hash DB: " + Convert.ToBase64String(user.Contrasena));


                    if (hashBytes.SequenceEqual(user.Contrasena))
					{
                        HttpContext.Session.SetString("usuario", user.Nombre);
                        Console.WriteLine("Usuario logueado:" + user.Nombre);
                        HttpContext.Session.SetInt32("idRol", user.idRol);

						if(user.idRol == 1)
						{
                            return RedirectToAction("Dashboard", "Account");

                        }else if(user.idRol == 2)
                        {
                            return RedirectToAction("JuegosVenta", "Account");
							//juegos venta es la pantalla que se mostrara para vender
                        }
                        
                    }
                }
			}
			ViewBag.Error = "Credenciales incorrectas";
			return View();
		}

		public	IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}
	}
}
