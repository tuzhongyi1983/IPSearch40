using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IPSearch40.Models
{
    /// <summary>
    /// 设备IP设置模型
    /// </summary>
    public class DeviceIPSettingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
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
        private String m_IPAddressText;
        /// <summary>
        /// IP地址
        /// </summary>
        [DisplayName("IP地址名称")]
        public String IPAddressText
        {
            get { return m_IPAddressText; }
            set
            {
                if (Equals(value, m_IPAddressText)) return;
                m_IPAddressText = value;
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

        private Boolean m_OSDNameEnabled = false;
        /// <summary>
        /// 是否启用OSD名称修改
        /// </summary>
        [DisplayName("OSDNameEnabled")]
        public Boolean OSDNameEnabled
        {
            get { return m_OSDNameEnabled; }
            set
            {
                if (Equals(value, m_OSDNameEnabled)) return;
                m_OSDNameEnabled = value;
                OnPropertyChanged();
            }
        }
        private String m_OSDName;
        /// <summary>
        /// OSD名称
        /// </summary>
        [DisplayName("OSD名称")]
        public String OSDName
        {
            get { return m_OSDName; }
            set
            {
                if (Equals(value, m_OSDName)) return;
                m_OSDName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 验证是否有效
        /// </summary>
        /// <returns></returns>
        public ValidationResult Validate()
        {
            if (String.IsNullOrEmpty(Password) == true)
                return new ValidationResult(false,"密码不能为空");
            System.Net.IPAddress ipAddress = null;
            if (!System.Net.IPAddress.TryParse(this.IPAddress, out ipAddress))
                return new ValidationResult(false, "无效的IP地址");
            System.Net.IPAddress subnetMask = null;
            if (!System.Net.IPAddress.TryParse(this.SubnetMask, out subnetMask))
                return new ValidationResult(false, "无效的子网掩码");
            System.Net.IPAddress gateway = null;
            if (!System.Net.IPAddress.TryParse(this.Gateway, out gateway))
                return new ValidationResult(false, "无效的网关");
            if (this.Port <=0 || this.Port > 65535)
                return new ValidationResult(false, "无效的端口号");
            return ValidationResult.ValidResult;
        }
    }
}
