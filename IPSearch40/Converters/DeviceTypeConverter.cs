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
    /// 设备类型转换工具
    /// </summary>
    [ValueConversion(typeof(string), typeof(DeviceType))]
    public class DeviceTypeConverter : IValueConverter
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
            if (value is DeviceType)
            {
                var type = (DeviceType)value;
                if (type == DeviceType.DVS)
                    return "DVS";
                else if (type == DeviceType.FaceCamera)
                    return "人脸抓拍摄像机";
                else if (type == DeviceType.IPCamera)
                    return "IP摄像机";
                else if (type == DeviceType.NVR)
                    return "NVR";
                else if (type == DeviceType.HDDecoder)
                    return "高清解码器";
                else if (type == DeviceType.IntelligentCamera)
                    return "智能摄像机";
                else if (type == DeviceType.MicroIntelligentNVR)
                    return "轻智能NVR";
                else if (type == DeviceType.FaceNVR)
                    return "智能NVR";
                else if (type == DeviceType.All)
                    return "全部";
                else if (type == DeviceType.VehicleCamera)
                    return "车辆抓拍摄像机";
                else
                    return "未知类型";
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
            if (value is String)
            {
                if ((String)value == "IP摄像机")
                {
                    return DeviceType.IPCamera;
                }
                else if ((String)value == "NVR")
                {
                    return DeviceType.NVR;
                }
                else if ((String)value == "DVS")
                {
                    return DeviceType.DVS;
                }
                else if ((String)value == "人脸抓拍摄像机")
                {
                    return DeviceType.FaceCamera;
                }
                else if ((String)value == "高清解码器")
                {
                    return DeviceType.HDDecoder;
                }
                else if ((String)value == "智能摄像机")
                {
                    return DeviceType.IntelligentCamera;
                }
                else if ((String)value == "轻智能NVR")
                {
                    return DeviceType.MicroIntelligentNVR;
                }
                else if ((String)value == "智能NVR")
                {
                    return DeviceType.FaceNVR;
                }
                else if((String)value == "全部")
                {
                    return DeviceType.All;
                }
                else if((String)value == "车辆抓拍摄像机")
                {
                    return DeviceType.VehicleCamera;
                }
                else
                {
                    return DeviceType.None;
                }
            }
            return DeviceType.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static String[] GetValues()
        {
            return new String[] { "全部", "未知类型", "IP摄像机","NVR","DVS","高清解码器","智能摄像机","人脸抓拍摄像机", "轻智能NVR", "智能NVR","车辆抓拍摄像机" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String GetString(DeviceType type)
        {
            if (type == DeviceType.DVS)
                return "DVS";
            else if (type == DeviceType.FaceCamera)
                return "人脸抓拍摄像机";
            else if (type == DeviceType.IPCamera)
                return "IP摄像机";
            else if (type == DeviceType.NVR)
                return "NVR";
            else if (type == DeviceType.HDDecoder)
                return "高清解码器";
            else if (type == DeviceType.IntelligentCamera)
                return "智能摄像机";
            else if (type == DeviceType.MicroIntelligentNVR)
                return "轻智能NVR";
            else if (type == DeviceType.FaceNVR)
                return "智能NVR";
            else if (type == DeviceType.All)
                return "全部";
            else if (type == DeviceType.VehicleCamera)
                return "车辆抓拍摄像机";
            else
                return "未知类型";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DeviceType GetDeviceType(String value)
        {
            if ((String)value == "IP摄像机")
            {
                return DeviceType.IPCamera;
            }
            else if ((String)value == "NVR")
            {
                return DeviceType.NVR;
            }
            else if ((String)value == "DVS")
            {
                return DeviceType.DVS;
            }
            else if ((String)value == "人脸抓拍摄像机")
            {
                return DeviceType.FaceCamera;
            }
            else if ((String)value == "高清解码器")
            {
                return DeviceType.HDDecoder;
            }
            else if ((String)value == "智能摄像机")
            {
                return DeviceType.IntelligentCamera;
            }
            else if ((String)value == "轻智能NVR")
            {
                return DeviceType.MicroIntelligentNVR;
            }
            else if ((String)value == "智能NVR")
            {
                return DeviceType.FaceNVR;
            }
            else if ((String)value == "全部")
            {
                return DeviceType.All;
            }
            else if ((String)value == "车辆抓拍摄像机")
            {
                return DeviceType.VehicleCamera;
            }
            else
            {
                return DeviceType.None;
            }
        }
    }
}
