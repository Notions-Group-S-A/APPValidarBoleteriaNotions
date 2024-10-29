using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using APPValidarBoleteriaNotions.Services;
using BarcodeScanner.Mobile;
using System.Net;

namespace APPValidarBoleteriaNotions
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var contexto = await new ContextoService().CargarContextoAsync();


            if(contexto == null || contexto?.Sincronizado==false)
            {
                await Shell.Current.GoToAsync("ConfiguracionPage");
                return;
            }

            if (contexto == null || contexto?.Logueado == false)
            {
                await Shell.Current.GoToAsync("LoginPage");
                return;
            }
        }

        private async void btnValidarQR_Clicked(object sender, EventArgs e)
        {
            Panel1.IsVisible = false;
            Panel2.IsVisible = true;

            var qr = await LeerQR();

            await ValidarQR(qr);
        }
      
        async Task<string> LeerQR()
        {
            var tcs = new TaskCompletionSource<List<BarcodeResult>>();

            var pageParams = new Dictionary<string, object> { { "Parametro", tcs } };

            await Shell.Current.GoToAsync("BarcodePage", pageParams);

            List<BarcodeResult> barCodes = await tcs.Task;

            string valor = barCodes[0].DisplayValue;

            return valor;
        }

        async Task ValidarQR(string qr)
        {
            #region persistencia
            var contexto = await new ContextoService().CargarContextoAsync();
            #endregion

            var respuesta = await new ControlEntradasClientService().ValidarEntrada(qr, contexto.Usuario);

            switch (respuesta.codigo)
            {
                case DTO_CodigoEntrada.Valido://ha quemado el qr
                    {
                        //mostrar el boton de quemar
                        ProcesarRespuestaValida(respuesta, contexto.Usuario);
                    }
                    break;
                case DTO_CodigoEntrada.Invalido://la entrada caducada - existe es no vigente
                    {
                        ProcesarRespuestaNoValida(respuesta);
                    }
                    break;
                case DTO_CodigoEntrada.Quemada://es valida
                    {
                        ProcesarRespuestaNoValida(respuesta);
                    }
                    break;
                case DTO_CodigoEntrada.Inexistente://no se encontro 
                    {
                        ProcesarRespuestaNoValida(respuesta);
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }

        async private void ProcesarRespuestaValida(DTO_RespuestaEntrada<DTO_Entrada> respuesta,string usuario)
        {
            var validacion = respuesta?.datos;

            lbEvento.Text = validacion?.Evento;

            lbFuncion.Text = validacion?.Funcion;

            lbSector.Text = validacion?.Sector;

            long idRelacionCarrito = Convert.ToInt64(validacion?.Id_Relacion_Entradas_ItemCarrito);


            Imagen.Source = new FontImageSource() {
                FontFamily = "AwesomeSolid",
                Glyph = "\uf058",//"&#xf058;",
                Size = 88,
                Color = Color.FromArgb("#5f945e")
            };

            #region quemar
            var respuesta2 = await new ControlEntradasClientService().QuemarEntrada(idRelacionCarrito, usuario);
            #endregion
        }

        private void ProcesarRespuestaNoValida(DTO_RespuestaEntrada<DTO_Entrada> respuesta)
        {
            var validacion = respuesta?.datos;

            lbEvento.Text = validacion?.Evento;

            lbFuncion.Text = validacion?.Funcion;

            lbSector.Text = validacion?.Sector;

            Imagen.Source = new FontImageSource()
            {
                FontFamily = "AwesomeSolid",
                Glyph = "\uf273",//"&#xf058;",
                Size = 88,
                Color = Color.FromArgb("#01003B")
            };////

        }
 
    }
}
