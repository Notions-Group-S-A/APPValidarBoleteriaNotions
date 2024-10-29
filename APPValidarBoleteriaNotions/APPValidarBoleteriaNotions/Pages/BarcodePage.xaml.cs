using BarcodeScanner.Mobile;
using System.Reflection.Metadata;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace APPValidarBoleteriaNotions.Pages
{

    [QueryProperty(nameof(Parametro), "Parametro")]
    public partial class BarcodePage : ContentPage
    {
        private TaskCompletionSource<List<BarcodeResult>> _taskCompletionSource;

        public TaskCompletionSource<List<BarcodeResult>> Parametro
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


        public BarcodePage()
        {
            InitializeComponent();

#if ANDROID
            BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeScanner.Mobile.BarcodeFormats.QRCode|BarcodeScanner.Mobile.BarcodeFormats.Code39);
            BarcodeScanner.Mobile.Methods.AskForRequiredPermission();
#endif
        }

        async private void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
        {
            List<BarcodeResult> result = e.BarcodeResults;

            /*
            string result = string.Empty;
            for (int i = 0; i < obj.Count; i++)
            {
                result += $"Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine} ";
            }

            _taskCompletionSource.TrySetResult(result);
            Dispatcher.Dispatch(async () =>
            {
                //await DisplayAlert("Result", result, "OK");

                ResultLabel.Text = result;

                Camera.IsScanning = true;

                await Shell.Current.GoToAsync("..");
            });
            */
            _taskCompletionSource.TrySetResult(result);
            await Shell.Current.GoToAsync("..");
        }

        void SwitchCameraButton_Clicked(object sender, EventArgs e)
        {
            Camera.CameraFacing = Camera.CameraFacing == CameraFacing.Back ? CameraFacing.Front : CameraFacing.Back;
        }

        void TorchButton_Clicked(object sender, EventArgs e)
        {
            Camera.TorchOn = Camera.TorchOn == false;
        }
    }
}