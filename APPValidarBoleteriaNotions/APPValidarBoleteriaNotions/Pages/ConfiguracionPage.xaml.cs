
using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using APPValidarBoleteriaNotions.Services;
using APPValidarBoleteriaNotions.Views;
using System.Net;

namespace APPValidarBoleteriaNotions.Pages;

public partial class ConfiguracionPage : ContentPage
{
	public ConfiguracionPage()
	{
		InitializeComponent();
	}

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        var contexto = await new ContextoService().CargarContextoAsync();
        enEnte.Text= contexto?.Ente;
    }

    async private void btnSincronizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            Mensaje.IsVisible = false;

            string ente = enEnte.Text.Trim();
            if (string.IsNullOrEmpty(ente) == true)
            {
                Mensaje.Show($"Complete el número del ente", "Error", SetIconos.ICONO_ERROR);
                Mensaje.IsVisible = true;
                return;
            }

            var respuesta = await new AuthControllerClientService().GetEnpoint(ente);

            if (respuesta.codigo == DTO_CodigoResultado.Success)
            {
                string? enteT = respuesta.datos?.Ente;
                string? endpoint = respuesta.datos?.Endpoint;

                //evaluo el respuesta
                if (string.IsNullOrEmpty(enteT) == false && string.IsNullOrEmpty(endpoint) == false)
                {
                    #region persistencia
                    var contexto = await new ContextoService().CargarContextoAsync();
                    contexto.Ente = enteT;
                    contexto.URLEndPoint = endpoint;
                    contexto.Sincronizado = true;
                    await new ContextoService().GuardarContextoAsync(contexto);
                    #endregion

                    if (contexto.IsAuthenticated == true)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
                    }
                }
                else
                {
                    Mensaje.Show($"Se han devuelto algunos campos nulos: Ente:{enteT}, EndPoint:{endpoint}", "Error en la Respuesta", SetIconos.ICONO_CONEXION);
                    Mensaje.IsVisible = true;
                }
            }
            else
            {
                Mensaje.Show($"{respuesta.mensaje}", "Error en la conexión", SetIconos.ICONO_CONEXION);
                Mensaje.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            Mensaje.Show($"Error: {ex.Message}\nInner Exception: {ex.InnerException?.Message}\nStack Trace: {ex.StackTrace}", "Error", SetIconos.ICONO_CONEXION);
            Mensaje.IsVisible = true;
        }
    }
        
    protected override bool OnBackButtonPressed()
    {
        //evita el boton de forward 
        return true; 
    }
}