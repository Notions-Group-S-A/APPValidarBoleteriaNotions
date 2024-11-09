namespace APPValidarBoleteriaNotions.Pages;

public partial class MensajePage : ContentPage
{
	public MensajePage()
	{
		InitializeComponent();
	}

    private void btnReintentar_Clicked(object sender, EventArgs e)
    {
        bool tieneInternet = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        if(tieneInternet==true)
            Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }
}