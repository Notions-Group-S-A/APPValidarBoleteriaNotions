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

            _taskCompletionSource.TrySetResult(result);
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
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