using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Invoice_Free
{
    public class SmartTextColorBasedOnAccentTypeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Debug.WriteLine((bool)value ? ElementTheme.Dark : ElementTheme.Light);
            return (bool)value ? ElementTheme.Dark : ElementTheme.Light;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
