using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace ElectroneumSpace.Droid
{
    [Activity(Label = "ElectroneumSpace", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            CarouselViewRenderer.Init();

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {
            // Register any platform specific implementations
        }
    }
}

