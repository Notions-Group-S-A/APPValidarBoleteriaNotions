using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using APPValidarBoleteriaNotions.Services;
using APPValidarBoleteriaNotions.Views;
using System.Net;

namespace APPValidarBoleteriaNotions.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
    }

    async private void btnLogin_Clicked(object sender, EventArgs e)
    {
        var contexto = await new ContextoService().CargarContextoAsync();

        Mensaje.IsVisible = false;

        string usuario = enUsuario.Text.Trim();
        string clave = enClave.Text.Trim();

        var respuesta=await new ControlEntradasClientService() {URL_Base= contexto?.URLEndPoint }.Login(usuario, clave);

        if (respuesta.codigo == DTO_CodigoEntrada.Valido)
        {
            #region persistencia
            contexto.Logueado = true;
            await new ContextoService().GuardarContextoAsync(contexto);
            #endregion

            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Mensaje.Show(respuesta.mensaje, "Error en la Respuesta", SetIconos.ICONO_ERROR);
            Mensaje.IsVisible = true;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        //evita el boton de forward 
        return true;
    }
}