using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IPSearch40.Models
{
    /// <summary>
    /// 设备
    /// </summary>
    public class DeviceModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private Boolean m_IsSelected = false;
        /// <summary>
        /// 选中
        /// </summary>
        [DisplayName("选中")]
        public Boolean IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                if (Equals(value, m_IsSelected)) return;
                m_IsSelected = value;
                OnPropertyChanged();
            }
        }

        private String m_No;
        /// <summary>
        /// 编号,格式化'0001
        /// </summary>
        [DisplayName("编号")]
        public String No
        {
            get { return m_No; }
            set
            {
                if (Equals(value, m_No)) return;
                m_No = value;
                OnPropertyChanged();
            }
        }
        private String m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称")]
        public String Name
        {
            get { return m_Name; }
            set
            {
                if (Equals(value, m_Name)) return;
                m_Name = value;
                OnPropertyChanged();
            }
        }

        private DeviceType m_DeviceType;
        /// <summary>
        /// 设备类型
        /// </summary>
        [DisplayName("设备类型")]
        public DeviceType DeviceType
        {
            get { return m_DeviceType; }
            set
            {
                if (Equals(value, m_DeviceType)) return;
                m_DeviceType = value;
                OnPropertyChanged();
            }
        }


        private String m_Model;
        /// <summary>
        /// 设备型号
        /// </summary>
        [DisplayName("型号")]
        public String Model
        {
            get { return m_Model; }
            set
            {
                if (Equals(value, m_Model)) return;
                m_Model = value;
                OnPropertyChanged();
            }
        }
        private String m_SerialNumber;
        /// <summary>
        /// 设备序列号
        /// </summary>
        [DisplayName("序列号")]
        public String SerialNumber
        {
            get { return m_SerialNumber; }
            set
            {
                if (Equals(value, m_SerialNumber)) return;
                m_SerialNumber = value;
                OnPropertyChanged();
            }
        }
        private String m_PhysicalAddress;
        /// <summary>
        /// MAC地址
        /// </summary>
        [DisplayName("MAC地址")]
        public String PhysicalAddress
        {
            get { return m_PhysicalAddress; }
            set
            {
                if (Equals(value, m_PhysicalAddress)) return;
                m_PhysicalAddress = value;
                OnPropertyChanged();
            }
        }
        private String m_IPAddress;
        /// <summary>
        /// IP地址
        /// </summary>
        [DisplayName("IP地址")]
        public String IPAddress
        {
            get { return m_IPAddress; }
            set
            {
                if (Equals(value, m_IPAddress)) return;
                m_IPAddress = value;
                OnPropertyChanged();
            }
        }
        private Int32 m_Port;
        /// <summary>
        /// 端口号
        /// </summary>
        [DisplayName("端口号")]
        public Int32 Port
        {
            get { return m_Port; }
            set
            {
                if (Equals(value, m_Port)) return;
                m_Port = value;
                OnPropertyChanged();
            }
        }
        private String m_SubnetMask;
        /// <summary>
        /// 子网掩码
        /// </summary>
        [DisplayName("子网掩码")]
        public String SubnetMask
        {
            get { return m_SubnetMask; }
            set
            {
                if (Equals(value, m_SubnetMask)) return;
                m_SubnetMask = value;
                OnPropertyChanged();
            }
        }
        private String m_Gateway;
        /// <summary>
        /// 默认网关
        /// </summary>
        [DisplayName("默认网关")]
        public String Gateway
        {
            get { return m_Gateway; }
            set
            {
                if (Equals(value, m_Gateway)) return;
                m_Gateway = value;
                OnPropertyChanged();
            }
        }
        private String m_Dns1;
        /// <summary>
        /// Dns1
        /// </summary>
        [DisplayName("Dns1")]
        public String Dns1
        {
            get { return m_Dns1; }
            set
            {
                if (Equals(value, m_Dns1)) return;
                m_Dns1 = value;
                OnPropertyChanged();
            }
        }
        private String m_Dns2;
        /// <summary>
        /// Dns2
        /// </summary>
        [DisplayName("Dns2")]
        public String Dns2
        {
            get { return m_Dns2; }
            set
            {
                if (Equals(value, m_Dns2)) return;
                m_Dns2 = value;
                OnPropertyChanged();
            }
        }
        private Boolean m_DHCP = false;
        /// <summary>
        /// 是否启用DHCP
        /// </summary>
        [DisplayName("DHCP")]
        public Boolean DHCP
        {
            get { return m_DHCP; }
            set
            {
                if (Equals(value, m_DHCP)) return;
                m_DHCP = value;
                OnPropertyChanged();
            }
        }
        private String m_Username;
        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        public String Username
        {
            get { return m_Username; }
            set
            {
                if (Equals(value, m_Username)) return;
                m_Username = value;
                OnPropertyChanged();
            }
        }
        private String m_Password;
        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        public String Password
        {
            get { return m_Password; }
            set
            {
                if (Equals(value, m_Password)) return;
                m_Password = value;
                OnPropertyChanged();
            }
        }
        private String m_Software;
        /// <summary>
        /// 软件版本
        /// </summary>
        [DisplayName("软件版本")]
        public String Software
        {
            get { return m_Software; }
            set
            {
                if (Equals(value, m_Software)) return;
                m_Software = value;
                OnPropertyChanged();
            }
        }
        private String m_Hardware;
        /// <summary>
        /// 硬件版本
        /// </summary>
        [DisplayName("硬件版本")]
        public String Hardware
        {
            get { return m_Hardware; }
            set
            {
                if (Equals(value, m_Hardware)) return;
                m_Hardware = value;
                OnPropertyChanged();
            }
        }
        private Int32 m_VideoChannelCount;
        /// <summary>
        /// 编码通道数量
        /// </summary>
        [DisplayName("编码通道数量")]
        public Int32 VideoChannelCount
        {
            get { return m_VideoChannelCount; }
            set
            {
                if (Equals(value, m_VideoChannelCount)) return;
                m_VideoChannelCount = value;
                OnPropertyChanged();
            }
        }

        private String m_ProtocolType;
        /// <summary>
        /// 协议类型
        /// </summary>
        [DisplayName("协议类型")]
        public String ProtocolType
        {
            get { return m_ProtocolType; }
            set
            {
                if (Equals(value, m_ProtocolType)) return;
                m_ProtocolType = value;
                OnPropertyChanged();
            }
        }


        private Boolean m_IPConflict;
        /// <summary>
        /// 是否有IP冲突
        /// </summary>
        [DisplayName("是否有IP冲突")]
        public Boolean IPConflict
        {
            get { return m_IPConflict; }
            set
            {
                if (Equals(value, m_IPConflict)) return;
                m_IPConflict = value;
                OnPropertyChanged();
            }
        }

        private Nullable<Boolean> m_SettingSucceed;
        /// <summary>
        /// 设置成功
        /// </summary>
        [DisplayName("设置成功")]
        public Nullable<Boolean> SettingSucceed
        {
            get { return m_SettingSucceed; }
            set
            {
                if (Equals(value, m_SettingSucceed)) return;
                m_SettingSucceed = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 设备信息
        /// </summary>
        public Howell.Net.NetworkDevice.Common.NetworkDeviceInformation NetworkDeviceInformation { get; set; }

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <param name="descriptionName"></param>
        /// <returns></returns>
        public static String GetPropertyName(String descriptionName)
        {
            if(descriptionName == "编号")
            {
                return "No";
            }
            else if(descriptionName == "名称")
            {
                return "Name";
            }
            else if(descriptionName == "IP地址")
            {
                return "IPAddress";
            }
            else if(descriptionName == "子网掩码")
            {
                return "SubnetMask";
            }
            else if (descriptionName == "端口号")
            {
                return "Port";
            }
            else if (descriptionName == "型号")
            {
                return "Model";
            }
            else if(descriptionName == "序列号")
            {
                return "SerialNumber";
            }
            else if (descriptionName == "默认网关")
            {
                return "Gateway";
            }
            else if (descriptionName == "MAC地址")
            {
                return "PhysicalAddress";
            }
            else if (descriptionName == "DHCP")
            {
                return "DHCP";
            }
            else if (descriptionName == "软件版本")
            {
                return "Software";
            }
            else if (descriptionName == "硬件版本")
            {
                return "Hardware";
            }
            else if (descriptionName == "用户名")
            {
                return "Username";
            }
            else if (descriptionName == "密码")
            {
                return "Password";
            }
            else if (descriptionName == "Dns1")
            {
                return "Dns1";
            }
            else if (descriptionName == "Dns2")
            {
                return "Dns2";
            }
            else if (descriptionName == "编码通道数量")
            {
                return "VideoChannelCount";
            }
            else if(descriptionName =="设备类型")
            {
                return "DeviceType";
            }
            return descriptionName;
        }
    }

   
}
