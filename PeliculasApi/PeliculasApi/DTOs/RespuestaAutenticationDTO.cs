namespace PeliculasApi.DTOs
{
    public class RespuestaAutenticationDTO
    {
        public  required string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
