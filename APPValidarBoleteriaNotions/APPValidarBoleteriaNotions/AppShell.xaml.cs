using APPValidarBoleteriaNotions.Pages;

namespace APPValidarBoleteriaNotions;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MensajePage), typeof(MensajePage));
        Routing.RegisterRoute(nameof(HashPage), typeof(HashPage));
        Routing.RegisterRoute(nameof(BarcodePage), typeof(BarcodePage));
        Routing.RegisterRoute(nameof(ConfiguracionPage), typeof(ConfiguracionPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
       
    }

    public string Usuario
    {
        get => lbUsuario.Text;
        set => lbUsuario.Text = value;
    }
}
