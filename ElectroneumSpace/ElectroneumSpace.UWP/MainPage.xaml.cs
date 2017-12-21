using Microsoft.Practices.Unity;
using Prism.Unity;
using Syncfusion.ListView.XForms.UWP;

namespace ElectroneumSpace.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            SfListViewRenderer.Init();
            LoadApplication(new ElectroneumSpace.App(new UwpInitializer()));
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}
