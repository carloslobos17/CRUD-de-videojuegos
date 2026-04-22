using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AppWeb.Data;
using AppWeb.Filtros;
using AppWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
            //var data = (from v in _context.Videojuegos
            //            join c in _context.Categorias
            //            on v.idCategoria equals c.idCategoria
            //            group v by c.categoria into g
            //            select new
            //            {
            //                Categoria = g.Key,
            //                Total = g.Count()
            //            }).ToList();
            //ViewBag.Categorias = data.Select(x => x.Categoria).ToList();
            //ViewBag.Totales = data.Select(x => x.Total).ToList();
            return View();
		}

		public IActionResult ObtenerDatos(string categoria)
		{
			var query = from v in _context.Videojuegos
						join c in _context.Categorias
						on v.idCategoria equals c.idCategoria
						select new { c.categoria};

            if (!string.IsNullOrEmpty(categoria))
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
                        return RedirectToAction("Dashboard", "Account");
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
