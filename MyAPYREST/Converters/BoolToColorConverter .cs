using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MyAPYREST.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                // Si el parámetro es "TextColor", determinamos el color del texto según el fondo
                if (parameter != null && parameter.ToString() == "TextColor")
                {
                    return isActive ? Colors.White : Color.FromArgb("#800180"); // Texto blanco si activo, morado si no.
                }
                else
                {
                    return isActive ? Color.FromArgb("#800180") : Colors.White; // Fondo morado si activo, blanco si no.
                }
            }

            return Colors.Transparent; // Si el valor no es válido, devuelve transparente.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
