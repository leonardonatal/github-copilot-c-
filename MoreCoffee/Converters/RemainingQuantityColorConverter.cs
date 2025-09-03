using System.Globalization;

namespace MoreCoffee.Converters;

public class RemainingQuantityColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double remainingOunces)
        {
            // Red when below 20% full, yellow when below 50%, green otherwise
            if (remainingOunces <= 0)
                return Colors.Red;
            if (remainingOunces < 2)
                return Colors.OrangeRed;
            if (remainingOunces < 4)
                return Colors.Orange;
            return Colors.Green;
        }
        
        return Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}