using MyAPYREST.Models;
using MyAPYREST.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyAPYREST.ViewModel
{
    public class FavoritesViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";

        public ObservableCollection<FavoriteMovie> FavoriteMovies { get; set; }
        public ICommand DeleteFavoriteCommand { get; }
        public ICommand SaveNotesCommand { get; }

        public FavoritesViewModel()
        {
            _databaseService = new DatabaseService();
            FavoriteMovies = new ObservableCollection<FavoriteMovie>();
            _ = LoadFavorites();

            DeleteFavoriteCommand = new Command<FavoriteMovie>(async (movie) => await RemoveFavorite(movie));
            SaveNotesCommand = new Command<FavoriteMovie>(async (movie) => await SaveNotes(movie));

            MessagingCenter.Subscribe<MovieViewModel>(this, "FavoriteUpdated", async (sender) =>
            {
                await LoadFavorites(); // Recargar la lista de favoritos
            });
        }

        private async Task RemoveFavorite(FavoriteMovie movie)
        {
            if (movie == null) return;

            await _databaseService.RemoveFavoriteAsync(movie);
            FavoriteMovies.Remove(movie);

            OnPropertyChanged(nameof(FavoriteMovies));

            // 🔥 NOTIFICAR A HOME PARA QUE ACTUALICE INMEDIATAMENTE
            MessagingCenter.Send(this, "FavoriteUpdated");
        }


        private async Task SaveNotes(FavoriteMovie movie)
        {
            if (movie == null) return;

            try
            {
                var existingMovie = await _databaseService.GetFavoriteByIdAsync(movie.Id);

                if (existingMovie == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se encontró la película en la base de datos", "OK");
                    return;
                }

                existingMovie.Notes = movie.Notes;
                await _databaseService.UpdateFavoriteAsync(existingMovie);

                // 🔹 Asegurar que la imagen del póster NO se pierda al actualizar
                existingMovie.PosterPath = string.IsNullOrEmpty(existingMovie.PosterPath)
                    ? "placeholder.png"
                    : $"{ImageBaseUrl}{existingMovie.PosterPath}";

                // 🔹 REFRESCAR UI PARA QUE SE VEA EL CAMBIO AL INSTANTE
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    int index = FavoriteMovies.IndexOf(movie);
                    if (index >= 0)
                    {
                        FavoriteMovies[index] = existingMovie;
                        OnPropertyChanged(nameof(FavoriteMovies));
                    }
                });

                // 🔹 OCULTAR TECLADO DESPUÉS DE GUARDAR
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (Application.Current.MainPage is Page page)
                    {
                        page.DisplayAlert("Guardado", "Nota guardada con éxito", "OK");

                        // 🔥 Ocultar el teclado enfocando otro elemento de la UI
                        var currentPage = Application.Current.MainPage as ContentPage;
                        if (currentPage != null)
                        {
                            var dummyEntry = new Entry { IsVisible = false };
                            currentPage.Content.FindByName<Entry>("HiddenEntry")?.Focus();
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la nota: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un problema al guardar la nota.", "OK");
            }
        }




        private async Task LoadFavorites()
        {
            var movies = await _databaseService.GetFavoritesAsync();

            // 🔹 Asegurar que la UI refleja los cambios
            MainThread.BeginInvokeOnMainThread(() =>
            {
                FavoriteMovies.Clear();
                foreach (var movie in movies)
                {
                    movie.PosterPath = string.IsNullOrEmpty(movie.PosterPath) ? "placeholder.png" : $"{ImageBaseUrl}{movie.PosterPath}";
                    FavoriteMovies.Add(movie);
                }
                OnPropertyChanged(nameof(FavoriteMovies));  // 🔥 Refresca la lista en la UI
            });
        }

    }
}
