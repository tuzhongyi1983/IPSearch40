﻿using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IPSearch40.Excels
{
    /// <summary>
    /// Excel表单扩展函数
    /// </summary>
    public static  class ExcelTablesExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<T> ConvertTableToObjects<T>(this ExcelTable table) where T : new()
        {
            //DateTime Conversion
            var convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                if (excelDate < 1)
                    throw new ArgumentException("Excel dates cannot be smaller than 0.");

                var dateOfReference = new DateTime(1900, 1, 1);

                if (excelDate > 60d)
                    excelDate = excelDate - 2;
                else
                    excelDate = excelDate - 1;
                return dateOfReference.AddDays(excelDate);
            });
            var getPropertyName = new Func<PropertyInfo, String>(
                prop =>
                {
                    DisplayNameAttribute attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault() as DisplayNameAttribute;
                    return attribute?.DisplayName ?? prop.Name;
                });

            //Get the properties of T
            var tprops = (new T())
                .GetType()
                .GetProperties()
                .ToList();

            //Get the cells based on the table address
            var start = table.Address.Start;
            var end = table.Address.End;
            var cells = new List<ExcelRangeBase>();

            //Have to use for loops insteadof worksheet.Cells to protect against empties
            for (var r = start.Row; r <= end.Row; r++)
                for (var c = start.Column; c <= end.Column; c++)
                    cells.Add(table.WorkSheet.Cells[r, c]);

            var groups = cells
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Assume the second row represents column data types (big assumption!)
            //var types = groups
            //    .Skip(1)
            //    .First()
            //    .Select(rcell => rcell.Value.GetType())
            //    .ToList();

            var types = tprops.Select(x => new { Name = getPropertyName(x), Type = x.PropertyType }).ToList();

            //Assume first row has the column names
            var colnames = groups
                .First()
                .Select((hcell, idx) => new { Name = hcell.Value.ToString(), index = idx })
                .Where(o => tprops.Select(p => getPropertyName(p)).Contains(o.Name))
                .ToList();
            
            
            //Everything after the header is data
            var rowvalues = groups
                .Skip(1) //Exclude header
                .Select(cg => cg.Select(c => c.Value).ToList());

            //Create the collection container
            var collection = rowvalues
                .Select(row =>
                {
                    var tnew = new T();
                    colnames.ForEach(colname =>
                    {
                //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                var val = row[colname.index];
                        var type = types.First(p => p.Name == colname.Name).Type;//[colname.index];
                        var prop = tprops.First(p => getPropertyName(p) == colname.Name);

                //If it is numeric it is a double since that is how excel stores all numbers
                if (type == typeof(double) || type == typeof(Int32) || type == typeof(DateTime)||
                        type == typeof(double?) || type == typeof(Int32?) || type == typeof(DateTime?))
                        {
                            if (!string.IsNullOrWhiteSpace(val?.ToString()))
                            {
                                //Unbox it
                                var unboxedVal = (double)val;

                                //FAR FROM A COMPLETE LIST!!!
                                if (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Int32?))
                                    prop.SetValue(tnew, (int)unboxedVal);
                                else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
                                    prop.SetValue(tnew, unboxedVal);
                                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                                    prop.SetValue(tnew, convertDateTime(unboxedVal));
                                else
                                    throw new NotImplementedException(String.Format("Type '{0}' not implemented yet!", prop.PropertyType.Name));
                            }
                        }
                        else
                        {
                            //Its a string
                            prop.SetValue(tnew, val);
                        }
                    });

                    return tnew;
                });


            //Send it back
            return collection;
        }
    }
}
