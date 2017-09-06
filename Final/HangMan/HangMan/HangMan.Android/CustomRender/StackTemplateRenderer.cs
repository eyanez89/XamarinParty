using HangMan.Droid.CustomRender;
using HangMan.Forms.CustomRenders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(StackTemplate), typeof(StackTemplateRenderer))]
namespace HangMan.Droid.CustomRender
{
    public class StackTemplateRenderer : VisualElementRenderer<StackLayout>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as StackTemplate;
            element?.Render();
        }
    }
}