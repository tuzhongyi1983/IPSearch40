using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSearch40.Excels
{
    /// <summary>
    /// 设备EXCEL数据
    /// </summary>
    public class DeviceExcelData
    {
        /// <summary>
        /// 编号,格式化'0001
        /// </summary>
        [DisplayName("编号")]
        public String No { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称")]
        public String Name { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        [DisplayName("设备类型")]
        public String DeviceType { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        [DisplayName("型号")]
        public String Model { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [DisplayName("IP地址")]
        public String IPAddress { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        [DisplayName("端口号")]
        public Int32 Port { get; set; }
        /// <summary>
        /// 子网掩码
        /// </summary>
        [DisplayName("子网掩码")]
        public String SubnetMask { get; set; }
        /// <summary>
        /// 默认网关
        /// </summary>
        [DisplayName("默认网关")]
        public String Gateway { get; set; }
        /// <summary>
        /// Dns1
        /// </summary>
        [DisplayName("Dns1")]
        public String Dns1 { get; set; }
        /// <summary>
        /// Dns2
        /// </summary>
        [DisplayName("Dns2")]
        public String Dns2 { get; set; }
        /// <summary>
        /// 是否启用DHCP
        /// </summary>
        [DisplayName("DHCP")]
        public String DHCP { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        public String Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        public String Password { get; set; }
        /// <summary>
        /// MAC地址
        /// </summary>
        [DisplayName("MAC地址")]
        public String PhysicalAddress { get; set; }
        /// <summary>
        /// 软件版本
        /// </summary>
        [DisplayName("软件版本")]
        public String Software { get; set; }
        /// <summary>
        /// 硬件版本
        /// </summary>
        [DisplayName("硬件版本")]
        public String Hardware { get; set; }
        /// <summary>
        /// 编码通道数量
        /// </summary>
        [DisplayName("编码通道数量")]
        public Int32 VideoChannelCount { get; set; }
        /// <summary>
        /// 协议类型
        /// </summary>
        [DisplayName("协议类型")]
        public String ProtocolType { get; set; }


    }
}
