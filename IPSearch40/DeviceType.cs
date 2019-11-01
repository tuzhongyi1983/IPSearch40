using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IPSearch40
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        [XmlEnum("未知类型")]
        None = 0,
        /// <summary>
        /// IP摄像机
        /// </summary>
        [XmlEnum("IP摄像机")]
        IPCamera = 1,
        /// <summary>
        /// NVR
        /// </summary>
        [XmlEnum("NVR")]
        NVR = 2,
        /// <summary>
        /// DVS
        /// </summary>
        [XmlEnum("DVS")]
        DVS = 3,
        /// <summary>
        /// 高清解码器
        /// </summary>
        [XmlEnum("高清解码器")]
        HDDecoder = 4,
        /// <summary>
        /// 人脸抓拍摄像机
        /// </summary>
        [XmlEnum("人脸抓拍摄像机")]
        FaceCamera = 5,
        /// <summary>
        /// 智能摄像机
        /// </summary>
        [XmlEnum("智能摄像机")]
        IntelligentCamera = 6,
        /// <summary>
        /// 轻智能NVR
        /// </summary>
        [XmlEnum("轻智能NVR")]
        MicroIntelligentNVR = 7,
        /// <summary>
        /// 智能NVR
        /// </summary>
        [XmlEnum("智能NVR")]
        FaceNVR = 8,
        /// <summary>
        /// 车辆抓拍摄像机
        /// </summary>
        [XmlEnum("车辆抓拍摄像机")]
        VehicleCamera = 9,
        /// <summary>
        /// 全部
        /// </summary>
        [XmlEnum("全部")]
        All= 0xFF,
    }
}
