
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

    async private void btnSincronizar_Clicked(object sender, EventArgs e)
    {
        Mensaje.IsVisible = false;

        string ente = enEnte.Text.Trim();

        var respuesta=await new AuthControllerClientService().GetEnpoint(ente);

        if (respuesta.codigo == DTO_CodigoResultado.Success)
        {
            string? enteT = respuesta.datos?.Ente;
            string? endpoint = respuesta.datos?.Endpoint;

            if (string.IsNullOrEmpty(enteT) == false && string.IsNullOrEmpty(endpoint) == false)
            {
                #region persistencia
                var contexto = await new ContextoService().CargarContextoAsync();
                contexto.Ente = enteT;
                contexto.URLEndPoint = endpoint;
                contexto.Sincronizado = true;
                await new ContextoService().GuardarContextoAsync(contexto);
                #endregion

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Mensaje.Show($"Algunos de los campos son nulos: Ente:{enteT}, EndPoint:{endpoint}", "Error en la Respuesta",SetIconos.ICONO_ERROR);
                Mensaje.IsVisible = true;
            }
        }
    }

    protected override bool OnBackButtonPressed()
    {
        //evita el boton de forward 
        return true; 
    }
}