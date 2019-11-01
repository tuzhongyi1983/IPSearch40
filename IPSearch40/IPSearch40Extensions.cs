using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPSearch40
{
    /// <summary>
    /// 
    /// </summary>
    public static class IPSearch40Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this System.Net.IPAddress ipAddress)
        {
            Byte [] bytes = ipAddress.GetAddressBytes();            
            return Convert.ToUInt32(((UInt32)bytes[0] << 24) + ((UInt32)bytes[1] << 16) + ((UInt32)bytes[2] << 8) + (UInt32)bytes[3]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns></returns>
        public static System.Net.IPAddress ToIPAddress(this UInt32 intValue)
        {
            return System.Net.IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}", (intValue >> 24) & 0xFF, (intValue >> 16) & 0xFF, (intValue >> 8) & 0xFF, intValue & 0xFF));
        }
        
    }
}
