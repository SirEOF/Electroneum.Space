using ElectroneumSpace.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName(nameof(ElectroneumSpace))]
[assembly: ExportEffect(typeof(ControlRemoveBackgroundEffect), nameof(ControlRemoveBackgroundEffect))]
namespace ElectroneumSpace.Droid.Effects
{
    public class ControlRemoveBackgroundEffect : PlatformEffect
    {
        protected override void OnAttached() => Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);

        protected override void OnDetached()
        {
            // Stub
        }
    }
}