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
	}

    async private void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
    {
        
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    async private void btnConfirmar_Clicked(object sender, EventArgs e)
    {
        var result = new List<HashResult>();

        _taskCompletionSource.TrySetResult(result);

        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    //private void EntryHash_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    if (sender is Entry entry)
    //    {
    //        var plainText = entry.Text.Replace("-", "");

    //        var formattedText = string.Empty;
    //        for (int i = 0; i < plainText.Length; i++)
    //        {
    //            if (i > 0 && i % 2 == 0)
    //            {
    //                formattedText += "-";
    //            }
    //            formattedText += plainText[i];
    //        }

    //        if (entry.Text != formattedText)
    //        {
    //            entry.TextChanged -= EntryHash_TextChanged; // Evitar bucle infinito
    //            entry.Text = formattedText;
    //            entry.CursorPosition = formattedText.Length; // Mover el cursor al final
    //            entry.TextChanged += EntryHash_TextChanged; // Volver a suscribirse
    //        }
    //    }
    //}

    string hashCode;

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        var entry = sender as Entry;

        // Verificar si se completaron 2 caracteres
        if (entry.Text.Length == 2)
        {
            // Mover al siguiente Entry
            if (entry == entry1)
                entry2.Focus();
            else if (entry == entry2)
                entry3.Focus();
            else if (entry == entry3)
                entry4.Focus();
        }

        hashCode = $"{entry1.Text}{entry2.Text}{entry3.Text}{entry4.Text}";
    }

    // Manejar el evento de eliminación de texto
    private void Entry_Focused(object sender, FocusEventArgs e)
    {
        var entry = sender as Entry;
        entry.TextChanged += Entry_TextChanged;
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // Si el texto se ha eliminado, mover el foco al anterior
        if (e.NewTextValue.Length < e.OldTextValue.Length)
        {
            if (entry == entry2)
                entry1.Focus();
            else if (entry == entry3)
                entry2.Focus();
            else if (entry == entry4)
                entry3.Focus();
        }
        hashCode = $"{entry1.Text}{entry2.Text}{entry3.Text}{entry4.Text}";
    }

    //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    var entry = sender as Entry;

    //    Si el texto es completo(2 caracteres), mueve el foco al siguiente Entry
    //    if (string.IsNullOrEmpty(entry.Text))
    //    {
    //        if (entry.Text.Length >= 2)
    //        {
    //            if (entry == entry1)
    //                entry2.Focus();
    //            else if (entry == entry2)
    //                entry3.Focus();
    //            else if (entry == entry3)
    //                entry4.Focus();
    //        }
    //    }
    //    else if (entry.Text.Length >= 2)
    //    {
    //        Si el texto es completo(2 caracteres), mueve el foco al siguiente Entry
    //        if (entry == entry1)
    //            entry2.Focus();
    //        else if (entry == entry2)
    //            entry3.Focus();
    //        else if (entry == entry3)
    //            entry4.Focus();
    //    }

    //    hashCode = $"{entry1.Text}{entry2.Text}{entry3.Text}{entry4.Text}";
    //}
}