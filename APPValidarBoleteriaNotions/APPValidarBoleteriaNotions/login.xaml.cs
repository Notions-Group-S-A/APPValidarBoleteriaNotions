namespace APPValidarBoleteriaNotions;

public partial class login : ContentPage
{
	public login()
	{
		InitializeComponent();
	}

    async private void btnIntresar_Clicked(object sender, EventArgs e)
    {
        Contexto.Logueado = true;
        await Shell.Current.GoToAsync("..");
    }
}