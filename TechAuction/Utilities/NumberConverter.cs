using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace TechAuction.Utilities.NumberConvert
{
    public class NumberToFormattedStringConverter : IValueConverter
    {
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            if (value is decimal number && parameter is string param)
            {
                var parameters = param.Split(';');
                string unit = parameters[0];
                string? singularForm = parameters.Length > 1 ? parameters[1] : null;
                string? pluralForm = parameters.Length > 2 ? parameters[2] : singularForm;

                if (!string.IsNullOrEmpty(singularForm) && !string.IsNullOrEmpty(pluralForm))
                {
                    string unitToUse = number == 1 ? singularForm : pluralForm;
                    return string.Format(CultureInfo.InvariantCulture, "{0:N0} {1}", number, unitToUse);
                }
                else
                {
                    return string.Format(CultureInfo.InvariantCulture, "{0:N0} {1}", number, unit);
                }
            }
            return value!;
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && parameter is string param)
            {
                var parameters = param.Split(';');
                string? unit = parameters[0];
                string? singularForm = parameters.Length > 1 ? parameters[1] : null;
                string? pluralForm = parameters.Length > 2 ? parameters[2] : singularForm;

                if (!string.IsNullOrEmpty(singularForm) && !string.IsNullOrEmpty(pluralForm))
                {
                    string unitToRemove = str.Contains(singularForm) ? singularForm : pluralForm;
                    var numericPart = str.Replace($" {unitToRemove}", "").Replace(".", "");
                    if (decimal.TryParse(numericPart, out decimal result))
                    {
                        return result;
                    }
                }
                else
                {
                    var numericPart = str.Replace($" {unit}", "").Replace(".", "");
                    if (decimal.TryParse(numericPart, out decimal result))
                    {
                        return result;
                    }
                }
            }
            return value!;
        }
    }
}