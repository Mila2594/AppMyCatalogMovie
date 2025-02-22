using MyAPYREST.Services;
using System.Diagnostics;
using MyAPYREST.ViewModel;

namespace MyAPYREST.View // Asegúrate de que coincide con el XAML
{

    [QueryProperty(nameof(MovieId), "MovieId")]
    public partial class DetailMediaItem : ContentPage
    {
        public int MovieId { get; set; }
        private readonly MovieService _movieService;

        public DetailMediaItem()
        {
            InitializeComponent();
            _movieService = new MovieService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (MovieId > 0)
            {
                var movie = await _movieService.GetMovieByIdAsync(MovieId);

                if (movie != null)
                {
                    BindingContext = new MovieDetailViewModel(movie);
                }
                else
                {
                    Console.WriteLine(" No se encontró la película.");
                }
            }
        }

    }


}
