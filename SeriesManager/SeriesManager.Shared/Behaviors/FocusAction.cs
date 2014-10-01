using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SeriesManager.Behaviors
{
    public class FocusAction : DependencyObject, IAction
    {
        public Control TargetObject
        {
            get { return (Control)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(Control), typeof(FocusAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            if (this.TargetObject != null)
            {
                this.TargetObject.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
            return null;
        }
    }
}
