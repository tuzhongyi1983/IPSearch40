using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IPSearch40.Excels
{
    /// <summary>
    /// Excel文件导出器
    /// </summary>
    public static class ExcelPackageExporter
    {
        /// <summary>
        /// 导出摄像机Excel数据流
        /// </summary>
        /// <param name="list">摄像机Excel数据</param>
        /// <returns>返回数据流</returns>
        public static MemoryStream ExportDeviceExcelStream(IList<DeviceExcelData> list)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                pck.Workbook.Worksheets.AddDeviceWorkSheet(list);
                MemoryStream ms = new MemoryStream();
                pck.SaveAs(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms;
            }
        }

        /// <summary>
        /// 导出摄像机Excel数据流
        /// </summary>
        /// <param name="list">摄像机Excel数据</param>
        /// <param name="fileName">文件路径</param>
        public static void ExportDeviceExcelStream(IList<DeviceExcelData> list, String fileName)
        {
            using (MemoryStream ms = ExportDeviceExcelStream(list))
            {
                System.IO.File.WriteAllBytes(fileName, ms.ToArray());
            }
        }
        /// <summary>
        /// 导入摄像机Excel数据流
        /// </summary>
        /// <param name="stream">摄像机Excel数据流</param>
        /// <returns>返回摄像机Excel对象列表</returns>
        public static IList<DeviceExcelData> ImportDeviceExcelStream(System.IO.Stream stream)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                stream.Seek(0, SeekOrigin.Begin);
                pck.Load(stream);
                return pck.Workbook.Worksheets.GetDeviceExcelDatas();
            }
        }

        /// <summary>
        /// 导出摄像机Excel数据流
        /// </summary>
        /// <param name="list">摄像机Excel数据</param>
        /// <param name="fileName">文件路径</param>
        /// <returns>返回摄像机Excel对象列表</returns>
        public static IList<DeviceExcelData> ImportDeviceExcelStream(String fileName)
        {
            Byte[] buffer = System.IO.File.ReadAllBytes(fileName);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                return ImportDeviceExcelStream(ms);
            }
        }
    }
}
