//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Globalization;
using System.Windows.Data;
using WPFHexaEditor.Core.Bytes;

namespace WPFHexaEditor.Core.Converters
{
    /// <summary>
    /// Used to convert hexadecimal to Long value.
    /// </summary>
    public sealed class HexToLongStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ByteConverters.IsHexaValue(value.ToString()))
                return ByteConverters.HexLiteralToLong(value.ToString());
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}