using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyAPYREST.Converters
{
    public class BoolToFavoriteImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorite && isFavorite)
            {
                return "favorite.png"; // Imagen para favoritos activados
            }
            return "favorite_border.png"; // Imagen para favoritos desactivados
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
