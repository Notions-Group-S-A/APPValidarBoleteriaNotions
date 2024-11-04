using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using APPValidarBoleteriaNotions.Pages;
using APPValidarBoleteriaNotions.Services;
using APPValidarBoleteriaNotions.Utils;
using APPValidarBoleteriaNotions.Views;
using BarcodeScanner.Mobile;
using Microsoft.Maui.Graphics.Text;
using System.Drawing;

namespace APPValidarBoleteriaNotions
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public const int INICIO = 1;
        public const int PRINCIPAL = 2;
        public const int MENSAJE = 3;
        private void HabilitarPanel(int n)
        {
            switch (n)
            {
                case INICIO: 
                    {
                        Panel1.IsVisible = true;
                        Panel2.IsVisible = false;
                        Panel3.IsVisible = false;
                    } break;
                case PRINCIPAL:
                    {
                        Panel1.IsVisible = false;
                        Panel2.IsVisible = true;
                        Panel3.IsVisible = false;
                    }
                    break;
                case MENSAJE:
                    {
                        Panel1.IsVisible = false;
                        Panel2.IsVisible = false;
                        Panel3.IsVisible = true;
                    }
                    break;
            }
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var contexto = await new ContextoService().CargarContextoAsync();

            if (contexto == null || contexto?.Sincronizado == false)
            {
                await Shell.Current.GoToAsync($"{nameof(ConfiguracionPage)}");
                return;
            }

            if (contexto == null || contexto?.IsAuthenticated == false)
            {
                await Shell.Current.GoToAsync($"{nameof(LoginPage)}");
                return;
            }

            HabilitarPanel(INICIO);
        }

        private async void btnValidar_Clicked(object sender, EventArgs e)
        {
            HabilitarPanel(MENSAJE);

            string? codigo = "";
            if (sender == btnValidarQR || sender == btnComenzarValidarQR)
            { 
                codigo = await LeerQR();
            }
            else if (sender == btnValidarHash || sender==btnComenzarValidarHash)
            { 
                codigo = await LeerHash();
            }
            else return;

            await ValidarCodigo(codigo);

            HabilitarPanel(PRINCIPAL);
        }

        async Task<string> LeerQR()
        {
            var tcs = new TaskCompletionSource<List<BarcodeResult>>();

            var pageParams = new Dictionary<string, object> { { "Parametro", tcs } };

            await Shell.Current.GoToAsync($"{nameof(BarcodePage)}", pageParams);

            List<BarcodeResult> barCodes = await tcs.Task;

            string valor = barCodes[0].DisplayValue;

            return valor;
        }

        async Task ValidarCodigo(string qr)
        {
            #region persistencia
            var contexto = await new ContextoService().CargarContextoAsync();
            #endregion

            var respuesta = await new ControlEntradasClientService().ValidarEntrada(qr, contexto.Usuario);

            #region
            Mensaje.IsVisible = false;

            if (respuesta == null || respuesta.codigo == DTO_CodigoEntrada.NO_SUCESS)
                await MostrarError(respuesta?.codigo as int?, respuesta?.mensaje);

            MostrarRespuesta(respuesta);
            
            #endregion
        }

        async Task<string?> LeerHash()
        {
            var tcs = new TaskCompletionSource<List<HashResult>>();

            var pageParams = new Dictionary<string, object> { { "Parametro", tcs } };

            await Shell.Current.GoToAsync($"{nameof(HashPage)}", pageParams);

            List<HashResult> hashCodes = await tcs.Task;

            string? valor = null;

            if (hashCodes != null && hashCodes.Count > 0)
                valor = hashCodes[0].DisplayValue;

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

            #region quemar
            var respuesta2 = await new ControlEntradasClientService().QuemarEntrada(idRelacionCarrito, usuario);
            #endregion
        }

        private void MostrarRespuesta(DTO_RespuestaEntrada<DTO_Entrada> respuesta)
        {
            btnQuemarQR.IsVisible = false;

            btnQuemarQR.IsVisible = false;

            bool mostrarDatosEntrada = false;
            string glyph = "";
            string color = "";

            #region icono
            if (respuesta?.codigo == DTO_CodigoEntrada.Valido) //vigente, puede pasar. 
            {
                glyph = "circle-check";
                color = "#009900";
                mostrarDatosEntrada = true;
                lbSector.Text = respuesta?.datos?.Sector;
                lbSector.TextColor = Colors.Green;

                if (respuesta?.datos?.Quemada == false)
                {
                    btnQuemarQR.IsVisible = true;
                }
            }
            else if (respuesta?.codigo == DTO_CodigoEntrada.Invalido) //vencida
            {
                glyph = "calendar-xmark";
                color = "#CC0000";
                mostrarDatosEntrada = true;
                lbSector.Text = respuesta?.datos?.Sector;
            }
            else if (respuesta?.codigo == DTO_CodigoEntrada.Quemada)//ya fue usada
            {
                glyph = "calendar-xmark";
                color = "#CC0000";
                mostrarDatosEntrada = true;
                lbSector.Text = respuesta.mensaje;
                lbSector.TextColor = Colors.Red;
            }
            else if (respuesta?.codigo == DTO_CodigoEntrada.Inexistente)//no encontrada
            {
                glyph = "circle-xmark";
                color = "#CC0000";
                mostrarDatosEntrada = false;
                lbSector.Text = respuesta?.datos?.Sector;
                lbSector.TextColor = Colors.Green;
            }
            #endregion

            Imagen.Source = new FontImageSource()
            {
                FontFamily = "AwesomeSolid",
                Glyph = glyph,
                Size = 88,
                Color = Microsoft.Maui.Graphics.Color.FromArgb(color)
            };

            if (mostrarDatosEntrada == true)
            {
                lbEntrada.Text = respuesta?.datos?.Codigo;

                lbEvento.Text = respuesta?.datos?.Evento;

                lbFuncion.Text = respuesta?.datos?.Funcion;
            }
        }

        int idEntrada;
        async private void btnQuemarQR_Clicked(object sender, EventArgs e)
        {
            #region persistencia
            var contexto = await new ContextoService().CargarContextoAsync();
            #endregion

            var respuesta = await new ControlEntradasClientService().QuemarEntrada(idEntrada, contexto.Usuario);

            if (respuesta == null || respuesta.codigo == DTO_CodigoEntrada.NO_SUCESS)
            {
                MostrarError(respuesta?.codigo as int?, respuesta?.mensaje);
            }
            else
            {
                //mensaje de ok
                btnQuemarQR.IsVisible = false;

                lbSector.Text = "ok!";
            }
        }

        async private Task<bool> MostrarError(int? codigo, string? mensaje)
        {
            if (codigo == null)
            {
                await Mensaje.Show("Respuesta nula", "Error", SetIconos.ICONO_ERROR);
                Mensaje.IsVisible = true;
                return false;
            }
            else if (codigo == (int?) DTO_CodigoEntrada.NO_SUCESS)
            {
                await Mensaje.Show(mensaje, "Error en la conexión", SetIconos.ICONO_ERROR);
                Mensaje.IsVisible = true;
                return false;
            }
            return true;
        }
    }
}
