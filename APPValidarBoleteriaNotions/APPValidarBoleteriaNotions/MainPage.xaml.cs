using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using APPValidarBoleteriaNotions.Pages;
using APPValidarBoleteriaNotions.Services;
using APPValidarBoleteriaNotions.Utils;
using APPValidarBoleteriaNotions.Views;
using BarcodeScanner.Mobile;
using Microsoft.Maui.Graphics.Text;
using System.Drawing;

namespace APPValidarBoleteriaNotions;

public partial class MainPage : ContentPage
{
    ContextoService _contextoService;
    ControlEntradasClientService _controlEntradasClientService;

    public MainPage()
    {
        InitializeComponent();

        _contextoService = new ContextoService();

        _controlEntradasClientService = new ControlEntradasClientService();
    }

    public static int PanelActual = 1;

    public const int INICIO = 1;
    public const int PRINCIPAL = 2;
    public const int MENSAJE = 3;
    private void HabilitarPanel(int n)
    {
        PanelActual = n;
        switch (n)
        {
            case INICIO:
                {
                    PanelPrincipal.IsVisible = true;
                    PanelResultado.IsVisible = false;
                    PanelEspera.IsVisible = false;

                    lbEvento.Text = "";
                    lbFuncion.Text = "";
                    lbSector.Text = "";
                    lbSector.Text = "";

                }
                break;
            case PRINCIPAL:
                {
                    PanelPrincipal.IsVisible = false;
                    PanelResultado.IsVisible = true;
                    PanelEspera.IsVisible = false;
                }
                break;
            case MENSAJE:
                {
                    //hacer el clear
                    //Mensaje.IsVisible = false;

                    PanelPrincipal.IsVisible = false;
                    PanelResultado.IsVisible = false;
                    PanelEspera.IsVisible = true;
                }
                break;
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        var contexto = await new ContextoService().CargarContextoAsync();

        if (contexto == null || contexto?.IsSincronizado == false)
        {
            await Shell.Current.GoToAsync($"//{nameof(ConfiguracionPage)}");

            _controlEntradasClientService = new ControlEntradasClientService
            {
                URL_Base = contexto.URLEndPoint
            };
            return;
        }

        if (contexto == null || contexto?.IsAuthenticated == false)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            return;
        }

        HabilitarPanel(PanelActual);
    }

    private async void btnValidarEntrada_Clicked(object sender, EventArgs e)
    {
        HabilitarPanel(MENSAJE);

        string? codigo = "";
        if (sender == btnValidarQR || sender == btnComenzarValidarQR)
        {
            codigo = await LeerQR();
        }

        else if (sender == btnComenzarValidarHash)
        {
            codigo = await LeerHash();
        }
        else
        {
            HabilitarPanel(INICIO);
            return;
        }

        if (codigo != null)
        {
            var respuesta = await ValidarCodigo(codigo);

            #region
            if (respuesta == null || respuesta.codigo == DTO_CodigoEntrada.FALLO_RED)
            {
                await Shell.Current.GoToAsync($"{nameof(MensajePage)}");
                HabilitarPanel(INICIO);
                return;
            }
            else if (respuesta.codigo == DTO_CodigoEntrada.RESPUESTA_NO_COMPLETA)
            {
                await DisplayAlert("Error", $"{respuesta.mensaje}", "Cerrar");
                HabilitarPanel(INICIO);
                return;
            }
            else if (respuesta.codigo == DTO_CodigoEntrada.ERROR_RESPUESTA)
            {
                await DisplayAlert("Error", $"{respuesta.mensaje}", "Cerrar");
                HabilitarPanel(INICIO);
                return;
            }

            MostrarRespuesta(respuesta);
        }
        else
        {
            HabilitarPanel(INICIO);
            return;
        }
        #endregion

        HabilitarPanel(PRINCIPAL);
    }

    private void btnValidarSalida_Clicked(object sender, EventArgs e)
    {
        //no implementado todavia
    }

    async Task<string> LeerQR()
    {
        var tcs = new TaskCompletionSource<List<BarcodeResult>>();
        var pageParams = new Dictionary<string, object> { { "Parametro", tcs } };
        await Shell.Current.GoToAsync($"{nameof(BarcodePage)}", pageParams);

        List<BarcodeResult> barCodes = await tcs.Task;

        if (barCodes == null || barCodes.Count == 0)
            return null;

        string valor = barCodes[0].DisplayValue;
        return valor;
    }

    async Task<DTO_RespuestaEntrada<DTO_Entrada>> ValidarCodigo(string qr)
    {
        var contexto = await _contextoService.CargarContextoAsync();

        _controlEntradasClientService = new ControlEntradasClientService
        {
            URL_Base = contexto.URLEndPoint
        };

        var respuesta = await _controlEntradasClientService.ValidarEntrada(qr, contexto.Usuario);
        return respuesta;
    }

    async Task<string?> LeerHash()
    {
        var tcs = new TaskCompletionSource<List<HashResult>>();

        var pageParams = new Dictionary<string, object> { { "Parametro", tcs } };
        await Shell.Current.GoToAsync($"{nameof(HashPage)}", pageParams);
        List<HashResult> hashCodes = await tcs.Task;

        if (hashCodes == null || hashCodes.Count == 0)
            return null;

        string? valor = hashCodes[0].DisplayValue;
        return valor;
    }

    async private void ProcesarRespuestaValida(DTO_RespuestaEntrada<DTO_Entrada> respuesta, string usuario)
    {
        var validacion = respuesta?.datos;
        lbEvento.Text = validacion?.Evento;
        lbFuncion.Text = validacion?.Funcion;
        lbSector.Text = validacion?.Sector;

        long idRelacionCarrito = Convert.ToInt64(validacion?.Id_Relacion_Entradas_ItemCarrito);

        Imagen.Source = new FontImageSource()
        {
            FontFamily = "AwesomeSolid",
            Glyph = "\uf058",//"&#xf058;",
            Size = 88,
            Color = Microsoft.Maui.Graphics.Color.FromArgb("#5f945e")
        };

        var respuesta2 = await new ControlEntradasClientService().QuemarEntrada(idRelacionCarrito, usuario);
    }

    private void MostrarRespuesta(DTO_RespuestaEntrada<DTO_Entrada> respuesta)
    {
        btnQuemarQR.IsVisible = false;

        string glyph = "";
        string color = "";

        #region clear
        lbEntradaLabel.Text = "";
        lbEntradaNumero.Text = "";
        lbEntradaMensaje.Text = "";
        lbEvento.Text = "";
        lbFuncion.Text = "";
        lbFecha.Text = "";
        lbSector.Text = "";
        lbUbicacion.Text = "";
        lbTextoEntrada.Text = "";
        lbNombreEntrada.Text = "";
        btnQuemarQR.IsVisible = false;
        #endregion

        #region icono
        if (respuesta?.codigo == DTO_CodigoEntrada.Valido)
        {
            #region caso entrada vigente 
            glyph = "circle-check";
            color = "#009900";

            lbEntradaLabel.Text = "Ticket: ";
            lbEntradaNumero.Text = $"{respuesta?.datos?.Id}";
            lbEntradaNumero.TextColor = Microsoft.Maui.Graphics.Color.FromArgb("#11b2cf");

            lbEntradaMensaje.Text = $"Ingresos:{respuesta?.datos?.Ingreso_Cantidad} - {respuesta?.datos?.Ingreso_Fecha}";
            lbEntradaMensaje.ClearValue(Label.TextColorProperty);

            lbEvento.Text = respuesta?.datos?.Evento;
            lbFuncion.Text = respuesta?.datos?.Funcion;
            lbFecha.Text = respuesta?.datos?.Funcion_Fecha;
            lbSector.Text = respuesta?.datos?.Sector;
            lbUbicacion.Text = respuesta?.datos?.Ubicacion;
            lbTextoEntrada.Text = respuesta?.datos?.Texto_Entrada;
            lbNombreEntrada.Text = respuesta?.datos?.Nombre_Entrada;
            btnQuemarQR.IsVisible = respuesta?.datos?.Quemada == false;

            idEntrada = respuesta?.datos?.Id_Relacion_Entradas_ItemCarrito ?? 0;
            #endregion
        }
        else if (respuesta?.codigo == DTO_CodigoEntrada.Invalido)
        {
            #region entrada vencida
            glyph = "calendar-xmark";
            color = "#CC0000";

            lbEntradaLabel.Text = "Ticket: ";
            lbEntradaNumero.Text = $"{respuesta?.datos?.Id}";
            lbEntradaNumero.TextColor = Microsoft.Maui.Graphics.Color.FromArgb("#11b2cf");
            lbEntradaMensaje.Text = $"{respuesta?.mensaje}";
            lbEntradaMensaje.TextColor = Colors.Red;

            lbEvento.Text = respuesta?.datos?.Evento;
            lbFuncion.Text = respuesta?.datos?.Funcion;
            lbFecha.Text = respuesta?.datos?.Funcion_Fecha;
            lbSector.Text = respuesta?.datos?.Sector;
            lbUbicacion.Text = respuesta?.datos?.Ubicacion;
            lbTextoEntrada.Text = respuesta?.datos?.Texto_Entrada;
            lbNombreEntrada.Text = respuesta?.datos?.Nombre_Entrada;
            btnQuemarQR.IsVisible = respuesta?.datos?.Quemada == false;

            idEntrada = respuesta?.datos?.Id_Relacion_Entradas_ItemCarrito ?? 0;
            #endregion
        }
        else if (respuesta?.codigo == DTO_CodigoEntrada.Quemada)
        {
            #region fue usada
            glyph = "calendar-xmark";
            color = "#CC0000";

            lbEntradaLabel.Text = "Ticket: ";
            lbEntradaNumero.Text = $"{respuesta?.datos?.Id}";
            lbEntradaNumero.TextColor = Microsoft.Maui.Graphics.Color.FromArgb("#11b2cf");
            lbEntradaMensaje.Text = $"{respuesta?.mensaje}";
            lbEntradaMensaje.TextColor = Colors.Red;

            lbEvento.Text = respuesta?.datos?.Evento;
            lbFuncion.Text = respuesta?.datos?.Funcion;
            lbFecha.Text = respuesta?.datos?.Funcion_Fecha;
            lbSector.Text = respuesta?.datos?.Sector;
            lbUbicacion.Text = respuesta?.datos?.Ubicacion;
            lbTextoEntrada.Text = respuesta?.datos?.Texto_Entrada;
            lbNombreEntrada.Text = respuesta?.datos?.Nombre_Entrada;
            btnQuemarQR.IsVisible = false;

            idEntrada = respuesta?.datos?.Id_Relacion_Entradas_ItemCarrito ?? 0;
            #endregion
        }
        else if (respuesta?.codigo == DTO_CodigoEntrada.Inexistente)
        {
            #region no encontrada
            glyph = "circle-xmark";
            color = "#CC0000";

            lbEntradaMensaje.Text = $"{respuesta?.mensaje}";
            lbEntradaMensaje.TextColor = Colors.Red;
            #endregion
        }
        #endregion

        Imagen.Source = new FontImageSource()
        {
            FontFamily = "AwesomeSolid",
            Glyph = glyph,
            Size = 88,
            Color = Microsoft.Maui.Graphics.Color.FromArgb(color)
        };
    }

    long idEntrada;
    async private void btnQuemarQR_Clicked(object sender, EventArgs e)
    {
        #region persistencia
        var contexto = await _contextoService.CargarContextoAsync();
        #endregion

        _controlEntradasClientService = new ControlEntradasClientService
        {
            URL_Base = contexto.URLEndPoint
        };

        var respuesta = await _controlEntradasClientService.QuemarEntrada(idEntrada, contexto.Usuario);

        if (respuesta == null || respuesta.codigo == DTO_CodigoEntrada.FALLO_RED)
        {
            await Shell.Current.GoToAsync($"{nameof(MensajePage)}");
             return;
        }
        else if (respuesta.codigo == DTO_CodigoEntrada.RESPUESTA_NO_COMPLETA)
        {
            await DisplayAlert("Error", $"{respuesta.mensaje}", "Cerrar");
             return;
        }
        else if (respuesta.codigo == DTO_CodigoEntrada.ERROR_RESPUESTA)
        {
            await DisplayAlert("Error", $"{respuesta.mensaje}", "Cerrar");
            return;
        }
        else
        {
            //mensaje de ok
            btnQuemarQR.IsVisible = false;
            lbSector.Text = "ok!";
        }
    }

    private void btnVolver_Clicked(object sender, EventArgs e)
    {
        HabilitarPanel(INICIO);
    }
}
