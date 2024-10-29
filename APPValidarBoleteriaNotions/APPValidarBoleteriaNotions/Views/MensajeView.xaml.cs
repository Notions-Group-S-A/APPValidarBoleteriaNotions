namespace APPValidarBoleteriaNotions.Views;

public partial class MensajeView : ContentView
{
	

	public MensajeView()
	{
		InitializeComponent();
	}

	async public Task Show(string contenido, string titulo, Icono icono)
	{
        lbMensajeTitulo.Text = titulo;
        lbMensajeDetalle.Text = contenido;
		mensajeIcono.Glyph = icono.Glyph;
		mensajeIcono.Color = icono.GetColor;
    }
}

public class Icono
{
	public string Glyph { get; set; }

	public string Color { get; set; }

	public Microsoft.Maui.Graphics.Color GetColor { get { return Microsoft.Maui.Graphics.Color.FromArgb(Color); } }

}

static public class SetIconos
{
	static public  Icono ICONO_ERROR = new Icono { Color= "#FF0000", Glyph="\ue560" };
    static public Icono ICONO_CONEXION = new Icono { Color = "#FF0000", Glyph = "\ue560" };
}