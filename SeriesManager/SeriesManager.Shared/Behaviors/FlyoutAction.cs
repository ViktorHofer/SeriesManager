using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Media;
#endif

namespace SeriesManager.Behaviors
{
    public class FlyoutAction : DependencyObject, IAction
    {
        public string Target { get; set; }

        public object Execute(object sender, object parameter)
        {
            var pageType = Type.GetType(Target);

#if WINDOWS_APP
            var flyout = Activator.CreateInstance(pageType) as SettingsFlyout;
            if (flyout != null)
            {
                flyout.ShowIndependent();
                return true;
            }
            return false;
#elif WINDOWS_PHONE_APP
            var frame = GetFrame(sender as DependencyObject);
            return frame.Navigate(pageType);
#endif
        }

#if WINDOWS_PHONE_APP
        private Frame GetFrame(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            var parentFrame = parent as Frame;
            if (parentFrame != null) return parentFrame;
            return GetFrame(parent);
        }
#endif
    }
}
