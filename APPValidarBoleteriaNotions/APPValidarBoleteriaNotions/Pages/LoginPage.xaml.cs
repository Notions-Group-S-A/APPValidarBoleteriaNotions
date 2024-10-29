using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;

namespace APPValidarBoleteriaNotions.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
    }

    async private void btnLogin_Clicked(object sender, EventArgs e)
    {
        string usuario = enUsuario.Text.Trim();
        string clave = enClave.Text.Trim();

        var respuesta=await new ControlEntradasClientService() {URL_Base=""}.Login(usuario, clave);

        if (respuesta.codigo == DTO_CodigoEntrada.Valido)
        {
            Contexto.Logueado = true;
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            
        }

        
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}