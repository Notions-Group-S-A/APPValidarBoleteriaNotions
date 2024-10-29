
using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;

namespace APPValidarBoleteriaNotions.Pages;

public partial class ConfiguracionPage : ContentPage
{
	public ConfiguracionPage()
	{
		InitializeComponent();
	}

    async private void btnSincronizar_Clicked(object sender, EventArgs e)
    {
        PanelMensaje.IsVisible = false;
        lbMensajeTitulo.Text = "";
        lbMensajeDetalle.Text = "";

        string ente = enEnte.Text;

        var respuesta=await new AuthControllerClientService().GetEnpoint(ente);

        if (respuesta != null && respuesta.codigo == DTO_CodigoResultado.Success)
        {
            string? enteT = respuesta.datos?.Ente;
            string? endpoint = respuesta.datos?.Endpoint;

            if (string.IsNullOrEmpty(enteT) == false && string.IsNullOrEmpty(endpoint) == false)
            {
                Contexto.Ente = enteT;
                Contexto.URLEndPoint = endpoint;
                Contexto.Sincronizado = true;

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                PanelMensaje.IsVisible = true;
                string mensaje = $"Algunos de los campos son nulos: Ente:{enteT}, EndPoint:{endpoint}";
                
                lbMensajeTitulo.Text = "Error en la Respuesta";
                lbMensajeDetalle.Text = mensaje;
                mensajeIcono.Glyph = "\uf273";
            }
        }
        else
        {
            PanelMensaje.IsVisible = true;
            lbMensajeTitulo.Text = "Error en la Conexión";
            lbMensajeDetalle.Text = respuesta.mensaje ;
            mensajeIcono.Glyph = "\ue560";
            mensajeIcono.Color = Color.FromArgb("#FF0000");
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true; 
    }
}