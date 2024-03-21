using ExamenLibros.Extensions;
using ExamenLibros.Filters;
using ExamenLibros.Models;
using ExamenLibros.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenLibros.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            return View();
        }
        public async Task<IActionResult> Libros()
        {
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }
        public async Task<IActionResult> LibrosGenero(int idgenero)
        {
            List<Libro> librosGenero = await this.repo.LibrosGeneroAsync(idgenero);
            return View(librosGenero);
        }
        public async Task<IActionResult> DetalleLibro(int id)
        {
            Libro libro = await this.repo.FindLibroAsync(id);
            return View(libro);
        }
        [AuthorizeUsuarios]
        public IActionResult GuardarLibroCarrito(int idLibro, int? idGenero)
        {
            if (idLibro != null)
            //Guardamos el producto en el carrito
            {
                List<int> carrito;
                if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idLibro);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }
            if (idGenero != null)
            {
                return RedirectToAction("LibrosGenero", "Libros", new { idgenero = idGenero });

            }
            else
            {
                return RedirectToAction("Libros");
            }
        }

        public IActionResult Carrito(int? idLibroEliminar, int idUsuario)
        {
            //Le pasamos el carrito
            ViewBag.IdUsuario = idUsuario;
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");

            //Tienes que crear para añadir datos al carrito
            if (carrito == null)
            {
                return View();
            }
            else
            {
                if (idLibroEliminar != null)
                {
                    carrito.Remove(idLibroEliminar.Value);
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
                List<Libro> libros = this.repo.GetLibrosCarrito(carrito);
                return View(libros);
            }
        }
        [AuthorizeUsuarios]
        [HttpPost]
        public async Task<IActionResult> ComprarProducto(List<int> libros, List<int> cantidades, int iduser)
        {
            int idpedido = await repo.GetUltimaCompra();
            int idfactura = await repo.GetUltimaFactura();
            for (int i = 0; i < libros.Count; i++)
            {
                int libro = libros[i];
                int cantidad = cantidades[i];

                idpedido++;
                idfactura++;
                Pedido nuevoPedido= new Pedido()
                {
                    IdPedido = idpedido,
                    IdFactura = idfactura,
                    IdLibro = libro,
                    Fecha = DateTime.Now,
                    IdUsuario = iduser,
                    Cantidad = cantidad
                };

                await repo.ComprarProducto(nuevoPedido);

            }
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("MostrarComprasUsuario", "Libros", new { iduser = iduser });
        }

        [AuthorizeUsuarios]
        public IActionResult MostrarComprasUsuario(int iduser)
        {
            List<VistaPedido> comprasUsuario = this.repo.GetComprasUsuario(iduser);
            return View(comprasUsuario);
        }

    }
}
