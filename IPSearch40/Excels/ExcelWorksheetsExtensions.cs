using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.DataValidation.Contracts;
using System.Text.RegularExpressions;
using OfficeOpenXml.ConditionalFormatting;
using IPSearch40.Converters;

namespace IPSearch40.Excels
{
    /// <summary>
    /// Excel工作表单的扩展函数
    /// </summary>
    public static class ExcelWorksheetsExtensions
    {
        /// <summary>
        /// 添加摄像机信息表单
        /// </summary>
        /// <param name="worksheets">表单列表集合</param>
        /// <param name="list">摄像机列表</param>
        /// <param name="tableStyles">表单样式</param>
        public static void AddDeviceWorkSheet(this ExcelWorksheets worksheets, IList<DeviceExcelData> list, OfficeOpenXml.Table.TableStyles tableStyles = OfficeOpenXml.Table.TableStyles.Light8)
        {
            ExcelWorksheet worksheet = worksheets.Add("设备信息");

            var dataRange = worksheet.Cells["A1"].LoadFromCollection(
                list.OrderBy(x => x.Name).OrderBy(x => x.No),
                true, tableStyles);
            worksheet.DataValidations.AddUniqueValidation("A2:A65536");
            worksheet.DataValidations.AddListValidation("C2:C65536",DeviceTypeConverter.GetValues());
            worksheet.DataValidations.AddListValidation("K2:K65536", Boolean2ChineseConverter.GetValues());
            worksheet.DataValidations.AddUniqueValidation("E2:E65536");
            worksheet.DataValidations.AddUniqueValidation("N2:N65536");            
            //worksheet.Column(0).Style.Locked = true;
            //worksheet.Column(13).Style.Locked = true;
            //worksheet.Column(14).Style.Locked = true;
            //worksheet.Column(15).Style.Locked = true;
            //worksheet.Column(16).Style.Locked = true;
            //worksheet.Column(17).Style.Locked = true;
            //worksheet.Protection.IsProtected = true;
            dataRange.AutoFitColumns();
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="worksheets"></param>
        /// <param name="name"></param>
        /// <param name="list"></param>
        /// <param name="tableStyles"></param>
        public static void AddDataToWorkSheet<T>(this ExcelWorksheets worksheets,String name, IList<T> list, OfficeOpenXml.Table.TableStyles tableStyles = OfficeOpenXml.Table.TableStyles.Light8)
        {
            ExcelWorksheet worksheet = worksheets.Add(name);
            var dataRange = worksheet.Cells["A1"].LoadFromCollection(
                list,
                true, tableStyles);
            dataRange.AutoFitColumns();
            return;
        }

        /// <summary>
        /// 获取摄像机数据
        /// </summary>
        /// <param name="worksheets">表单列表集合</param>
        /// <returns></returns>
        public static IList<DeviceExcelData> GetDeviceExcelDatas(this ExcelWorksheets worksheets)
        {
            ExcelWorksheet worksheet = worksheets.Where(x => x.Name == "设备信息").FirstOrDefault();
            if (worksheet == null) return null;
            IList<DeviceExcelData> result = worksheet.Tables.FirstOrDefault()?.ConvertTableToObjects<DeviceExcelData>()?.ToList();
            if (result == null) return null;
            return result;
        }
    }
}
