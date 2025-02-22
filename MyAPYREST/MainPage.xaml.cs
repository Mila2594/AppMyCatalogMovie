namespace MyAPYREST
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage()
        {
            InitializeComponent();
        }

        private async void goToApiRestPageCliked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ApiRestPage");
        }

        private async void goToApiGratuitaPageCliked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ApiGratuitaPage");
        }

       
    }

}
