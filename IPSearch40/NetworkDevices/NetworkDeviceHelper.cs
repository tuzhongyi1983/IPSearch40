using Howell.Net.NetworkDevice.Common;
using IPSearch40.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSearch40.NetworkDevices
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetworkDeviceHelper
    {
        /// <summary>
        /// 获取OSD配置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OSDConfig GetOSDConfig(DeviceModel model)
        {
            using (NetworkDeviceConnection conn = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateConnection())
            {
                NetworkDeviceConnectionStringBuilder builder = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateConnectionStringBuilder();
                builder.Host = model.IPAddress;
                builder.Port = model.Port;
                builder.Username = model.Username;
                builder.Password = model.Password;
                conn.ConnectionString = new Uri(builder.ToString());
                conn.Open();
                NetworkDeviceDataReader reader = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateDataReader();
                reader.Connection = conn;
                return reader.GetOSDConfig(new Howell.Net.NetworkDevice.MediaIdentifier() { No = 0 });
            }
        }
        /// <summary>
        /// 设置OSD名称
        /// </summary>
        /// <param name="model"></param>
        /// <param name="osdName"></param>
        public static void SetOSDName(DeviceModel model, String osdName)
        {
            using (NetworkDeviceConnection conn = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateConnection())
            {
                NetworkDeviceConnectionStringBuilder builder = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateConnectionStringBuilder();
                builder.Host = model.IPAddress;
                builder.Port = model.Port;
                builder.Username = model.Username;
                builder.Password = model.Password;
                conn.ConnectionString = new Uri(builder.ToString());
                conn.Open();
                NetworkDeviceDataReader reader = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateDataReader();
                reader.Connection = conn;
                OSDConfig osd = reader.GetOSDConfig(new Howell.Net.NetworkDevice.MediaIdentifier() { No = 0 });
                osd.Name = osdName;
                reader.SetOSDConfig(new Howell.Net.NetworkDevice.MediaIdentifier() { No = 0 }, osd);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="osd"></param>
        public static void CopyOSDConfig(DeviceModel model, OSDConfig osd)
        {
            using (NetworkDeviceConnection conn = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateConnection())
            {
                NetworkDeviceConnectionStringBuilder builder = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateConnectionStringBuilder();
                builder.Host = model.IPAddress;
                builder.Port = model.Port;
                builder.Username = model.Username;
                builder.Password = model.Password;
                conn.ConnectionString = new Uri(builder.ToString());
                conn.Open();
                NetworkDeviceDataReader reader = NetworkDeviceProviderFactories.GetFactory(model.ProtocolType).CreateDataReader();
                reader.Connection = conn;
                reader.CopyOSDConfig(new Howell.Net.NetworkDevice.MediaIdentifier() { No = 0 }, osd);
            }
        }
    }
}
