using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IPSearch40.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class PortRule : ValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int port;
            if (!int.TryParse(value.ToString(), out port))
            {
                return new ValidationResult(false, "无效的端口号");
            }
            if (port > 65535 || port <= 0)
            {
                return new ValidationResult(false, "端口号范围:1-65535");
            }
            return ValidationResult.ValidResult;
        }
    }
}
