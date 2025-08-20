using System.Globalization;


namespace SquoundApp.Converters
{
	public class EnumEqualsConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is null || parameter is null)
				return false;

			if (value.GetType().IsEnum && parameter.GetType().IsEnum && value.GetType() == parameter.GetType())
				return value.Equals(parameter);

			return false;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (value is bool b && b && parameter is not null)
				return parameter;

			return Binding.DoNothing;
		}
	}
}
