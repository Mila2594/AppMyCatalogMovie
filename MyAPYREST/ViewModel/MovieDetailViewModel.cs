using MyAPYREST.Models;
using MyAPYREST.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbApiDom.Dto.SidewayClasses.SubClasses;

namespace MyAPYREST.ViewModel
{
    public class MovieDetailViewModel : BaseViewModel
    {
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";
        private readonly DatabaseService _databaseService;

        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string ReleaseYear { get; set; }
        public string Rating { get; set; }
        public string Overview { get; set; }
        public string Language { get; set; }
        public ICommand FavoriteCommand { get; }

        private bool isFavorite;
        public bool IsFavorite
        {
            get => isFavorite;
            set => SetProperty(ref isFavorite, value);
        }
        private FavoriteMovie _favoriteMovie;

        public MovieDetailViewModel(Movie movie)
        {
            _databaseService = new DatabaseService();
            Title = movie.Title;
            PosterPath = string.IsNullOrEmpty(movie.PosterPath) ? "placeholder.png" : $"{ImageBaseUrl}{movie.PosterPath}";
            // Genres = string.Join(", ", movie.GenreNames);
            ReleaseYear = movie.ReleaseDate?.Substring(0, 4);
            // Duration = $"{movie.Runtime} min";
            Rating = $"{movie.Rating}/10";
            Overview = movie.Overview;
            Language = movie.Language.ToUpper();

            FavoriteCommand = new Command(async () => await ToggleFavorite(movie));
            CheckIfFavorite(movie.Id);

            MessagingCenter.Subscribe<FavoritesViewModel>(this, "FavoriteUpdated", async (sender) =>
            {
                await CheckIfFavorite(movie.Id);  // 🔥 Verificar si la película sigue en favoritos
            });

        }

        private async Task CheckIfFavorite(int movieId)
        {
            var favorite = await _databaseService.GetFavoriteByIdAsync(movieId);
            IsFavorite = favorite != null;
            OnPropertyChanged(nameof(IsFavorite));  // 🔥 Esto actualizará la UI automáticamente
        }


        private async Task ToggleFavorite(Movie movie)
        {
            var existingMovie = await _databaseService.GetFavoriteByIdAsync(movie.Id);

            if (existingMovie != null)
            {
                Console.WriteLine($" Eliminando de favoritos: {movie.Title}");
                await _databaseService.RemoveFavoriteAsync(existingMovie);
                IsFavorite = false;
            }
            else
            {
                Console.WriteLine($" Agregando a favoritos: {movie.Title}");

                _favoriteMovie = new FavoriteMovie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterPath = movie.PosterPath,
                    ReleaseYear = movie.ReleaseDate?.Substring(0, 4),
                    Rating = $"{movie.Rating}/10"
                };
                await _databaseService.AddFavoriteAsync(_favoriteMovie);
                IsFavorite = true;
            }
        }

    }
}
