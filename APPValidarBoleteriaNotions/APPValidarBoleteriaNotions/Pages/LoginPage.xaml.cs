namespace APPValidarBoleteriaNotions.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    async private void btnLogin_Clicked(object sender, EventArgs e)
    {
        Contexto.Usuario = enUsuario.Text.Trim();

        Contexto.Logueado = true;
        await Shell.Current.GoToAsync("..");
    }
}