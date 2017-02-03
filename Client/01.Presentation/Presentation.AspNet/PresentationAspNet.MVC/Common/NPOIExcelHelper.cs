using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.SS.UserModel;

namespace PresentationAspNet.MVC
{
    public class NPOIExcelHelper
    {
        public static string GetCellValue(ICell cell, DataFormatter dataFormatter, IFormulaEvaluator formulaEvaluator)
        {
            try
            {
                if (cell != null && cell.CellType == CellType.Numeric)
                {
                    return cell.NumericCellValue.ToString();
                }
                var ret = dataFormatter.FormatCellValue(cell, formulaEvaluator);
                return ret; //.Replace("\n", ";"); // remove line break
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}