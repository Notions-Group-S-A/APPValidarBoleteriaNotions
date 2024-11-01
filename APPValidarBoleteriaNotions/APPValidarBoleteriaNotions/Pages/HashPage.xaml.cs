using APPValidarBoleteriaNotions.Utils;
using BarcodeScanner.Mobile;

namespace APPValidarBoleteriaNotions.Pages;

[QueryProperty(nameof(Parametro), "Parametro")]
public partial class HashPage : ContentPage
{

    private TaskCompletionSource<List<HashResult>> _taskCompletionSource;

    public TaskCompletionSource<List<HashResult>> Parametro
    {
        get
        {
            return _taskCompletionSource;
        }
        set
        {
            _taskCompletionSource = value;
        }
    }

    public HashPage()
	{
		InitializeComponent();
        enHashCode.Focus();
    }
       
    async private void btnConfirmar_Clicked(object sender, EventArgs e)
    {
        var result = new List<HashResult>();

        result.Add(new HashResult {DisplayValue=enHashCode.Text });

        _taskCompletionSource.TrySetResult(result);

        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    async private void btnVolver_Clicked(object sender, EventArgs e)
    {
        var result = new List<HashResult>();
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    private void enHashCode_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length == enHashCode.MaxLength)
        {
            enHashCode.BackgroundColor = Color.FromArgb("#d1e7dd");
        }
        else
        {
            enHashCode.BackgroundColor = Color.FromArgb("#ecfeff");
        }
    }
}