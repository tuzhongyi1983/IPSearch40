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
    public class PasswordRule : ValidationRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(String.IsNullOrEmpty(value?.ToString()))
            {
                return new ValidationResult(false, "密码不能为空");
            }
            return ValidationResult.ValidResult;
        }
    }
}
