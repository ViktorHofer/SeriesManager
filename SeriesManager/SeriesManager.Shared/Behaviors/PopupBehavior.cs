using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SeriesManager.Behaviors
{
    public class PopupBehavior
    {
        public static readonly DependencyProperty CenterPopupProperty =
            DependencyProperty.Register("CenterPopup", typeof(bool), typeof(PopupBehavior), new PropertyMetadata(null, PropertyChangedCallback));

        public static void SetCenterPopup(UIElement element, bool value)
        {
            element.SetValue(CenterPopupProperty, value);
        }

        public static bool GetCenterPopup(UIElement element)
        {
            return (bool)element.GetValue(CenterPopupProperty);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var popup = (Popup)d;
            popup.Loaded += (s, a) => 
                ((FrameworkElement)popup.Child).Loaded += (se, aa) => 
                    ChangePopupPosition(popup, (bool)e.NewValue);

            Window.Current.SizeChanged += (s, a) => ChangePopupPosition(popup, (bool)e.NewValue);
        }

        private static void ChangePopupPosition(Popup popup, bool centerPopup)
        {
            var child = (FrameworkElement)popup.Child;            

            popup.HorizontalOffset = centerPopup ? (Window.Current.Bounds.Width - child.ActualWidth) / 2 : 0;
            popup.VerticalOffset = centerPopup ? (Window.Current.Bounds.Height - child.ActualHeight) / 2 : 0;
        }
    }
}
