using System;
using Windows.ApplicationModel.Resources;

namespace SeriesManager.Behaviors
{
    internal static class ResourceHelper
    {
        public static string CallMethodActionValidMethodNotFoundExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CallMethodActionValidMethodNotFoundExceptionMessage");
            }
        }
        public static string ChangePropertyActionCannotFindPropertyNameExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("ChangePropertyActionCannotFindPropertyNameExceptionMessage");
            }
        }
        public static string ChangePropertyActionCannotSetValueExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("ChangePropertyActionCannotSetValueExceptionMessage");
            }
        }
        public static string ChangePropertyActionPropertyIsReadOnlyExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("ChangePropertyActionPropertyIsReadOnlyExceptionMessage");
            }
        }
        public static string GoToStateActionTargetHasNoStateGroups
        {
            get
            {
                return ResourceHelper.GetString("GoToStateActionTargetHasNoStateGroups");
            }
        }
        public static string CannotAttachBehaviorMultipleTimesExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CannotAttachBehaviorMultipleTimesExceptionMessage");
            }
        }
        public static string CannotFindEventNameExceptionMessage
        {
            get
            {
                return ResourceHelper.GetString("CannotFindEventNameExceptionMessage");
            }
        }
        public static string InvalidLeftOperand
        {
            get
            {
                return ResourceHelper.GetString("InvalidLeftOperand");
            }
        }
        public static string InvalidRightOperand
        {
            get
            {
                return ResourceHelper.GetString("InvalidRightOperand");
            }
        }
        public static string InvalidOperands
        {
            get
            {
                return ResourceHelper.GetString("InvalidOperands");
            }
        }
        public static string GetString(string resourceName)
        {
            ResourceLoader forCurrentView = ResourceLoader.GetForCurrentView("Microsoft.Xaml.Interactions/Strings");
            return forCurrentView.GetString(resourceName);
        }
    }
}
