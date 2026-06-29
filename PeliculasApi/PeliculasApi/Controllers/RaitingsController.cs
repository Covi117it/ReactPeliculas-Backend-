using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/rating")]
    public class RaitingsController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;

        public RaitingsController(ApplicationDbContext context, IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] RatingCreacionDTO ratingCreacionDTO)
        {
            var usuarioId = await servicioUsuarios.ObtenerUsuarioId();

            var ratingActual = await context.RatingsPeliculas
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingCreacionDTO.PeliculaId
                && x.UsuarioId == usuarioId);

               if (ratingActual is null)
            {
                var rating = new Rating()
                {
                    PeliculaId = ratingCreacionDTO.PeliculaId,
                    Puntuacion = ratingCreacionDTO.Puntuacion,
                    UsuarioId = usuarioId

                };

                context.Add(rating);
            } else
            {
                ratingActual.Puntuacion = ratingCreacionDTO.Puntuacion;
            }

            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
