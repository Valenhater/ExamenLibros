using ExamenLibros.Data;
using ExamenLibros.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamenLibros.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }
        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<Libro> FindLibroAsync(int idLibro)
        {
            return await this.context.Libros.FirstOrDefaultAsync(x => x.IdLibro == idLibro);
        }
        public async Task<List<Libro>> LibrosGeneroAsync(int idgenero)
        {
            return await this.context.Libros.Where(p => p.IdGenero == idgenero).ToListAsync();
        }
        public async Task<List<Genero>> GetAllGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
        public List<Libro> GetLibrosCarrito(List<int> idLibro)
        {
            return context.Libros.Where(p => idLibro.Contains(p.IdLibro)).ToList();
        }
        public async Task ComprarProducto(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            await context.SaveChangesAsync();
        }
        public async Task<int> GetUltimaCompra()
        {
            var ultimoIdImagen = await this.context.Pedidos
                                            .MaxAsync(imagen => (int?)imagen.IdPedido);

            return ultimoIdImagen ?? 1;
        }
        public async Task<int> GetUltimaFactura()
        {
            var ultimoIdImagen = await this.context.Pedidos
                                            .MaxAsync(imagen => (int?)imagen.IdFactura);

            return ultimoIdImagen ?? 1;
        }
        public List<VistaPedido> GetComprasUsuario(int userid)
        {
            return context.VistaPedidos.Where(c => c.IdUsuario == userid).ToList();
        }
    }
}
