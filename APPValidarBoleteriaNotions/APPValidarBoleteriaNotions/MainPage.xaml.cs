using APPValidarBoleteriaClientService;
using APPValidarBoleteriaClientService.Models;
using BarcodeScanner.Mobile;

namespace APPValidarBoleteriaNotions
{
    public partial class MainPage : ContentPage
    {
        bool logueado = false;

        public MainPage()
        {
            InitializeComponent();
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Contexto.Logueado == false)
            {
                await Shell.Current.GoToAsync("LoginPage");
                return;
            }

            if(Contexto.Sincronizado==false)
            {
                await Shell.Current.GoToAsync("ConfiguracionPage");
                return;
            }
        }

        private async void btnValidarQR_Clicked(object sender, EventArgs e)
        {
            #region leer QR
            var tcs = new TaskCompletionSource<List<BarcodeResult>>();

            var pageParams = new Dictionary<string, object>{ { "Parametro", tcs} };

            await Shell.Current.GoToAsync("BarcodePage", pageParams);

            List<BarcodeResult> barCodes = await tcs.Task;


            string valor = barCodes[0].DisplayValue;
            #endregion

            #region ValidarEntrada
            var usuario = Contexto.Usuario;
            var respuesta = await new ControlEntradasClientService().GetValidarEntrada(valor, usuario);

            switch( respuesta.codigo )
            {
                case DTO_Codigo.Valido:
                    {
                        ProcesarRespuestaValida(respuesta,usuario);
                    }
                    break;
                case DTO_Codigo.Invalido:
                    {
                        ProcesarRespuestaNoValida(respuesta);
                    }
                    break;
                case DTO_Codigo.Quemada:
                    {
                        ProcesarRespuestaNoValida(respuesta);
                    }
                    break;
                case DTO_Codigo.Inexistente:
                    {
                        ProcesarRespuestaNoValida(respuesta);
                    }
                    break;
                default: 
                    {
                    }break;
            }
            #endregion
        }


        async private void ProcesarRespuestaValida(DTO_Respuesta<DTO_Entrada> respuesta,string usuario)
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
            var respuesta2 = await new ControlEntradasClientService().GetQuemarEntrada(idRelacionCarrito, usuario);
            #endregion
        }

        private void ProcesarRespuestaNoValida(DTO_Respuesta<DTO_Entrada> respuesta)
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
