
using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using APPValidarBoleteriaNotions.Services;
using APPValidarBoleteriaNotions.Views;
using System.Net;

namespace APPValidarBoleteriaNotions.Pages;

public partial class ConfiguracionPage : ContentPage
{
    AppValidarEntradaService _authControllerClientService;

    public ConfiguracionPage()
	{
		InitializeComponent();

        _authControllerClientService = new AppValidarEntradaService();
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        var contexto = await new ContextoService().CargarContextoAsync();
        // enEnte.Text= contexto?.Ente;
        lbEnte.Text = "Ente configurado:" + contexto?.Ente;
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

            var respuesta = await _authControllerClientService.GetEnpoint(ente);

            if (respuesta.codigo == DTO_CodigoResultado.Success)
            {
                string? endpoint = respuesta.datos;
                             
                if (string.IsNullOrEmpty(endpoint) == false)
                {
                    #region persistencia
                    
                    var contexto = await new ContextoService().CargarContextoAsync();
                    contexto.URLEndPoint = endpoint;

                    if(contexto.Ente!=ente)
                        contexto.IsAuthenticated = false;

                    contexto.Ente = ente;
                    contexto.IsSincronizado = true;

                    await new ContextoService().GuardarContextoAsync(contexto);
                    #endregion

                    if (contexto.IsAuthenticated == true)
                    {
                        MainPage.PanelActual = 1;
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                else
                {
                    Mensaje.Show($"Se han devuelto algunos campos nulos: Ente:{ente}, EndPoint:{endpoint}", "Error en la Respuesta", SetIconos.ICONO_ERROR);
                    Mensaje.IsVisible = true;
                }
            }
            else
            {
                if (respuesta.codigo == DTO_CodigoResultado.FALLO_RED)
                {
                    Mensaje.Show($"{respuesta.mensaje}", "Error en la Red", SetIconos.ICONO_CONEXION);
                }
                else if (respuesta.codigo == DTO_CodigoResultado.ERROR_RESPUESTA)
                {
                    Mensaje.Show($"{respuesta.mensaje}", "Error en la Respuesta", SetIconos.ICONO_ERROR);
                }
                else
                {
                    Mensaje.Show($"{respuesta.mensaje}", "Error al sincronizar", SetIconos.ICONO_ERROR);
                }
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