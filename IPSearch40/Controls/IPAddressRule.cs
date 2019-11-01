using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IPSearch40.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class IPAddressRule : ValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            IPAddress ipAddress;
            if (!IPAddress.TryParse(value.ToString(), out ipAddress))
            {
                return new ValidationResult(false, "无效的IP地址");
            }
            return ValidationResult.ValidResult;
        }
    }
}
