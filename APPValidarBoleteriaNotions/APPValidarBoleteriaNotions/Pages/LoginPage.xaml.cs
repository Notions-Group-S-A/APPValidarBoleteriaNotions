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
        Mensaje.IsVisible = false;

        var contexto = await new ContextoService().CargarContextoAsync();
        
        string usuario = enUsuario.Text.Trim();
        string clave = enClave.Text.Trim();

        if (string.IsNullOrEmpty(usuario) == true || string.IsNullOrEmpty(clave) == true)
        {
            Mensaje.Show("Complete usuario/contraseņa", "Error!", SetIconos.ICONO_ERROR);
            Mensaje.IsVisible = true;
            return;
        }

        if (contexto == null)
        {
            Mensaje.Show("Error al guardar las credenciales en el dispositivo", "Error", SetIconos.ICONO_ERROR);
            Mensaje.IsVisible = true;
            return;
        }

       var respuesta=await new ControlEntradasClientService() {URL_Base= contexto?.URLEndPoint }.Login(usuario, clave);

        if (respuesta!=null && respuesta.codigo == DTO_CodigoEntrada.Valido)
        {
            #region persistencia
            contexto.IsAuthenticated = true;
            contexto.Usuario = usuario;

            await new ContextoService().GuardarContextoAsync(contexto);
            #endregion

            if(App.Current!=null)
                ((AppShell)App.Current.MainPage).Usuario= usuario;

            MainPage.PanelActual = 1;
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            Mensaje.Show(respuesta.mensaje, "Error en la Respuesta", SetIconos.ICONO_ERROR);
            Mensaje.IsVisible = true;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        //evita el boton de forward 
        return true;
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        enClave.IsPassword = !enClave.IsPassword;
        btnTogglePassword.Text = enClave.IsPassword ? "Mostrar" : "Ocultar";
    }
}