using HangMan.Forms.CustomRenders;
using HangMan.iOS.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(StackTemplate), typeof(StackTemplateRenderer))]
namespace HangMan.iOS.CustomRender
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
