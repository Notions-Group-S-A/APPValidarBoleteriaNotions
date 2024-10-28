
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
        string ente = enEnte.Text;

        var respuesta=await new AuthControllerClientService().GetEnpoint(ente);
                
        Contexto.Ente = respuesta?.datos?.Ente;
        Contexto.URLEndPoint = respuesta?.datos?.Endpoint;
        Contexto.Sincronizado = true;
        
        Contexto.Logueado = true;
        await Shell.Current.GoToAsync("..");
    }
}