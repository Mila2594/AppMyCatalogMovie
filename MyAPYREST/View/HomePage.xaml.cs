using MyAPYREST.Models;
using MyAPYREST.ViewModel;

namespace MyAPYREST.View;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new MovieViewModel();


		
    }

   


}