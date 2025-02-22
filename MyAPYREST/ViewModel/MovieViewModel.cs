using CommunityToolkit.Maui.Core.Views;
using MyAPYREST.Models;
using MyAPYREST.Services;
using MyAPYREST.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMDbApiDom.Dto.Search;

namespace MyAPYREST.ViewModel
{
    public class MovieViewModel : BaseViewModel
    {
        private readonly MovieService _movieService;
        private readonly DatabaseService _databaseService;
        private List<Movie> _allMovies = new List<Movie>();
        private List<Movie> _filteredMovies = new List<Movie>();
        private List<Movie> _favorites;
        private List<Genre> _categories = new List<Genre>();
        private string _selectedCategory;

        private string searchQuery = string.Empty;
        public string SearchQuery
        {
            get { return searchQuery; }
            set { searchQuery = value; }
        }

        private bool _isAllActive = true;
        private bool _isEstrenosActive;
        private bool _isCategoryActive;

        public bool IsAllActive
        {
            get => _isAllActive;
            set => SetProperty(ref _isAllActive, value);
        }

        public bool IsEstrenosActive
        {
            get => _isEstrenosActive;
            set => SetProperty(ref _isEstrenosActive, value);
        }

        public bool IsCategoryActive
        {
            get => _isCategoryActive;
            set => SetProperty(ref _isCategoryActive, value);
        }

        public List<Movie> Movies
        {
            get => _filteredMovies;
            set => SetProperty(ref _filteredMovies, value);
        }

        private string searchTerm;
        public string SearchTerm
        {
            get => searchTerm;
            set => SetProperty(ref searchTerm, value);
        }

        private ICommand searchCommand;
        public ICommand SearchCommand
        {
            get { return searchCommand; }
            private set { searchCommand = value; }
        }
        public ICommand AllCommand { get; }
        public ICommand EstrenosCommand { get; }
        public ICommand CategoryCommand { get; }
        public ICommand NavigateToDetailCommand { get; }
        public ICommand FavoriteCommand { get; }

        //private SQLiteConnection _db;

        public MovieViewModel() 
        {
            _movieService = new MovieService();
            _databaseService = new DatabaseService();

            SearchCommand = new Command(() =>
            {
                SearchQuery = SearchTerm;

                SearchMovies();
            });
            AllCommand = new Command(() =>
            {
                LoadAllMovies();
                SetActiveButton("All");
                SearchTerm = string.Empty;
            });
            EstrenosCommand = new Command(FilterEstrenos);
            CategoryCommand = new Command(OpenCategoryDialog);
            LoadAllMovies();
            NavigateToDetailCommand = new Command<Movie>(async (movie) =>
            {
                if (movie != null)
                {
                    Console.WriteLine($"Navegando a Detalle de: {movie.Title}");
                    await Shell.Current.GoToAsync($"{nameof(DetailMediaItem)}?MovieId={movie.Id}");
                }
            });

            FavoriteCommand = new Command<Movie>(async (movie) => await ToggleFavorite(movie));

            MessagingCenter.Subscribe<FavoritesViewModel>(this, "FavoriteUpdated", async (sender) =>
            {
                await LoadAllMovies();
                OnPropertyChanged(nameof(Movies));
            });




        }

        private async Task LoadAllMovies()
        {
            
            var movies = await _movieService.GetMoviesAsync(""); // Obtener todas las películas
            _allMovies = movies;
            Movies = _allMovies;

        }

        private void SearchMovies()
        {

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Por favor ingrese alguna palabra para buscar.", "OK");
                return;
            }
            else
            {
                Movies = _allMovies
                    .Where(m => !string.IsNullOrEmpty(m.Title) &&
                                m.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        private void FilterEstrenos()
        {
            Console.WriteLine("Filtrando estrenos...");
            SetActiveButton("Estrenos");

            var seisMesesAtras = DateTime.Now.AddMonths(-1);
            Movies = _allMovies
                .Where(m => DateTime.TryParse(m.ReleaseDate, out var fechaEstreno) && fechaEstreno >= seisMesesAtras)
                .ToList();
        }

        private async void OpenCategoryDialog()
        {
            Console.WriteLine("📂 Abriendo diálogo de categorías...");
            SetActiveButton("Category");

            if (!_categories.Any()) // Cargar categorías solo si aún no se han cargado
            {
                _categories = await _movieService.GetCategoriesAsync() ?? new List<Genre>();
                if (!_categories.Any())
                {
                    Console.WriteLine("⚠ No se encontraron categorías.");
                    return;
                }
            }

            var categoryNames = _categories.Select(c => c.Name).ToArray();
            var selectedCategory = await Application.Current.MainPage.DisplayActionSheet(
                "Seleccione una categoría", "Cancelar", null, categoryNames);

            if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Cancelar")
            {
                Console.WriteLine($"📌 Categoría seleccionada: {selectedCategory}");

                var selectedCategoryId = _categories.FirstOrDefault(c => c.Name == selectedCategory)?.Id;
                if (selectedCategoryId.HasValue)
                {
                    Movies = _allMovies.Where(m => m.GenreIds.Contains(selectedCategoryId.Value)).ToList();

                    if (!Movies.Any())
                    {
                        Console.WriteLine("⚠ No se encontraron películas en esta categoría.");
                    }
                }
            }
        }

        private void SetActiveButton(string selected)
        {
            IsAllActive = selected == "All";
            IsEstrenosActive = selected == "Estrenos";
            IsCategoryActive = selected == "Category";
        }

        private async Task ToggleFavorite(Movie movie)
        {
            if (movie == null) return;

            var existingFavorite = await _databaseService.GetFavoriteByIdAsync(movie.Id);

            if (existingFavorite == null)
            {
                await _databaseService.AddFavoriteAsync(new FavoriteMovie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterPath = movie.PosterPath,
                    ReleaseYear = movie.ReleaseDate?.Substring(0, 4),
                    Rating = $"{movie.Rating}/10"
                });

                movie.IsFavorite = true;
            }
            else
            {
                await _databaseService.RemoveFavoriteAsync(existingFavorite);
                movie.IsFavorite = false;
            }

            // 🔥 ACTUALIZAR LA LISTA DIRECTAMENTE
            var index = Movies.IndexOf(movie);
            if (index >= 0)
            {
                Movies[index] = movie;
                OnPropertyChanged(nameof(Movies));  // 🔥 Esto forzará la actualización en la UI
            }

            // 🔥 ENVIAR MENSAJE PARA ACTUALIZAR LA OTRA VISTA
            MessagingCenter.Send(this, "FavoriteUpdated");
        }




    }
}
