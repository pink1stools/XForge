//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Globalization;
using System.Windows.Data;

namespace WPFHexaEditor.Core.Converters
{
    /// <summary>
    /// Permet d'inverser des bool
    /// </summary>
    public sealed class BoolInverterConverter : IValueConverter
    {
  
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = null;

            try
            {
                val = (bool)value;
            }
            catch { }


            if (val != null)
                return !val;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
