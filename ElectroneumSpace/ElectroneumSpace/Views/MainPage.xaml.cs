using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ElectroneumSpace.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

            PerformInitialConfiguration();
		}

        void PerformInitialConfiguration()
        {
            // Remove the labels for buttons unless on Android (Android needs them for the margin problem)
            if (Device.RuntimePlatform.Equals(Device.Android))
            {
                PoolButtonLabel.IsVisible = true;
                StatsButtonLabel.IsVisible = true;
                BlocksButtonLabel.IsVisible = true;
                DonateButtonLabel.IsVisible = true;
                SettingsButtonLabel.IsVisible = true;
            }

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}