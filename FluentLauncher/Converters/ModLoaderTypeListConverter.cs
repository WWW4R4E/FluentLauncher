using FluentLauncher.Core.Models;
using Microsoft.UI.Xaml.Data;

namespace FluentLauncher.Core.Converters
{
    public class ModLoaderTypeListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is List<ModLoaderType> modTypes && modTypes != null)
            {
                return string.Join(", ", modTypes);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}