using OfficeOpenXml.DataValidation;
using OfficeOpenXml.DataValidation.Contracts;
using System;

namespace IPSearch40.Excels
{
    /// <summary>
    /// 数据验证扩展功能
    /// </summary>
    public static class DataValidationsExtensions
    {
        /// <summary>
        /// 添加列表数据验证
        /// </summary>
        /// <param name="collection">数据验证集合</param>
        /// <param name="address">有效作用域，如：A:A，A2:A65535</param>
        /// <param name="listData">列表数据集合</param>
        /// <param name="errorStyle">验证失败提示样式</param>
        /// <param name="allowBlank">数据是否允许为空</param>
        /// <param name="showErrorMessage">是否显示验证失败的提示消息</param>
        /// <param name="error">验证失败提示消息</param>
        /// <returns>返回列表数据验证对象</returns>
        public static IExcelDataValidationList AddListValidation(this ExcelDataValidationCollection collection,
            String address, String [] listData, ExcelDataValidationWarningStyle errorStyle = ExcelDataValidationWarningStyle.stop,
            Boolean allowBlank = true,Boolean showErrorMessage = true,
            String error = null)
        {
            IExcelDataValidationList validationList = collection.AddListValidation(address);
            foreach (var data in listData)
            {
                validationList.Formula.Values.Add(data);
            }
            validationList.AllowBlank = allowBlank;
            validationList.ErrorStyle = errorStyle;
            validationList.ShowErrorMessage = showErrorMessage;
            validationList.Error = error??String.Format("只允许数据内容[{0}]",listData.ToSqlDataString());
            return validationList;
        }
        /// <summary>
        /// 添加范围整数的数据验证
        /// </summary>
        /// <param name="collection">数据验证集合</param>
        /// <param name="address">有效作用域，如：A:A，A2:A65535</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="errorStyle">验证失败提示样式</param>
        /// <param name="allowBlank">数据是否允许为空</param>
        /// <param name="showErrorMessage">是否显示验证失败的提示消息</param>
        /// <param name="error">验证失败提示消息</param>
        /// <returns>返回范围整数的数据验证对象</returns>
        public static IExcelDataValidationInt AddIntegerRangeValidation(this ExcelDataValidationCollection collection,
           String address, Int32 minValue, Int32 maxValue, ExcelDataValidationWarningStyle errorStyle = ExcelDataValidationWarningStyle.stop,
            Boolean allowBlank = true, Boolean showErrorMessage = true,
            String error = null)
        {
            IExcelDataValidationInt intValidation = collection.AddIntegerValidation(address);
            intValidation.Operator = ExcelDataValidationOperator.between;
            intValidation.Formula.Value = minValue;
            intValidation.Formula2.Value = maxValue;
            intValidation.AllowBlank = allowBlank;
            intValidation.ShowErrorMessage = showErrorMessage;
            intValidation.Error = error ?? String.Format("数值范围[{0},{1}]", intValidation.Formula.Value, intValidation.Formula2.Value);
            return intValidation;
        }
        /// <summary>
        /// 添加ID数据验证
        /// </summary>
        /// <param name="collection">数据验证集合</param>
        /// <param name="address">有效作用域，如：A:A，A2:A65535</param>
        /// <param name="errorStyle">验证失败提示样式</param>
        /// <param name="allowBlank">数据是否允许为空</param>
        /// <param name="showErrorMessage">是否显示验证失败的提示消息</param>
        /// <param name="error">验证失败提示消息</param>
        /// <returns></returns>
        public static IExcelDataValidationCustom AddIdentityValidation(this ExcelDataValidationCollection collection,
           String address, ExcelDataValidationWarningStyle errorStyle = ExcelDataValidationWarningStyle.stop,
            Boolean allowBlank = true, Boolean showErrorMessage = true,
            String error = null)
        {
            String dataRangeColumn = address.Substring(0, 1);
            Int32 indexOfSplit = address.IndexOf(":");
            String startRange = address.Substring(0, indexOfSplit == -1 ? address.Length : indexOfSplit);
            var customValidation = collection.AddCustomValidation(address);
            customValidation.Formula.ExcelFormula = String.Format("=AND(COUNTIF({0}:{0},{1})=1,ISNUMBER(SUMPRODUCT(SEARCH(MID({1},ROW(INDIRECT(\"1:\"&LEN({1}))),1),\"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'-_\"))))",dataRangeColumn,startRange);
            customValidation.AllowBlank = allowBlank;
            customValidation.ShowErrorMessage = showErrorMessage;
            customValidation.Error = "ID格式不正确或重复，格式必须是[a-z,0-9,A-Z,_-]的组合";
            return customValidation;
        }

        public static IExcelDataValidationCustom AddUniqueValidation(this ExcelDataValidationCollection collection,
           String address, ExcelDataValidationWarningStyle errorStyle = ExcelDataValidationWarningStyle.stop,
            Boolean allowBlank = true, Boolean showErrorMessage = true,
            String error = null)
        {
            String dataRangeColumn = address.Substring(0, 1);
            Int32 indexOfSplit = address.IndexOf(":");
            String startRange = address.Substring(0, indexOfSplit == -1 ? address.Length : indexOfSplit);
            var customValidation = collection.AddCustomValidation(address);
            customValidation.Formula.ExcelFormula = String.Format("=COUNTIF({0}:{0},{1})=1", dataRangeColumn, startRange);
            customValidation.AllowBlank = allowBlank;
            customValidation.ShowErrorMessage = showErrorMessage;
            customValidation.Error = "数据项重复";
            return customValidation;
        }
        /// <summary>
        /// 添加范围小数的数据验证
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="address"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="errorStyle"></param>
        /// <param name="allowBlank"></param>
        /// <param name="showErrorMessage"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IExcelDataValidationDecimal AddDoubleRangeValidation(this ExcelDataValidationCollection collection,
           String address,Double minValue,Double maxValue,
           ExcelDataValidationWarningStyle errorStyle = ExcelDataValidationWarningStyle.stop,
            Boolean allowBlank = true, Boolean showErrorMessage = true,
            String error = null)
        {
            IExcelDataValidationDecimal excelDataValidation = collection.AddDecimalValidation(address);
            excelDataValidation.Operator = ExcelDataValidationOperator.between;
            excelDataValidation.Formula.Value = minValue;
            excelDataValidation.Formula2.Value = maxValue;
            excelDataValidation.AllowBlank = allowBlank;            
            excelDataValidation.ShowErrorMessage = showErrorMessage;
            excelDataValidation.Error = error ?? String.Format("数值范围[{0},{1}]", excelDataValidation.Formula.Value, excelDataValidation.Formula2.Value);
            return excelDataValidation;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="address"></param>
        /// <param name="maxLength"></param>
        /// <param name="errorStyle"></param>
        /// <param name="allowBlank"></param>
        /// <param name="showErrorMessage"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IExcelDataValidationInt AddTextMaxLengthValidation(this ExcelDataValidationCollection collection,
            String address, Int32 maxLength, ExcelDataValidationWarningStyle errorStyle = ExcelDataValidationWarningStyle.stop,
            Boolean allowBlank = true, Boolean showErrorMessage = true,
            String error = null)
        {
            IExcelDataValidationInt excelDataValidation = collection.AddTextLengthValidation(address);
            excelDataValidation.Operator = ExcelDataValidationOperator.lessThanOrEqual;
            excelDataValidation.Formula.Value = maxLength;
            excelDataValidation.AllowBlank = allowBlank;
            excelDataValidation.ShowErrorMessage = showErrorMessage;
            excelDataValidation.Error = error ?? String.Format("数值长度必须小于{0}.", excelDataValidation.Formula.Value);
            return excelDataValidation;
        }
    }
}
