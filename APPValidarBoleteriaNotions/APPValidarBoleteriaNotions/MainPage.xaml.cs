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
                await Shell.Current.GoToAsync("login");
            }
        }

        private async void btnGoTo_Clicked(object sender, EventArgs e)
        {
            var tcs = new TaskCompletionSource<string>();

            var pageParams = new Dictionary<string, object>{ { "Parametro", tcs} };

            await Shell.Current.GoToAsync("BarcodePage", pageParams);

            string scannedValue = await tcs.Task;
            await DisplayAlert("Escaneo completado", $"Valor escaneado: {scannedValue}", "OK");
        }
    }
}
