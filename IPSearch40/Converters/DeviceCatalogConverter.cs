using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace IPSearch40.Converters
{
    /// <summary>
    /// 设备分类转换工具
    /// </summary>
    [ValueConversion(typeof(string), typeof(DeviceCatalog))]
    public class DeviceCatalogConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DeviceCatalog)
            {
                var catalog = (DeviceCatalog)value;
                if (catalog == DeviceCatalog.Normal)
                    return "普通设备";
                else
                    return "智能设备";
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is String)
            {
                if((String)value == "普通设备")
                {
                    return DeviceCatalog.Normal;
                }
                else
                {
                    return DeviceCatalog.Intelligent;
                }
            }
            return DeviceCatalog.Normal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String [] GetValues()
        {
            return new String[] { "普通设备", "智能设备" };
        }
    }
}
