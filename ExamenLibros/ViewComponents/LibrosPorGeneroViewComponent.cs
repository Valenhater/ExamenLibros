using ExamenLibros.Models;
using ExamenLibros.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenLibros.ViewComponents
{
    public class LibrosPorGeneroViewComponent : ViewComponent
    {
        private RepositoryLibros repo;

        public LibrosPorGeneroViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetAllGenerosAsync();
            return View(generos);
        }

    }
}
