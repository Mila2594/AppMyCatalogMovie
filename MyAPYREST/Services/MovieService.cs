//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAPYREST.Models;
using System.Text.Json;

namespace MyAPYREST.Services
{
    public class MovieService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.themoviedb.org/3";
        private const string ApiKey = "6f02e04c50406db95ca7666d2b1adcc0";
        private const string BearerToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI2ZjAyZTA0YzUwNDA2ZGI5NWNhNzY2NmQyYjFhZGNjMCIsInN1YiI6IjY3YTEyNzk1NmI2MDkxMThkMTI2NDc3ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.Cok2YPTQ_EMRT1t88NwrU1b4PnQhxIPK3sMMdljpp_A";

        public MovieService()
        {
            _httpClient = new HttpClient();

        }

        public async Task GetTrendingMoviesAsync()
        {
            Console.WriteLine("🔵 INICIO DEBUG");

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

                Console.WriteLine("🌐 Realizando petición...");
                var response = await client.GetStringAsync("https://api.themoviedb.org/3/trending/movie/week");

                Console.WriteLine("✅ Respuesta recibida:");
                Console.WriteLine(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Error: {ex.Message}");
            }
        }


        public async Task<List<Movie>> GetMoviesAsync(string query)
        {
            var url = string.IsNullOrEmpty(query)
         ? $"{ApiUrl}/discover/movie"
         : $"{ApiUrl}/search/movie?query={query}";

            Console.WriteLine("INICIO DEBUG");
            Console.WriteLine($"URL a consultar: {url}");

            try
            {
                Console.WriteLine("Realizando petición a la API...");

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Mandamos el Token de acceso en el Header como Bearer
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI2ZjAyZTA0YzUwNDA2ZGI5NWNhNzY2NmQyYjFhZGNjMCIsIm5iZiI6MTczODYxNDY3Ny4yNzIsInN1YiI6IjY3YTEyNzk1NmI2MDkxMThkMTI2NDc3ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.Cok2YPTQ_EMRT1t88NwrU1b4PnQhxIPK3sMMdljpp_A");

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: La API devolvió un código de estado {response.StatusCode}");
                    return new List<Movie>();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Respuesta JSON recibida");
                Console.WriteLine("Responde..." + jsonResponse);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var movieResponse = JsonSerializer.Deserialize<MovieResponse>(jsonResponse, options);

                if (movieResponse?.Results == null)
                {
                    Console.WriteLine("Error: No se pudieron obtener los resultados.");
                    return new List<Movie>();
                }

                Console.WriteLine("\nPelículas obtenidas:");
                foreach (var movie in movieResponse.Results)
                {
                    Console.WriteLine($"Título: {movie.Title} | Puntuación: {movie.Rating} | Fecha: {movie.ReleaseDate}");
                }

                return movieResponse.Results;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Error: La solicitud tardó demasiado y fue cancelada.");
                return new List<Movie>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las películas: {ex.Message}");
                return new List<Movie>();
            }
            finally
            {
                Console.WriteLine("FIN DEBUG");
            }

        }


        public async Task<List<Genre>> GetCategoriesAsync()
        {
            var url = $"{ApiUrl}/genre/movie/list?api_key={ApiKey}";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // Agregar el Bearer Token en los encabezados de la petición
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: La API devolvió un código de estado {response.StatusCode}");
                    return new List<Genre>();
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var genreResponse = JsonSerializer.Deserialize<GenreResponse>(jsonResponse, options);

                return genreResponse?.Genres ?? new List<Genre>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener las categorías: {ex.Message}");
                return new List<Genre>();
            }
        }

        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            var url = $"{ApiUrl}/movie/{movieId}?api_key={ApiKey}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error: La API devolvió {response.StatusCode}");
                    return null;
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var movie = JsonSerializer.Deserialize<Movie>(jsonResponse);

                return movie;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la película: {ex.Message}");
                return null;
            }
        }






    }
}
