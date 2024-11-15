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
       
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        var result = new List<HashResult>();
        if (_taskCompletionSource != null && !_taskCompletionSource.Task.IsCompleted)
        {
            _taskCompletionSource.TrySetResult(result); // Devuelve null si no hay resultado
        }
    }


    //protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    //{
    //    base.OnNavigatedFrom(args);

    //    if (BindingContext is Dictionary<string, object> parameters &&
    //        parameters.TryGetValue("Parametro", out var tcsObj) &&
    //        tcsObj is TaskCompletionSource<List<BarcodeResult>> tcs &&
    //        !tcs.Task.IsCompleted)
    //    {
    //        // Completar la tarea con null si el usuario presiona "Atrás"
    //        tcs.TrySetResult(null);
    //    }

    //}
}