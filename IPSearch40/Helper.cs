using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IPSearch40
{
    /// <summary>
    /// 工具
    /// </summary>
    public static class Helper
    {
        static Helper()
        {
            InitializeEnvironmentVariables();
            InitializeLogger();
            LoadModels(out CameraModels, "IPSearch40.DeviceModels.Camera.txt");
            LoadModels(out DVSModels, "IPSearch40.DeviceModels.DVS.txt");
            LoadModels(out FaceCameraModels, "IPSearch40.DeviceModels.FaceCamera.txt");
            LoadModels(out FaceNVRModels, "IPSearch40.DeviceModels.FaceNVR.txt");
            LoadModels(out HDDecoderModels, "IPSearch40.DeviceModels.HDDecoder.txt");
            LoadModels(out IntelligentCameraModels, "IPSearch40.DeviceModels.IntelligentCamera.txt");
            LoadModels(out MicroIntelligentNVRModels, "IPSearch40.DeviceModels.MicroIntelligentNVR.txt");
            LoadModels(out NVRModels, "IPSearch40.DeviceModels.NVR.txt");
            LoadModels(out VehicleCameraModels, "IPSearch40.DeviceModels.VehicleCamera.txt");
        }
        private static void InitializeLogger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(String.Format("{0}Logger.config", CurrentDirectory)));
            Logger = log4net.LogManager.GetLogger("Log4netRemotingServer");
        }

        private static void InitializeEnvironmentVariables()
        {
            foreach (String key in ConfigurationManager.AppSettings.Keys)
            {
                if (key == "CurrentDirectory")
                {
                    CurrentDirectory = ConfigurationManager.AppSettings[key];
                }
            }
            CurrentDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Threading.ThreadPool.SetMinThreads(256, 256);
        }

        #region Configurations

        private static void LoadModels(out Dictionary<String, String[]> list, String resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            list = new Dictionary<string, string[]>();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    String lineString = null;
                    while ((lineString = reader.ReadLine()) != null)
                    {
                        String[] models = lineString.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        if(list.ContainsKey(models[0])==false) list.Add(models[0], models);
                    }
                }
            }
        }
        private static Dictionary<String, String[]> CameraModels;
        private static Dictionary<String, String[]> DVSModels;
        private static Dictionary<String, String[]> FaceCameraModels;
        private static Dictionary<String, String[]> FaceNVRModels;
        private static Dictionary<String, String[]> HDDecoderModels;
        private static Dictionary<String, String[]> IntelligentCameraModels;
        private static Dictionary<String, String[]> MicroIntelligentNVRModels;
        private static Dictionary<String, String[]> NVRModels;
        private static Dictionary<String, String[]> VehicleCameraModels;

        private static Nullable<Boolean> m_AllDevices = null;
        /// <summary>
        /// Modbus报警器服务地址
        /// </summary>
        public static Boolean AllDevices
        {
            get
            {
                if (m_AllDevices == null)
                {
                    lock (typeof(Helper))
                    {
                        if (m_AllDevices == null)
                        {
                            m_AllDevices = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AllDevices"]);
                        }
                    }
                }
                return m_AllDevices??false;
            }
            set
            {
                m_AllDevices = value;
            }
        }
        #endregion


        /// <summary>
        /// 日志对象
        /// </summary>
        public static ILog Logger { get; private set; }
        /// <summary>
        /// 当前路径
        /// </summary>
        public static String CurrentDirectory { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allDevices"></param>
        /// <param name="isHowell"></param>
        /// <param name="classification"></param>
        /// <param name="howellModel"></param>
        /// <returns></returns>
        public static DeviceType GetDeviceType(String model, Boolean isHowell, Boolean allDevices, Howell.Industry.DeviceClassification classification, out String howellModel)
        {
            howellModel = null;
            if (ContainsModel(CameraModels, model, out howellModel) == true) return DeviceType.IPCamera;
            if (ContainsModel(DVSModels, model, out howellModel) == true) return DeviceType.DVS;
            if (ContainsModel(FaceCameraModels, model, out howellModel) == true) return DeviceType.FaceCamera;
            if (ContainsModel(FaceNVRModels, model, out howellModel) == true) return DeviceType.FaceNVR;
            if (ContainsModel(HDDecoderModels, model, out howellModel) == true) return DeviceType.HDDecoder;
            if (ContainsModel(IntelligentCameraModels, model, out howellModel) == true) return DeviceType.IntelligentCamera;
            if (ContainsModel(MicroIntelligentNVRModels, model, out howellModel) == true) return DeviceType.MicroIntelligentNVR;
            if (ContainsModel(NVRModels, model, out howellModel) == true) return DeviceType.NVR;
            if (ContainsModel(VehicleCameraModels, model, out howellModel) == true) return DeviceType.VehicleCamera;
            if (isHowell == true || allDevices == true)
            {
                howellModel = model;
                if (classification == Howell.Industry.DeviceClassification.NVR)
                    return DeviceType.NVR;
                else if (classification == Howell.Industry.DeviceClassification.IPCamera)
                    return DeviceType.IPCamera;
                else if (classification == Howell.Industry.DeviceClassification.DVS)
                    return DeviceType.DVS;
                else if (classification == Howell.Industry.DeviceClassification.DigitalMatrix||
                    classification == Howell.Industry.DeviceClassification.HDDecoder)
                    return DeviceType.HDDecoder;                
            }
            return DeviceType.None;
        }
        public static Boolean ContainsModel(Dictionary<String, String[]> list, String model, out String howellModel)
        {
            howellModel = null;
            if (model == null) return false;
            foreach (var key in list.Keys)
            {
                if (String.Equals(key, model) == true)
                {
                    howellModel = key;
                    return true;
                }
                foreach (var item in list[key])
                {
                    if (model.Contains(item) == true)
                    {
                        howellModel = key;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
