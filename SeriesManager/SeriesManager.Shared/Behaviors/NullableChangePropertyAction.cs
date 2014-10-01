using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SeriesManager.Behaviors
{
    public class NullableChangePropertyAction : DependencyObject, IAction
    {
        public Control TargetObject
        {
            get { return (Control)GetValue(TargetObjectProperty); }
            set { SetValue(TargetObjectProperty, value); }
        }

        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register("TargetObject", typeof(Control), typeof(NullableChangePropertyAction), new PropertyMetadata(null));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(NullableChangePropertyAction), new PropertyMetadata(null));

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(NullableChangePropertyAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            object obj;
            if (base.ReadLocalValue(TargetObjectProperty) != DependencyProperty.UnsetValue)
            {
                obj = this.TargetObject;
            }
            else
            {
                obj = sender;
            }

            if (obj == null || string.IsNullOrEmpty(this.PropertyName))
            {
                return false;
            }

            this.UpdatePropertyValue(obj);
            return true;
        }

        private void UpdatePropertyValue(object targetObject)
        {
            Type type = targetObject.GetType();
            PropertyInfo runtimeProperty = type.GetRuntimeProperty(this.PropertyName);
            this.ValidateProperty(type.Name, runtimeProperty);
            Exception ex = null;
            try
            {
                Type propertyType = runtimeProperty.PropertyType;
                TypeInfo typeInfo = propertyType.GetTypeInfo();
                object value;
                if (this.Value == null)
                {
                    value = (typeInfo.IsValueType ? Activator.CreateInstance(propertyType) : null);
                    if (propertyType == typeof(string)) value = string.Empty;
                }
                else
                {
                    if (typeInfo.IsAssignableFrom(this.Value.GetType().GetTypeInfo()))
                    {
                        value = this.Value;
                    }
                    else
                    {
                        string text = this.Value.ToString();
                        value = (typeInfo.IsEnum ? Enum.Parse(propertyType, text, false) : TypeConverterHelper.Convert(text, propertyType.FullName));
                    }
                }
                runtimeProperty.SetValue(targetObject, value, new object[0]);
            }
            catch (FormatException ex2)
            {
                ex = ex2;
            }
            catch (ArgumentException ex3)
            {
                ex = ex3;
            }
            if (ex != null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.ChangePropertyActionCannotSetValueExceptionMessage, new object[]
				{
					(this.Value != null) ? this.Value.GetType().Name : "null",
					this.PropertyName,
					runtimeProperty.PropertyType.Name
				}), ex);
            }
        }

        private void ValidateProperty(string targetTypeName, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.ChangePropertyActionCannotFindPropertyNameExceptionMessage, new object[]
				{
					this.PropertyName,
					targetTypeName
				}));
            }
            if (!propertyInfo.CanWrite)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, ResourceHelper.ChangePropertyActionPropertyIsReadOnlyExceptionMessage, new object[]
				{
					this.PropertyName,
					targetTypeName
				}));
            }
        }
    }
}
