using MyAPYREST.View;

namespace MyAPYREST
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Favoritesage), typeof(View.Favoritesage));
            Routing.RegisterRoute(nameof(DetailMediaItem), typeof(View.DetailMediaItem));
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//HomePage");
        }

        private async void OnFavoritesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Favoritesage));
        }
    }
}
