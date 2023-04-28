using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ChessRebirth.Utils
{
    public class PieceToDrawingImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || !(values[0] is PieceType) || !(values[1] is PieceColor))
            {
                return DependencyProperty.UnsetValue;
            }

            var type = (PieceType)values[0];
            var color = (PieceColor)values[1];
            string resourceName = $"chess_{type.ToString().ToLower()}_{color.ToString().ToLower()}DrawingImage";

            return Application.Current.Resources[resourceName] as DrawingImage;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
