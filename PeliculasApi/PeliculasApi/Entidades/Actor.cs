using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public required string Nombre { get; set; }
    }
}
