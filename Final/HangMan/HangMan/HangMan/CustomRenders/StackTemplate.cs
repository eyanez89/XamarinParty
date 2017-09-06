using System.Collections;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace HangMan.Forms.CustomRenders
{
    public class StackTemplate : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(StackTemplate), default(IEnumerable));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(StackTemplate), default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public void Render()
        {
            if (this.ItemTemplate == null || this.ItemsSource == null)
                return;

            if (Children.Count > 0)
            {
                int count = Children.Count - 1;

                while (count > -1)
                {
                    Children.RemoveAt(count);
                    count--;
                }
            }

            foreach (var item in this.ItemsSource)
            {
                var viewCell = this.ItemTemplate.CreateContent() as ViewCell;
                viewCell.View.BindingContext = item;
                Children.Add(viewCell.View);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(ItemsSource))
                Render();
        }
    }
}