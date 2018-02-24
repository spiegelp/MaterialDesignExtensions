using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensions.Converters
{
    public class FileFiltersTypeConverter : TypeConverter
    {
        public FileFiltersTypeConverter() { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string filterStr)
            {
                return FileFilterHelper.ParseFileFilters(filterStr);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is IList<IFileFilter> filters)
            {
                return FileFilterHelper.ConvertFileFiltersToString(filters);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
