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
        SetUpEventHandlers();
    }
       
    async private void btnConfirmar_Clicked(object sender, EventArgs e)
    {
        var result = new List<HashResult>();

        _taskCompletionSource.TrySetResult(result);

        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    async private void btnVolver_Clicked(object sender, EventArgs e)
    {
        var result = new List<HashResult>();
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }


    string hashCode;

    private void SetUpEventHandlers()
    {
        entry1.Focus();
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        if (sender == entry1 && entry1.Text.Length == entry1.MaxLength)
            entry2.Focus();
        else if (sender == entry2 && entry2.Text.Length == entry2.MaxLength)
            entry3.Focus();
        else if (sender == entry3 && entry3.Text.Length == entry3.MaxLength)
            entry4.Focus();
    }

    private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        var currentEntry = sender as Entry;

        if (string.IsNullOrEmpty(currentEntry.Text) && e.OldTextValue?.Length == 1)
        {
            if (currentEntry == entry4)
                entry3.Focus();
            else if (currentEntry == entry3)
                entry2.Focus();
            else if (currentEntry == entry2)
                entry1.Focus();
        }
    }

    private void entry1_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender == entry1 && entry1.Text.Length == entry1.MaxLength)
        {
            entry2.Focus();

        }
    }
}