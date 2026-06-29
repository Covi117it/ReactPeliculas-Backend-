using PeliculasAPI.Entidades;

namespace PeliculasApi.Entidades
{
    public class PeliculaGenero
    {
        public int GeneroId { get; set; }
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; } = null!;
        public Genero Genero { get; set; } = null!;
    }
}
