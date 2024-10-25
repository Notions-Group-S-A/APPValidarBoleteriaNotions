namespace APPValidarBoleteriaNotions
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(BarcodePage), typeof(BarcodePage));
        }
    }
}
