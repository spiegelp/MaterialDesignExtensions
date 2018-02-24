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
            return sourceType == typeof(string) || IsOfTypeFileFiltersList(sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is string filterStr)
            {
                return FileFilterHelper.ParseFileFilters(filterStr);
            }
            else if (value is IList<IFileFilter> filters)
            {
                return filters;
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return IsOfTypeFileFiltersList(destinationType) || destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }

            if (value is IList<IFileFilter> filters)
            {
                return FileFilterHelper.ConvertFileFiltersToString(filters);
            }
            else if (value is string filterStr)
            {
                return filterStr;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        private bool IsOfTypeFileFiltersList(Type type)
        {
            Type typeItem = type;

            while (typeItem != null)
            {
                if (typeItem == typeof(IList<IFileFilter>))
                {
                    return true;
                }

                typeItem = typeItem.BaseType;
            }

            return false;
        }
    }
}
