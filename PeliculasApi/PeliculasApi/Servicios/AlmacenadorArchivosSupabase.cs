using System.Net.Http.Headers;

namespace PeliculasApi.Servicios
{
    public class AlmacenadorArchivosSupabase : IAlmacenadorArchivos
    {
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private readonly HttpClient _httpClient;

        public AlmacenadorArchivosSupabase(IConfiguration configuration)
        {
            _supabaseUrl = configuration.GetValue<string>("Supabase:Url")!;
            _supabaseKey = configuration.GetValue<string>("Supabase:ApiKey")!;
            _httpClient = new HttpClient();
        }

        public async Task<string> Almacenar(string contenedor, IFormFile archivo)
        {
            var extension = Path.GetExtension(archivo.FileName);
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var urlSubida = $"{_supabaseUrl}/storage/v1/object/{contenedor}/{nombreArchivo}";

            using (var request = new HttpRequestMessage(HttpMethod.Post, urlSubida))
            {
                request.Headers.Add("Authorization", $"Bearer {_supabaseKey}");
                request.Headers.Add("apikey", _supabaseKey);

                using (var stream = archivo.OpenReadStream())
                {
                    var content = new StreamContent(stream);
                    content.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);
                    request.Content = content;

                    var response = await _httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContext = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Error al subir imagen a Supabase: {errorContext}");
                    }
                }
            }

            return $"{_supabaseUrl}/storage/v1/object/public/{contenedor}/{nombreArchivo}";
        }

        async Task IAlmacenadorArchivos.Borrar(string? ruta, string contenedor)
        {
            if (string.IsNullOrWhiteSpace(ruta)) return;

            var nombreArchivo = Path.GetFileName(ruta);
            var urlBorrado = $"{_supabaseUrl}/storage/v1/object/{contenedor}";

            using (var request = new HttpRequestMessage(HttpMethod.Delete, urlBorrado))
            {
                request.Headers.Add("Authorization", $"Bearer {_supabaseKey}");
                request.Headers.Add("apikey", _supabaseKey);

                var jsonPlayload = $"{{\"prefixes\": [\"{nombreArchivo}\"]}}";
                request.Content = new StringContent(jsonPlayload, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    await _httpClient.SendAsync(request);
                } catch (Exception ex)
                {
                    throw new Exception($"Error al borrar imagen de Supabase: {ex.Message}");
            }
            }
        }
    }
}