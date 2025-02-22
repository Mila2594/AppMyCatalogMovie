using MyAPYREST.ViewModel;

namespace MyAPYREST.View;

public partial class Favoritesage : ContentPage
{
	public Favoritesage()
	{
		InitializeComponent();
        BindingContext = new FavoritesViewModel();
    }
}